using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class YFASScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalCategories = 0; // ���� �ߵ� ���ֿ� �ش��ϴ� ����
    public int totalScore { get; set; } // ����
    public Dictionary<string, string> ScoreData { get; private set; }
    private List<int> selectedAnswers = new List<int>(); // ����� ���� ����

    public QuestionRenderer questionRenderer;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;


    private readonly Dictionary<int, (int min, int max)> scoringRules = new Dictionary<int, (int, int)>
    {
        { 3, (0, 1) }, { 5, (0, 2) }, { 7, (0, 3) },  // ����: ���� 3, 5, 7�� ���� ���� ��Ģ
        { 1, (0, 4) }, { 2, (0, 4) }, { 24, (1, 0) }  // �߰����� ���� ��Ģ
    };

    private readonly int[][] categoryQuestions = new int[][] // �� ���ֿ� ���ϴ� ���� �׷��� �迭�� ����
    {
        new int[] { 1, 2, 3 },        // ���� 1
        new int[] { 4, 22, 24 },      // ���� 2
        new int[] { 5, 6, 7 },        // ���� 3
        new int[] { 8, 9, 10, 11 },   // ���� 4
        new int[] { 19 },             // ���� 5
        new int[] { 20, 21 },         // ���� 6
        new int[] { 12, 13, 14 },     // ���� 7
        new int[] { 15, 16 }          // ���� 8
    };

    public void AddScore(int questionIndex, int answerIndex)
    {
        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;

        int score = CalculateScore(questionIndex, answerIndex);
        questionScores[questionIndex] = score; // ���� ����
        selectedAnswers.Add(answerIndex); // ������ �亯 ���

        CalculateTotalScore();  // ���� ���
        CalculateCategoryScores(); // ���ֺ� ���� ������Ʈ

        Debug.Log($"Question {questionIndex} score: {score}");
        Debug.Log($"Total categories met: {totalCategories}");
        Debug.Log($"Total Score: {totalScore}"); // ���� ���
        //questionRenderer.scoreText.text = totalScore.ToString();

        // ���� �б⹮ �ڵ�
 
        if (totalCategories >= 3)
        {
            // 3�� �׸� �̻� - ���� �ߵ� ���豺
            level.text = "���� �ߵ� ���豺";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
            if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
        }
        else
        {
            // ����
            level.text = "����";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
            if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
        }

        notice.text = $"{name}���� ������ {totalScore}�� �Դϴ�.";

    }

    private int CalculateScore(int questionIndex, int answerIndex)
    {
        if (scoringRules.ContainsKey(questionIndex))
        {
            var (threshold, reverse) = scoringRules[questionIndex];
            return reverse == 1 ? (answerIndex == threshold ? 0 : 1) : (answerIndex >= threshold ? 1 : 0);
        }

        return answerIndex >= 3 ? 1 : 0; // �⺻ ��Ģ: 3�� �̻��̸� 1��
    }

    public void CalculateCategoryScores()
    {
        totalCategories = 0;

        foreach (var questions in categoryQuestions)
        {
            if (GetQuestionSum(questions) > 0)
            {
                totalCategories++;
            }
        }

        Debug.Log("Updated Total Categories: " + totalCategories);
    }

    private int GetQuestionSum(int[] questions)
    {
        int sum = 0;
        foreach (int question in questions)
        {
            if (questionScores.ContainsKey(question))
            {
                sum += questionScores[question];
            }
        }
        return sum;
    }

    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();
        questionRenderer.OtherTestComplete();
        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "YFAS");
        Debug.Log("Selected Answers:");
        for (int i = 0; i < selectedAnswers.Count; i++)
        {
            Debug.Log($"Question {i + 1}: Answer {selectedAnswers[i]}");
        }

    }
    public void ResetScores()
    {
        questionScores.Clear();
        selectedAnswers.Clear(); // ������ �亯 �ʱ�ȭ

        totalCategories = 0;
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

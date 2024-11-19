using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CbsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    private List<int> selectedAnswers = new List<int>(); // ����� ���� ����

    public int totalScore { get; set; } // ����

    public Dictionary<string, string> ScoreData { get; private set; }
    public QuestionRenderer questionRenderer;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;
    private float cutOffScore = -9.69f; // Cut-off ���� �ʱⰪ

    public void AddScore(int questionIndex, int answerIndex)
    {
        // ������ �ε����� ������ ����մϴ�.
        int score = answerIndex + 1; // �ε����� ���� ���� ���
        questionScores[questionIndex] = score; // ���� ����
        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;
        selectedAnswers.Add(answerIndex); // ������ �亯 ���


        // Cut-off ���� ���
        CalculateCutOffScore(questionIndex, answerIndex);

        // �� ���� ����
        CalculateTotalScore();

        // Cut-off ������ �������� ���� �б�
        if (cutOffScore > -1.34f)
        {
            // ���豺
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

    private void CalculateCutOffScore(int questionIndex, int answerIndex)
    {
        // Cut-off ������ ���
        switch (questionIndex)
        {
            case 0: // Q1a
                cutOffScore += (answerIndex * 0.33f);
                break;
            case 1: // Q2a
                cutOffScore += (answerIndex * 0.34f);
                break;
            case 2: // Q2b
                cutOffScore += (answerIndex * 0.50f);
                break;
            case 3: // Q2c
                cutOffScore += (answerIndex * 0.47f);
                break;
            case 4: // Q2d
                cutOffScore += (answerIndex * 0.33f);
                break;
            case 5: // Q2e
                cutOffScore += (answerIndex * 0.38f);
                break;
            case 6: // Q2f
                cutOffScore += (answerIndex * 0.31f);
                break;
        }

        Debug.Log("Cut-off Score: " + cutOffScore);
    }

    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        Debug.Log("Total Score: " + totalScore);
        questionRenderer.scoreText.text = totalScore.ToString();
    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();

        questionRenderer.OtherTestComplete();
        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "CBS");
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

        totalScore = 0;
        cutOffScore = -9.69f; // Cut-off ���� �ʱ�ȭ*/
        Debug.Log("Scores Reset");
    }
}

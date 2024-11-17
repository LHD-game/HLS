using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SapsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalScore { get; set; } // ����
    public QuestionRenderer questionRenderer;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;

    public Dictionary<string, string> ScoreData { get; private set; }

    private int factor1Score = 0; // 1���� (�ϻ��Ȱ���) ����
    private int factor3Score = 0; // 3���� (�ݴ�) ����
    private int factor4Score = 0; // 4���� (����) ����

    private int[] factor1Questions = { 1, 5, 9, 12, 15 };
    private int[] factor3Questions = { 4, 8, 11, 14 };
    private int[] factor4Questions = { 3, 7, 10, 13 };

    public void AddScore(int questionIndex, int answerIndex)
    {
        // SAPS������ ������ �ε����� ������ ����մϴ�. (1~4��)
        int score = answerIndex + 1; // �ε����� ���� ���� ��� (1���� 4����)
        questionScores[questionIndex] = score; // ���� ����
        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;

        // �� ���� ����
        CalculateTotalScore();
        questionRenderer.scoreText.text = totalScore.ToString();

        // ���κ� ���� ����
        CalculateFactorScores();

        // ����� ���� �з�
        DetermineUserGroup();

    }

    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        // ������ ����� �α׷� ���
        Debug.Log("Total Score: " + totalScore);
    }

    private void CalculateFactorScores()
    {
        factor1Score = CalculateFactorSum(factor1Questions);
        factor3Score = CalculateFactorSum(factor3Questions);
        factor4Score = CalculateFactorSum(factor4Questions);

        Debug.Log($"Factor 1 Score: {factor1Score}, Factor 3 Score: {factor3Score}, Factor 4 Score: {factor4Score}");
    }

    private int CalculateFactorSum(int[] questions)
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

    private void DetermineUserGroup()
    {
        if (totalScore >= 44 && factor1Score >= 15 && factor3Score >= 13 && factor4Score >= 13)
        {
            // ������ ����ڱ�
            level.text = "������ ����ڱ�";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
            if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
        }
        else if ( (totalScore >= 40 && totalScore <= 43 ) || factor1Score >= 14)
        {
            // ������ ���� ����ڱ�
            level.text = "������ ���� ����ڱ�";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/dangerorange");
            if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����

        }
        else if (totalScore <= 39 && factor1Score <= 13 && factor3Score <= 12 && factor4Score <= 12)
        {
            // �Ϲ� ����ڱ�
            level.text = "�Ϲ� ����ڱ�";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
            if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
        }
        else
        {
            // �������� ��� (���ǿ� ��� �ش����� �ʴ� ��� ó��)
          
            Debug.Log("User Group: �з� �Ұ�");
        }

        notice.text = $"{name}���� ������ {totalScore}�� �Դϴ�.";
    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();

        questionRenderer.OtherTestComplete();
        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "SAPS");
    }
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        factor1Score = 0;
        factor3Score = 0;
        factor4Score = 0;
        Debug.Log("Scores Reset");
    }
}

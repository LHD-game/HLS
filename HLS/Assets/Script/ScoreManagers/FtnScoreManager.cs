using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class FtnScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalScore { get; set; } // ����

    public QuestionRenderer questionRenderer;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;

    public Dictionary<string, object> ScoreData { get; private set; }


    private void Start()
    {
        if (questionRenderer != null && questionRenderer.scoreText != null)
        {
            questionRenderer.scoreText.text = totalScore.ToString(); // �ʱⰪ ����
        }
    }

    public void AddScore(int questionIndex, int answerIndex)
    {
        // 0��, 1��, 2��, 3��
        int score = answerIndex; // �ε����� ���� ���� ���
        questionScores[questionIndex] = score; // ���� ����

        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;
        // �� ���� ����
        CalculateTotalScore();

        if (totalScore >= 7 )
        {
            level.text = "���豺";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
            if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
        }

        else
        {
            level.text = "����";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
            if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
        }

        notice.text = $"{name}���� ������ {totalScore}�� �Դϴ�.";
    }
 

    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        Debug.Log("Total Score: " + totalScore);
        Debug.Log(totalScore.ToString());
        questionRenderer.scoreText.text = totalScore.ToString(); // ���� ���� �ݿ�
    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, object>();

        //questionRenderer.OtherTestComplete();
        ScoreData.Add("total", totalScore.ToString());
        rd.addData(ScoreData, "FTND");
    }
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

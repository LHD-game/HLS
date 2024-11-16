using UnityEngine;
using System.Collections.Generic;

public class FtnScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalScore = 0; // ����

    public QuestionRenderer questionRenderer;

    public Dictionary<string, string> ScoreData { get; private set; }


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

        // �� ���� ����
        CalculateTotalScore();
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
        ScoreData = new Dictionary<string, string>();

        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "FTND");
    }
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

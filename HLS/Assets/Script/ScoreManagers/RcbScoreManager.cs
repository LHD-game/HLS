using UnityEngine;
using System.Collections.Generic;

public class RcbScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    private List<int> selectedAnswers = new List<int>(); // ����� ���� ����

    public int totalScore { get; set; } // ����

    public Dictionary<string, string> ScoreData { get; private set; }

    public QuestionRenderer questionRenderer;
    

    public void AddScore(int questionIndex, int answerIndex)
    {
        // CSV ���Ͽ��� ������ ó���ϴ� ������� ����
        int score = answerIndex + 1; // �ε��� ������� ���� ��� (0~6 -> 1~7��)
        questionScores[questionIndex] = score; // ���� ����
        selectedAnswers.Add(answerIndex); // ������ �亯 ���

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
        rd.addotherData(ScoreData, "RCBS");
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
        Debug.Log("Scores Reset");
    }
}

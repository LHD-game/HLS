using UnityEngine;
using System.Collections.Generic;

public class RcbScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalScore = 0; // ����

    public Dictionary<string, string> ScoreData { get; private set; }

    public void AddScore(int questionIndex, int answerIndex)
    {
        // CSV ���Ͽ��� ������ ó���ϴ� ������� ����
        int score = answerIndex + 1; // �ε��� ������� ���� ��� (0~6 -> 1~7��)
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
    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();

        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "RCBS");
    }
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

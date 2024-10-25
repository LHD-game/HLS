using UnityEngine;
using System.Collections.Generic;

public class FtnScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    private FtnCsvReader csvReader; // FTND CSV �����͸� �о���� ���� ����
    public int totalScore = 0; // ����

    public void AddScore(int questionIndex, int answerIndex)
    {
        // FTND������ ������ �ε����� ������ ����մϴ�. (0~3��)
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
    }

    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

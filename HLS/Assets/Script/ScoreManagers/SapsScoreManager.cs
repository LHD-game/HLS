using UnityEngine;
using System.Collections.Generic;

public class SapsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalScore = 0; // ����

    public void AddScore(int questionIndex, int answerIndex)
    {
        // SAPS������ ������ �ε����� ������ ����մϴ�. (1~4��)
        int score = answerIndex + 1; // �ε����� ���� ���� ��� (1���� 4����)
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

        // ������ ����� �α׷� ���
        Debug.Log("Total Score: " + totalScore);
    }

    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

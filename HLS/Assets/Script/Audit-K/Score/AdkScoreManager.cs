using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdkScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalScore = 0; // ����

    // ���� �߰� (������ �ε����� �״�� ������ ���)
    public void AddScore(int questionIndex, int answerIndex)
    {
        // ������ �ε����� ������ ó�� (0������ ����)
        int score = answerIndex; // ������ �ε����� �״�� ������ ���
        questionScores[questionIndex] = score; // �ش� ������ ������ ����

        // �� ���� ����
        CalculateTotalScore();
    }

    // ���� ���
    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        // ���� ����� �޽��� ���
        Debug.Log("Total Score: " + totalScore);
    }

    // ���� �ʱ�ȭ
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

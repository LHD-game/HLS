using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HlsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    [SerializeField] public int totalScore = 0; // ����

    // ���� �߰� (������ �ε����� �״�� ������ ���)
    public void AddScore(int questionIndex, int answerIndex)
    {
        // ������ �ε����� ������ ó�� (1~4��)
        int score = answerIndex + 1;
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

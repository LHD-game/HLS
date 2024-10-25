using UnityEngine;
using System.Collections.Generic;

public class CbsScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    private CbsCsvReader csvReader; // CBS CSV �����͸� �о���� ���� ����
    public int totalScore = 0; // ����
    private float cutOffScore = -9.69f; // Cut-off ���� �ʱⰪ

    public void AddScore(int questionIndex, int answerIndex)
    {
        // ������ �ε����� ������ ����մϴ�.
        int score = answerIndex; // �ε����� ���� ���� ���
        questionScores[questionIndex] = score; // ���� ����

        // Cut-off ���� ���
        CalculateCutOffScore(questionIndex, answerIndex);

        // �� ���� ����
        CalculateTotalScore();
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
    }

    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        cutOffScore = -9.69f; // Cut-off ���� �ʱ�ȭ
        Debug.Log("Scores Reset");
    }
}

using UnityEngine;
using System.Collections.Generic;

public class CbsScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    private CbsCsvReader csvReader; // CBS CSV 데이터를 읽어오기 위한 변수
    public int totalScore = 0; // 총점
    private float cutOffScore = -9.69f; // Cut-off 점수 초기값

    public void AddScore(int questionIndex, int answerIndex)
    {
        // 선택지 인덱스를 점수로 사용합니다.
        int score = answerIndex; // 인덱스에 따른 점수 계산
        questionScores[questionIndex] = score; // 점수 저장

        // Cut-off 점수 계산
        CalculateCutOffScore(questionIndex, answerIndex);

        // 총 점수 갱신
        CalculateTotalScore();
    }

    private void CalculateCutOffScore(int questionIndex, int answerIndex)
    {
        // Cut-off 방정식 계산
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
        cutOffScore = -9.69f; // Cut-off 점수 초기화
        Debug.Log("Scores Reset");
    }
}

using UnityEngine;
using System.Collections.Generic;

public class RcbScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    public int totalScore = 0; // 총점

    public void AddScore(int questionIndex, int answerIndex)
    {
        // CSV 파일에서 점수를 처리하는 방식으로 변경
        int score = answerIndex + 1; // 인덱스 기반으로 점수 계산 (0~6 -> 1~7점)
        questionScores[questionIndex] = score; // 점수 저장

        // 총 점수 갱신
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

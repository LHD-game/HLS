using UnityEngine;
using System.Collections.Generic;

public class FtnScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    private FtnCsvReader csvReader; // FTND CSV 데이터를 읽어오기 위한 변수
    public int totalScore = 0; // 총점

    public void AddScore(int questionIndex, int answerIndex)
    {
        // FTND에서는 선택지 인덱스를 점수로 사용합니다. (0~3점)
        int score = answerIndex; // 인덱스에 따른 점수 계산
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

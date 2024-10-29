using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HlsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    [SerializeField] public int totalScore = 0; // 총점

    // 점수 추가 (선택지 인덱스를 그대로 점수로 사용)
    public void AddScore(int questionIndex, int answerIndex)
    {
        // 선택지 인덱스를 점수로 처리 (1~4점)
        int score = answerIndex + 1;
        questionScores[questionIndex] = score; // 해당 질문의 점수를 저장

        // 총 점수 갱신
        CalculateTotalScore();
    }

    // 총점 계산
    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        // 총점 디버그 메시지 출력
        Debug.Log("Total Score: " + totalScore);
    }

    // 점수 초기화
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

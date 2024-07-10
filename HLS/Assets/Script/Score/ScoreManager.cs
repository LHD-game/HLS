using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText; // 점수를 표시할 UI 텍스트
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수를 저장할 딕셔너리

    void Start()
    {
        UpdateScoreText();
    }

    // 특정 질문에 대한 점수를 추가하는 메소드
    public void AddScore(int questionIndex, int points)
    {
        Debug.Log($"Adding score for question {questionIndex}: {points}");
        // 해당 질문에 대한 점수를 업데이트
        questionScores[questionIndex] = points;

        // 총 점수를 업데이트
        UpdateScoreText();
    }

    // 점수 텍스트를 업데이트하는 메소드
    private void UpdateScoreText()
    {
        int totalScore = 0;

        // 총 점수 계산
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        ScoreText.text = "전체 점수 : " + totalScore;
        Debug.Log($"Updated total score: {totalScore}");
    }

    // 점수를 초기화하고 첫 번째 질문과 키워드를 다시 렌더링하는 메소드
    public void Restart(QuestionRenderer questionRenderer)
    {
        questionScores.Clear(); // 점수 초기화
        UpdateScoreText(); // 텍스트 업데이트
        questionRenderer.ResetQuestions(); // 첫 번째 질문과 키워드로 돌아가기
    }
}

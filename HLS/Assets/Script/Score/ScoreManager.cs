using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText; // 점수를 표시할 UI 텍스트
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수를 저장할 딕셔너리
    public int[] scores = new int[9]; // 각 카테고리별 점수를 저장할 배열
    public int totalScore = 0;
    public bool goableToNext = false;
    private int totalQuestions = 36; // 총 질문 개수

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

        // 해당 질문의 카테고리 점수 업데이트
        int categoryIndex = (questionIndex - 1) / 4; // 4개 단위로 카테고리 인덱스를 결정
        scores[categoryIndex] = GetCategoryScore(categoryIndex);

        // 총 점수를 업데이트
        UpdateScoreText();
    }

    // 특정 카테고리의 총 점수를 계산하는 메소드
    private int GetCategoryScore(int categoryIndex)
    {
        int categoryScore = 0;
        int startIndex = categoryIndex * 4 + 1; // 각 카테고리의 시작 인덱스 (1부터 시작)
        for (int i = 0; i < 4; i++)
        {
            if (questionScores.ContainsKey(startIndex + i))
            {
                categoryScore += questionScores[startIndex + i];
            }
        }
        return categoryScore;
    }

    // 점수 텍스트를 업데이트하는 메소드
    private void UpdateScoreText()
    {
        totalScore = 0;

        // 총 점수 계산
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }
    }

    // 점수를 초기화하고 첫 번째 질문과 키워드를 다시 렌더링하는 메소드
    public void Restart(QuestionRenderer questionRenderer)
    {
        questionScores.Clear(); // 점수 초기화

        for (int i = 0; i < 9; i++)
        {
            scores[i] = 0;
        }

        UpdateScoreText(); // 텍스트 업데이트
        Time.timeScale = 1f;

    

        questionRenderer.ResetQuestions(); // 첫 번째 질문과 키워드로 돌아가기
    }
}

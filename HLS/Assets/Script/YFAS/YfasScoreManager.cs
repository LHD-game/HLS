using UnityEngine;
using System.Collections.Generic;

public class YFASScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    public int totalCategories = 0; // 음식 중독 범주에 해당하는 개수
    private readonly Dictionary<int, (int min, int max)> scoringRules = new Dictionary<int, (int, int)>
    {
        { 3, (0, 1) }, { 5, (0, 2) }, { 7, (0, 3) },  // 예시: 문항 3, 5, 7에 대한 점수 규칙
        { 1, (0, 4) }, { 2, (0, 4) }, { 24, (1, 0) }  // 추가적인 문항 규칙
    };

    // 각 범주에 속하는 질문 그룹을 배열로 관리
    private readonly int[][] categoryQuestions = new int[][]
    {
        new int[] { 1, 2, 3 },        // 범주 1
        new int[] { 4, 22, 24 },      // 범주 2
        new int[] { 5, 6, 7 },        // 범주 3
        new int[] { 8, 9, 10, 11 },   // 범주 4
        new int[] { 19 },             // 범주 5
        new int[] { 20, 21 },         // 범주 6
        new int[] { 12, 13, 14 },     // 범주 7
        new int[] { 15, 16 }          // 범주 8
    };

    // 점수를 추가하고 로그에 찍음
    public void AddScore(int questionIndex, int answerIndex)
    {
        int score = CalculateScore(questionIndex, answerIndex);
        questionScores[questionIndex] = score; // 점수 저장

        Debug.Log($"Question {questionIndex} score: {score}");

        CalculateCategoryScores(); // 범주별 점수 업데이트
        Debug.Log($"Total categories met: {totalCategories}");
    }

    // 점수를 계산하는 로직을 단순화
    private int CalculateScore(int questionIndex, int answerIndex)
    {
        if (scoringRules.ContainsKey(questionIndex))
        {
            var (threshold, reverse) = scoringRules[questionIndex];
            return reverse == 1 ? (answerIndex == threshold ? 0 : 1) : (answerIndex >= threshold ? 1 : 0);
        }

        return answerIndex >= 3 ? 1 : 0; // 기본 규칙: 3점 이상이면 1점
    }

    // 범주별 점수를 계산하여 음식 중독 가능성 판단
    public void CalculateCategoryScores()
    {
        totalCategories = 0;

        foreach (var questions in categoryQuestions)
        {
            if (GetQuestionSum(questions) > 0)
            {
                totalCategories++;
            }
        }

        Debug.Log("Updated Total Categories: " + totalCategories);
    }

    // 범주에 속한 문항들의 합산 점수 계산
    private int GetQuestionSum(int[] questions)
    {
        int sum = 0;
        foreach (int question in questions)
        {
            if (questionScores.ContainsKey(question))
            {
                sum += questionScores[question];
            }
        }
        return sum;
    }

    // 점수 초기화
    public void ResetScores()
    {
        questionScores.Clear();
        totalCategories = 0;
        Debug.Log("Scores Reset");
    }
}

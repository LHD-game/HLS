using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class YFASScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    public int totalCategories = 0; // 음식 중독 범주에 해당하는 개수
    public int totalScore { get; set; } // 총점

    public Dictionary<string, object> ScoreData { get; private set; }
    private List<int> selectedAnswers = new List<int>(); // 사용자 선택 저장


    public QuestionRenderer questionRenderer;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;


    private readonly Dictionary<int, (int min, int max)> scoringRules = new Dictionary<int, (int, int)>
    {
        { 3, (0, 1) }, { 5, (0, 2) }, { 7, (0, 3) },  // 예시: 문항 3, 5, 7에 대한 점수 규칙
        { 1, (0, 4) }, { 2, (0, 4) }, { 24, (1, 0) }  // 추가적인 문항 규칙
    };

    private readonly int[][] categoryQuestions = new int[][] // 각 범주에 속하는 질문 그룹을 배열로 관리
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

    public void AddScore(int questionIndex, int answerIndex)
    {
        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;

        int score = CalculateScore(questionIndex, answerIndex);
        questionScores[questionIndex] = score; // 점수 저장
        selectedAnswers.Add(answerIndex); // 선택한 답변 기록

        CalculateTotalScore();  // 총점 계산
        CalculateCategoryScores(); // 범주별 점수 업데이트

        Debug.Log($"Question {questionIndex} score: {score}");
        Debug.Log($"Total categories met: {totalCategories}");
        Debug.Log($"Total Score: {totalScore}"); // 총점 출력
        //questionRenderer.scoreText.text = totalScore.ToString();

        // 예시 분기문 코드
 
        if (totalCategories >= 3)
        {
            // 3개 항목 이상 - 음식 중독 위험군
            level.text = "음식 중독 위험군";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
            if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
        }
        else
        {
            // 정상
            level.text = "정상";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
            if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
        }

        notice.text = $"{name}님의 점수는 {totalScore}점 입니다.";

    }

    private int CalculateScore(int questionIndex, int answerIndex)
    {
        if (scoringRules.ContainsKey(questionIndex))
        {
            var (threshold, reverse) = scoringRules[questionIndex];
            return reverse == 1 ? (answerIndex == threshold ? 0 : 1) : (answerIndex >= threshold ? 1 : 0);
        }

        return answerIndex >= 3 ? 1 : 0; // 기본 규칙: 3점 이상이면 1점
    }

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

    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, object>();
        //questionRenderer.OtherTestComplete();
        ScoreData.Add("total", totalScore.ToString());
        Debug.Log("Selected Answers:");
        for (int i = 0; i < selectedAnswers.Count; i++)
        {
            Debug.Log($"Question {i + 1}: Answer {selectedAnswers[i]}");
        }

        rd.addData(ScoreData, "YFAS");

    }
    public void ResetScores()
    {
        questionScores.Clear();
        selectedAnswers.Clear(); // 선택한 답변 초기화

        totalCategories = 0;
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

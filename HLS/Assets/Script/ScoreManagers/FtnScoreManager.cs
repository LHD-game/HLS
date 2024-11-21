using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class FtnScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    public int totalScore { get; set; } // 총점

    public QuestionRenderer questionRenderer;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;

    public Dictionary<string, object> ScoreData { get; private set; }
    private List<int> selectedAnswers = new List<int>(); // 사용자 선택 저장



    private void Start()
    {
        if (questionRenderer != null && questionRenderer.scoreText != null)
        {
            questionRenderer.scoreText.text = totalScore.ToString(); // 초기값 설정
        }
    }

    public void AddScore(int questionIndex, int answerIndex)
    {
        // 0점, 1점, 2점, 3점
        int score = answerIndex; // 인덱스에 따른 점수 계산
        questionScores[questionIndex] = score; // 점수 저장

        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;
        selectedAnswers.Add(answerIndex); // 선택한 답변 기록

        // 총 점수 갱신
        CalculateTotalScore();

        if (totalScore >= 7 )
        {
            level.text = "위험군";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
            if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
        }

        else
        {
            level.text = "정상";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
            if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
        }

        notice.text = $"{name}님의 점수는 {totalScore}점 입니다.";
    }
 

    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        Debug.Log("Total Score: " + totalScore);
        Debug.Log(totalScore.ToString());
        questionRenderer.scoreText.text = totalScore.ToString(); // 최종 점수 반영
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

        rd.addData(ScoreData, "FTND");
    }
    public void ResetScores()
    {
        questionScores.Clear();
        selectedAnswers.Clear(); // 선택한 답변 초기화

        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

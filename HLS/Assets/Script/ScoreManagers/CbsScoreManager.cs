using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CbsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    private List<int> selectedAnswers = new List<int>(); // 사용자 선택 저장

    public int totalScore { get; set; } // 총점

    public Dictionary<string, string> ScoreData { get; private set; }
    public QuestionRenderer questionRenderer;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;
    private float cutOffScore = -9.69f; // Cut-off 점수 초기값

    public void AddScore(int questionIndex, int answerIndex)
    {
        // 선택지 인덱스를 점수로 사용합니다.
        int score = answerIndex + 1; // 인덱스에 따른 점수 계산
        questionScores[questionIndex] = score; // 점수 저장
        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;
        selectedAnswers.Add(answerIndex); // 선택한 답변 기록


        // Cut-off 점수 계산
        CalculateCutOffScore(questionIndex, answerIndex);

        // 총 점수 갱신
        CalculateTotalScore();

        // Cut-off 점수를 기준으로 상태 분기
        if (cutOffScore > -1.34f)
        {
            // 위험군
            level.text = "쇼핑 중독 위험군";
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
        questionRenderer.scoreText.text = totalScore.ToString();
    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();

        questionRenderer.OtherTestComplete();
        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "CBS");
        Debug.Log("Selected Answers:");

        for (int i = 0; i < selectedAnswers.Count; i++)
        {
            Debug.Log($"Question {i + 1}: Answer {selectedAnswers[i]}");
        }

    }
    public void ResetScores()
    {
        questionScores.Clear();
        selectedAnswers.Clear(); // 선택한 답변 초기화

        totalScore = 0;
        cutOffScore = -9.69f; // Cut-off 점수 초기화*/
        Debug.Log("Scores Reset");
    }
}

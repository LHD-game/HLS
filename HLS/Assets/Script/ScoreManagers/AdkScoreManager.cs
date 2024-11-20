using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using JetBrains.Annotations;
using UnityEngine.UI;

public class AdkScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    public int totalScore { get; set; } // 총점

    public QuestionRenderer questionRenderer;
    public string gender;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;

    public Dictionary<string, string> ScoreData { get; private set; }
    private List<int> selectedAnswers = new List<int>(); // 사용자 선택 저장


    // 점수 추가 (선택지 인덱스를 그대로 점수로 사용)
    public void AddScore(int questionIndex, int answerIndex)
    {
        // 선택지 인덱스를 점수로 처리 (0점부터 시작)
        int score = answerIndex; // 선택지 인덱스를 그대로 점수로 사용
        questionScores[questionIndex] = score; // 해당 질문의 점수를 저장
        gender = questionRenderer.Ugen; // 성별 정보 불러오기
        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;
        selectedAnswers.Add(answerIndex); // 선택한 답변 기록
        // 총 점수 갱신
        CalculateTotalScore();

        if (gender == "M")
        {
            if (totalScore <= 9)
            {
                level.text = "정상음주군";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
                if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
            }

            else if (totalScore <= 19)
            {
                level.text = "위험음주군";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/dangerorange");
                if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
            }

            else
            {
                level.text = "알코올 사용장애";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
                if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
            }
        }

        else
        {
            if (totalScore <= 5)
            {
                level.text = "정상음주군";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
                if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
            }

            else if (totalScore <= 9)
            {
                level.text = "위험음주군";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/dangerorange");
                if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
            }

            else
            {
                level.text = "알코올 사용장애";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
                if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용

            }
        }

        notice.text = $"{name}님의 점수는 {totalScore}점 입니다.";
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
        // UI에 점수 표시
        if (questionRenderer != null && questionRenderer.scoreText != null)
        {
            questionRenderer.scoreText.text = totalScore.ToString();
        }
        else
        {
            Debug.LogError("questionRenderer 또는 scoreText가 null입니다.");
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

        rd.addData(ScoreData, "AUDIT");
    }

    // 점수 초기화
    public void ResetScores()
    {
        questionScores.Clear();
        selectedAnswers.Clear(); // 선택한 답변 초기화
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

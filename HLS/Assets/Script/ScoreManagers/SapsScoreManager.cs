using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SapsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    public int totalScore { get; set; } // 총점
    public QuestionRenderer questionRenderer;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;

    public Dictionary<string, string> ScoreData { get; private set; }

    private int factor1Score = 0; // 1요인 (일상생활장애) 점수
    private int factor3Score = 0; // 3요인 (금단) 점수
    private int factor4Score = 0; // 4요인 (내성) 점수

    private int[] factor1Questions = { 1, 5, 9, 12, 15 };
    private int[] factor3Questions = { 4, 8, 11, 14 };
    private int[] factor4Questions = { 3, 7, 10, 13 };

    public void AddScore(int questionIndex, int answerIndex)
    {
        // SAPS에서는 선택지 인덱스를 점수로 사용합니다. (1~4점)
        int score = answerIndex + 1; // 인덱스에 따른 점수 계산 (1부터 4까지)
        questionScores[questionIndex] = score; // 점수 저장
        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;

        // 총 점수 갱신
        CalculateTotalScore();
        questionRenderer.scoreText.text = totalScore.ToString();

        // 요인별 점수 갱신
        CalculateFactorScores();

        // 사용자 상태 분류
        DetermineUserGroup();

    }

    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        // 총점을 디버그 로그로 출력
        Debug.Log("Total Score: " + totalScore);
    }

    private void CalculateFactorScores()
    {
        factor1Score = CalculateFactorSum(factor1Questions);
        factor3Score = CalculateFactorSum(factor3Questions);
        factor4Score = CalculateFactorSum(factor4Questions);

        Debug.Log($"Factor 1 Score: {factor1Score}, Factor 3 Score: {factor3Score}, Factor 4 Score: {factor4Score}");
    }

    private int CalculateFactorSum(int[] questions)
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

    private void DetermineUserGroup()
    {
        if (totalScore >= 44 && factor1Score >= 15 && factor3Score >= 13 && factor4Score >= 13)
        {
            // 고위험 사용자군
            level.text = "고위험 사용자군";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
            if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
        }
        else if ( (totalScore >= 40 && totalScore <= 43 ) || factor1Score >= 14)
        {
            // 잠재적 위험 사용자군
            level.text = "잠재적 위험 사용자군";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/dangerorange");
            if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용

        }
        else if (totalScore <= 39 && factor1Score <= 13 && factor3Score <= 12 && factor4Score <= 12)
        {
            // 일반 사용자군
            level.text = "일반 사용자군";
            Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
            if (newSprite != null) targetImage.sprite = newSprite; // 스프라이트 적용
        }
        else
        {
            // 예외적인 경우 (조건에 모두 해당하지 않는 경우 처리)
          
            Debug.Log("User Group: 분류 불가");
        }

        notice.text = $"{name}님의 점수는 {totalScore}점 입니다.";
    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();

        questionRenderer.OtherTestComplete();
        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "SAPS");
    }
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        factor1Score = 0;
        factor3Score = 0;
        factor4Score = 0;
        Debug.Log("Scores Reset");
    }
}

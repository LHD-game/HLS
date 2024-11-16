using UnityEngine;
using System.Collections.Generic;

public class FtnScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    public int totalScore = 0; // 총점

    public QuestionRenderer questionRenderer;

    public Dictionary<string, string> ScoreData { get; private set; }


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
        Debug.Log(totalScore.ToString());
        questionRenderer.scoreText.text = totalScore.ToString(); // 최종 점수 반영
    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();

        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "FTND");
    }
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

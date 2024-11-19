using UnityEngine;
using System.Collections.Generic;

public class RcbScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    private List<int> selectedAnswers = new List<int>(); // 사용자 선택 저장

    public int totalScore { get; set; } // 총점

    public Dictionary<string, string> ScoreData { get; private set; }

    public QuestionRenderer questionRenderer;
    

    public void AddScore(int questionIndex, int answerIndex)
    {
        // CSV 파일에서 점수를 처리하는 방식으로 변경
        int score = answerIndex + 1; // 인덱스 기반으로 점수 계산 (0~6 -> 1~7점)
        questionScores[questionIndex] = score; // 점수 저장
        selectedAnswers.Add(answerIndex); // 선택한 답변 기록

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
        rd.addotherData(ScoreData, "RCBS");
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
        Debug.Log("Scores Reset");
    }
}

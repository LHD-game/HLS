using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HlsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    public int totalScore { get; set; } // 총점

    public QuestionRenderer questionRenderer;

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;

    public Dictionary<string, object> ScoreData { get; private set; }

    // 점수 추가 (선택지 인덱스를 그대로 점수로 사용)
    public void AddScore(int questionIndex, int answerIndex)
    {
        // 선택지 인덱스를 점수로 처리 (1~4점)
        int score = answerIndex + 1;
        questionScores[questionIndex] = score; // 해당 질문의 점수를 저장

        // 총 점수 갱신
        CalculateTotalScore();
    }
    public void Test() //테스트 용 함수 이후 삭제
    {
        for (int i = 0; i < 36; i++)
        {
            AddScore(i, 2);
        }
        SetData();
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
        //Debug.Log("Total Score: " + totalScore);
        questionRenderer.scoreText.text = totalScore.ToString();
    }

    public void SetData()
    {
        ScoreData = new Dictionary<string, object>();
        int count = 0;
        int groupScore = 0;
        foreach (int score in questionScores.Values)
        {
            count++;
            groupScore += score;
            //Debug.Log($"count%4={count % 4}");
            if (count % 4 == 0)
            {
                //Debug.Log($"count/4={count / 4}");
                //Debug.Log($"{sd.header[(count / 4) - 1]}");
                if ((count / 4)-1 < 9)
                    ScoreData.Add(sd.header[(count / 4)-1], groupScore.ToString());
                groupScore = 0;
            }

        }

        ScoreData.Add("total", totalScore.ToString());

        rd.addData(ScoreData, "HLS");
    }

    // 점수 초기화
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        //Debug.Log("Scores Reset");
    }
}

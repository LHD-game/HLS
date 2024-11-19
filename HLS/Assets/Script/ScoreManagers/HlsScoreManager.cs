using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HlsScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    private List<int> selectedAnswers = new List<int>(); // ����� ���� ����

    public int totalScore { get; set; } // ����

    public QuestionRenderer questionRenderer;

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;

    public Dictionary<string, string> ScoreData { get; private set; }

    // ���� �߰� (������ �ε����� �״�� ������ ���)
    public void AddScore(int questionIndex, int answerIndex)
    {
        // ������ �ε����� ������ ó�� (1~4��)
        int score = answerIndex + 1;
        questionScores[questionIndex] = score; // �ش� ������ ������ ����
        selectedAnswers.Add(answerIndex); // ������ �亯 ���

        // �� ���� ����
        CalculateTotalScore();
    }
    public void Test() //�׽�Ʈ �� �Լ� ���� ����
    {
        for (int i = 0; i < 36; i++)
        {
            AddScore(i, 2);
        }
        SetData();
    }
    // ���� ���
    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        // ���� ����� �޽��� ���
        //Debug.Log("Total Score: " + totalScore);
        questionRenderer.scoreText.text = totalScore.ToString();
    }

    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();
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
        Debug.Log("Selected Answers:");
        for (int i = 0; i < selectedAnswers.Count; i++)
        {
            Debug.Log($"Question {i + 1}: Answer {selectedAnswers[i]}");
        }
    }

    // ���� �ʱ�ȭ
    public void ResetScores()
    {
        questionScores.Clear();
        selectedAnswers.Clear(); // ������ �亯 �ʱ�ȭ

        totalScore = 0;
        //Debug.Log("Scores Reset");
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdkScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalScore = 0; // ����

    public QuestionRenderer questionRenderer;

    public Dictionary<string, string> ScoreData { get; private set; }

    // ���� �߰� (������ �ε����� �״�� ������ ���)
    public void AddScore(int questionIndex, int answerIndex)
    {
        // ������ �ε����� ������ ó�� (0������ ����)
        int score = answerIndex; // ������ �ε����� �״�� ������ ���
        questionScores[questionIndex] = score; // �ش� ������ ������ ����

        // �� ���� ����
        CalculateTotalScore();
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
        Debug.Log("Total Score: " + totalScore);
        // UI�� ���� ǥ��
        if (questionRenderer != null && questionRenderer.scoreText != null)
        {
            questionRenderer.scoreText.text = totalScore.ToString();
        }
        else
        {
            Debug.LogError("questionRenderer �Ǵ� scoreText�� null�Դϴ�.");
        }

    }

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();

        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "AUDIT");
    }

    // ���� �ʱ�ȭ
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

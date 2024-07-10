using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText; // ������ ǥ���� UI �ؽ�Ʈ
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ������ ������ ��ųʸ�

    void Start()
    {
        UpdateScoreText();
    }

    // Ư�� ������ ���� ������ �߰��ϴ� �޼ҵ�
    public void AddScore(int questionIndex, int points)
    {
        Debug.Log($"Adding score for question {questionIndex}: {points}");
        // �ش� ������ ���� ������ ������Ʈ
        questionScores[questionIndex] = points;

        // �� ������ ������Ʈ
        UpdateScoreText();
    }

    // ���� �ؽ�Ʈ�� ������Ʈ�ϴ� �޼ҵ�
    private void UpdateScoreText()
    {
        int totalScore = 0;

        // �� ���� ���
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        ScoreText.text = "��ü ���� : " + totalScore;
        Debug.Log($"Updated total score: {totalScore}");
    }

    // ������ �ʱ�ȭ�ϰ� ù ��° ������ Ű���带 �ٽ� �������ϴ� �޼ҵ�
    public void Restart(QuestionRenderer questionRenderer)
    {
        questionScores.Clear(); // ���� �ʱ�ȭ
        UpdateScoreText(); // �ؽ�Ʈ ������Ʈ
        questionRenderer.ResetQuestions(); // ù ��° ������ Ű����� ���ư���
    }
}

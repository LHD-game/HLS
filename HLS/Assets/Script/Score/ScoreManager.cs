using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText; // ������ ǥ���� UI �ؽ�Ʈ
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ������ ������ ��ųʸ�
    public int[] scores = new int[9]; // �� ī�װ��� ������ ������ �迭
    public int totalScore = 0;

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

        // �ش� ������ ī�װ� ���� ������Ʈ
        int categoryIndex = (questionIndex - 1) / 4; // 4�� ������ ī�װ� �ε����� ����
        scores[categoryIndex] = GetCategoryScore(categoryIndex);

        // �� ������ ������Ʈ
        UpdateScoreText();
    }

    // Ư�� ī�װ��� �� ������ ����ϴ� �޼ҵ�
    private int GetCategoryScore(int categoryIndex)
    {
        int categoryScore = 0;
        int startIndex = categoryIndex * 4 + 1; // �� ī�װ��� ���� �ε��� (1���� ����)
        for (int i = 0; i < 4; i++)
        {
            if (questionScores.ContainsKey(startIndex + i))
            {
                categoryScore += questionScores[startIndex + i];
            }
        }
        return categoryScore;
    }

    // ���� �ؽ�Ʈ�� ������Ʈ�ϴ� �޼ҵ�
    private void UpdateScoreText()
    {
        totalScore = 0;

        // �� ���� ���
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        ScoreText.text =
            "��ü " + totalScore + "\n" +
            "�޺� " + scores[0] + "\n" +
            "�� " + scores[1] + "\n" +
            "���� " + scores[2] + "\n" +
            "���� " + scores[3] + "\n" +
            "� " + scores[4] + "\n" +
            "���繰 ���� " + scores[5] + "\n" +
            "���� " + scores[6] + "\n" +
            "���� " + scores[7] + "\n" +
            "�Ϲ����� ��ü ���� " + scores[8];
        Debug.Log($"Updated total score: {totalScore}");
    }

    // ������ �ʱ�ȭ�ϰ� ù ��° ������ Ű���带 �ٽ� �������ϴ� �޼ҵ�
    public void Restart(QuestionRenderer questionRenderer)
    {
        questionScores.Clear(); // ���� �ʱ�ȭ

        for (int i = 0; i < 9; i++)
        {
            scores[i] = 0;
        }

        UpdateScoreText(); // �ؽ�Ʈ ������Ʈ

        questionRenderer.ResetQuestions(); // ù ��° ������ Ű����� ���ư���
    }
}

using UnityEngine;
using System.Collections.Generic;

public class SapsScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    private SapsCsvReader csvReader; // SAPS CSV �����͸� �о���� ���� ����
    public int totalScore = 0; // ����

    void Start()
    {
        csvReader = GetComponent<SapsCsvReader>(); // SapsCsvReader�κ��� ������ �޾ƿ�

        if (csvReader == null)
        {
            Debug.LogError("CSVReader�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        if (csvReader.csvData.Count == 0)
        {
            Debug.LogError("CSV �����Ͱ� ����ֽ��ϴ�.");
            return;
        }

        InitializeScoreRules(); // ���� ��Ģ ����
    }

    private void InitializeScoreRules()
    {
        if (csvReader == null || csvReader.csvData.Count == 0)
        {
            Debug.LogError("CSV �����Ͱ� �����ϴ�.");
            return;
        }

        // ���� ��Ģ ���� ������ ������ �� ����
    }

    public void AddScore(int questionIndex, int answerIndex)
    {
        if (csvReader == null || questionIndex < 0 || questionIndex >= csvReader.csvData.Count)
        {
            Debug.LogError("�߸��� ���� �ε����Դϴ�.");
            return;
        }

        // SAPS������ ������ �ε����� ������ ����մϴ�. (1~4��)
        int score = answerIndex; // �ε����� ���� ���� ��� (1���� 4����)
        questionScores[questionIndex] = score; // ���� ����

        // �� ���� ����
        CalculateTotalScore();
    }

    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        // ������ ����� �α׷� ���
        Debug.Log("Total Score: " + totalScore);
    }

    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

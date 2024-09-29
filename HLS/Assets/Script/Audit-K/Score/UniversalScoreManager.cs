using UnityEngine;
using System.Collections.Generic;

public class UniversalScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    private AdkCsvReader csvReader; // CSV �����͸� �о���� ���� ����
    public int totalScore = 0; // ����

    void Start()
    {
        csvReader = GetComponent<AdkCsvReader>();

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

        for (int i = 0; i < csvReader.csvData.Count; i++)
        {
            string[] questionData = csvReader.csvData[i];
        }
    }

    public void AddScore(int questionIndex, int answerIndex)
    {
        if (csvReader == null || questionIndex < 0 || questionIndex >= csvReader.csvData.Count)
        {
            Debug.LogError("�߸��� ���� �ε����Դϴ�.");
            return;
        }

        // ������ �ε����� ������ ó��
        int score = answerIndex; // �ε����� ���� ���� ���
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

        Debug.Log("Total Score: " + totalScore);
    }

    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

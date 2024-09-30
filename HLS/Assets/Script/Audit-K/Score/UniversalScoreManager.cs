using UnityEngine;
using System.Collections.Generic;

public class UniversalScoreManager : MonoBehaviour
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // 각 질문에 대한 점수 저장
    private AdkCsvReader csvReader; // CSV 데이터를 읽어오기 위한 변수
    public int totalScore = 0; // 총점

    void Start()
    {
        csvReader = GetComponent<AdkCsvReader>();

        if (csvReader == null)
        {
            Debug.LogError("CSVReader가 할당되지 않았습니다.");
            return;
        }

        if (csvReader.csvData.Count == 0)
        {
            Debug.LogError("CSV 데이터가 비어있습니다.");
            return;
        }

        InitializeScoreRules(); // 점수 규칙 설정
    }

    private void InitializeScoreRules()
    {
        if (csvReader == null || csvReader.csvData.Count == 0)
        {
            Debug.LogError("CSV 데이터가 없습니다.");
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
            Debug.LogError("잘못된 질문 인덱스입니다.");
            return;
        }

        // 선택지 인덱스를 점수로 처리
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
    }

    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

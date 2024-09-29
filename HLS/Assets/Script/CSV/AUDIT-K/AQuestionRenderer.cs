using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AQuestionRenderer : MonoBehaviour
{
    public GameObject Keyword; // 키워드 프리팹
    public GameObject Q1; // 질문 프리팹 1
    public GameObject Q2; // 질문 프리팹 2
    public GameObject Q3; // 질문 프리팹 3
    public GameObject Q4; // 질문 프리팹 4
    public ScoreManager scoreManager; // ScoreManager 인스턴스

    private ACSVReader csvReader;
    private int currentQuestionIndex = 1;

    void Start()
    {
        csvReader = GetComponent<ACSVReader>();
        StartCoroutine(WaitForCSVData());
    }

    IEnumerator WaitForCSVData()
    {
        // CSV 데이터가 로드될 때까지 기다립니다.
        while (csvReader.csvData.Count == 0)
        {
            yield return null;
        }

        // 첫 화면을 렌더링합니다.
        RenderQuestions();
    }

    public void RenderQuestions()
    {
        int remainingQuestions = csvReader.csvData.Count - currentQuestionIndex;
        int questionsToRender = Mathf.Min(4, remainingQuestions); // 최대 4개의 질문 렌더링

        if (questionsToRender > 0)
        {
            // 첫 번째 질문 프리팹 설정
            if (currentQuestionIndex < csvReader.csvData.Count)
                SetupQuestionPrefab(Q1, currentQuestionIndex);

            // 두 번째 질문 프리팹 설정
            if (currentQuestionIndex + 1 < csvReader.csvData.Count)
                SetupQuestionPrefab(Q2, currentQuestionIndex + 1);

            // 세 번째 질문 프리팹 설정
            if (currentQuestionIndex + 2 < csvReader.csvData.Count)
                SetupQuestionPrefab(Q3, currentQuestionIndex + 2);

            // 네 번째 질문 프리팹 설정
            if (currentQuestionIndex + 3 < csvReader.csvData.Count)
                SetupQuestionPrefab(Q4, currentQuestionIndex + 3);
        }
    }

    private void SetupQuestionPrefab(GameObject prefab, int index)
    {
        Text questionText = prefab.GetComponentInChildren<Text>();
        questionText.text = csvReader.csvData[index][1]; // CSV에서 두 번째 열의 데이터로 질문 설정

        ButtonHandler[] buttonHandlers = prefab.GetComponentsInChildren<ButtonHandler>();
        foreach (var handler in buttonHandlers)
        {
            handler.Initialize(scoreManager);
            handler.questionIndex = index;
        }
    }

    public void ResetQuestions()
    {
        currentQuestionIndex = 1; // 질문 인덱스 초기화
        RenderQuestions(); // 첫 번째 질문 렌더링
    }

    public void NextQuestions()
    {
        if (csvReader.csvData.Count >= currentQuestionIndex + 4)
        {
            currentQuestionIndex += 4; // 다음 4개 질문으로 이동
            RenderQuestions();
        }
    }

    public void PreviousQuestions()
    {
        if (currentQuestionIndex > 4)
        {
            currentQuestionIndex -= 4; // 이전 4개 질문으로 이동
            RenderQuestions();
        }
    }
}

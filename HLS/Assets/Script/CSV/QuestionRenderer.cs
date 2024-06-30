using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class QuestionRenderer : MonoBehaviour
{
    public GameObject Keyword; // 키워드 프리팹
    public GameObject Q1; // 질문 프리팹 1
    public GameObject Q2; // 질문 프리팹 2
    public GameObject Q3; // 질문 프리팹 3
    public GameObject Q4; // 질문 프리팹 4
    public ScoreManager scoreManager; // ScoreManager 인스턴스

    private CSVReader csvReader;
    private int currentQuestionIndex = 1;
    private int currentKeywordIndex = 1;


    void Start()
    {
        csvReader = GetComponent<CSVReader>();
        StartCoroutine(WaitForCSVData());
    }

    IEnumerator WaitForCSVData()
    {
        // CSV 데이터가 로드될 때까지 기다립니다.
        while (csvReader.csvData.Count == 0)
        {
            yield return null;
        }
        // CSV 데이터가 로드되면 첫 화면을 렌더링합니다.
        RenderQuestions();
    }

    public void RenderQuestions()
    {
        if (csvReader.csvData.Count >= currentQuestionIndex + 4)
        {   
            // 키워드 프리팹 설정
            SetupKeywordPrefab(Keyword, currentKeywordIndex);

            // 첫 번째 질문 프리팹 설정
            SetupQuestionPrefab(Q1, currentQuestionIndex);

            // 두 번째 질문 프리팹 설정
            SetupQuestionPrefab(Q2, currentQuestionIndex + 1);

            // 세 번째 질문 프리팹 설정
            SetupQuestionPrefab(Q3, currentQuestionIndex + 2);

            // 네 번째 질문 프리팹 설정
            SetupQuestionPrefab(Q4, currentQuestionIndex + 3);

           

        }
    }

    private void SetupQuestionPrefab(GameObject prefab, int index)
    {
        Text questionText = prefab.GetComponentInChildren<Text>();
        questionText.text = csvReader.csvData[index][1]; // Question 열의 데이터


        ButtonHandler[] buttonHandlers = prefab.GetComponentsInChildren<ButtonHandler>();
        foreach (var handler in buttonHandlers)
        {
            handler.Initialize(scoreManager);
            handler.questionIndex = index;
            // handler.score = ... // 점수를 설정하는 로직 추가
        }
    }

    private void SetupKeywordPrefab(GameObject prefab, int index)
    {
        Text keywordText = prefab.GetComponentInChildren<Text>();
        keywordText.text = csvReader.csvData[index][2]; // Keyword 열의 데이터

        ButtonHandler[] buttonHandlers = prefab.GetComponentsInChildren<ButtonHandler>();

        foreach (var handler in buttonHandlers)
        {
            handler.Initialize(scoreManager);
            handler.questionIndex = index;
            // handler.score = ... // 점수를 설정하는 로직 추가
        }
    }


    public void NextQuestions()
    {
        if (csvReader.csvData.Count >= currentQuestionIndex + 4)
        {
            currentQuestionIndex += 4;
            currentKeywordIndex += 4; 
            RenderQuestions();
        }
    }

    public void PreviousQuestions()
    {
        if (currentQuestionIndex >= 4)
        {
            currentQuestionIndex -= 4;
            currentKeywordIndex -= 4;
            RenderQuestions();
        }
    }
}

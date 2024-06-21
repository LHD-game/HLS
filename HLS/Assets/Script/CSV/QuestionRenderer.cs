using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class QuestionRenderer : MonoBehaviour
{
    public GameObject Q1; // 질문 프리팹 1
    public GameObject Q2; // 질문 프리팹 2
    public GameObject Q3; // 질문 프리팹 3
    public GameObject Q4; // 질문 프리팹 4

    private CSVReader csvReader;
    private int currentQuestionIndex = 1;

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
            // 첫 번째 질문 프리팹 설정
            Text questionText1 = Q1.GetComponentInChildren<Text>();
            questionText1.text = csvReader.csvData[currentQuestionIndex][1]; // Question 열의 데이터

            // 두 번째 질문 프리팹 설정
            Text questionText2 = Q2.GetComponentInChildren<Text>();
            questionText2.text = csvReader.csvData[currentQuestionIndex + 1][1]; // Question 열의 데이터

            // 세 번째 질문 프리팹 설정
            Text questionText3 = Q3.GetComponentInChildren<Text>();
            questionText3.text = csvReader.csvData[currentQuestionIndex + 2][1]; // Question 열의 데이터

            // 네 번째 질문 프리팹 설정
            Text questionText4 = Q4.GetComponentInChildren<Text>();
            questionText4.text = csvReader.csvData[currentQuestionIndex + 3][1]; // Question 열의 데이터
        }
    }

    public void NextQuestions()
    {
        if (csvReader.csvData.Count >= currentQuestionIndex + 4)
        {
            currentQuestionIndex += 4;
            RenderQuestions();
        }
    }

    public void PreviousQuestions()
    {
        if (currentQuestionIndex >= 4)
        {
            currentQuestionIndex -= 4;
            RenderQuestions();
        }
    }
}

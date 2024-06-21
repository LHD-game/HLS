using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class QuestionRenderer : MonoBehaviour
{
    public GameObject Q1; // ���� ������ 1
    public GameObject Q2; // ���� ������ 2
    public GameObject Q3; // ���� ������ 3
    public GameObject Q4; // ���� ������ 4

    private CSVReader csvReader;
    private int currentQuestionIndex = 1;

    void Start()
    {
        csvReader = GetComponent<CSVReader>();
        StartCoroutine(WaitForCSVData());
    }

    IEnumerator WaitForCSVData()
    {
        // CSV �����Ͱ� �ε�� ������ ��ٸ��ϴ�.
        while (csvReader.csvData.Count == 0)
        {
            yield return null;
        }
        // CSV �����Ͱ� �ε�Ǹ� ù ȭ���� �������մϴ�.
        RenderQuestions();
    }

    public void RenderQuestions()
    {
        if (csvReader.csvData.Count >= currentQuestionIndex + 4)
        {
            // ù ��° ���� ������ ����
            Text questionText1 = Q1.GetComponentInChildren<Text>();
            questionText1.text = csvReader.csvData[currentQuestionIndex][1]; // Question ���� ������

            // �� ��° ���� ������ ����
            Text questionText2 = Q2.GetComponentInChildren<Text>();
            questionText2.text = csvReader.csvData[currentQuestionIndex + 1][1]; // Question ���� ������

            // �� ��° ���� ������ ����
            Text questionText3 = Q3.GetComponentInChildren<Text>();
            questionText3.text = csvReader.csvData[currentQuestionIndex + 2][1]; // Question ���� ������

            // �� ��° ���� ������ ����
            Text questionText4 = Q4.GetComponentInChildren<Text>();
            questionText4.text = csvReader.csvData[currentQuestionIndex + 3][1]; // Question ���� ������
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

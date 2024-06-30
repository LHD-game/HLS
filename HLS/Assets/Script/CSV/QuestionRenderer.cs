using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class QuestionRenderer : MonoBehaviour
{
    public GameObject Keyword; // Ű���� ������
    public GameObject Q1; // ���� ������ 1
    public GameObject Q2; // ���� ������ 2
    public GameObject Q3; // ���� ������ 3
    public GameObject Q4; // ���� ������ 4
    public ScoreManager scoreManager; // ScoreManager �ν��Ͻ�

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
            // Ű���� ������ ����
            SetupKeywordPrefab(Keyword, currentKeywordIndex);

            // ù ��° ���� ������ ����
            SetupQuestionPrefab(Q1, currentQuestionIndex);

            // �� ��° ���� ������ ����
            SetupQuestionPrefab(Q2, currentQuestionIndex + 1);

            // �� ��° ���� ������ ����
            SetupQuestionPrefab(Q3, currentQuestionIndex + 2);

            // �� ��° ���� ������ ����
            SetupQuestionPrefab(Q4, currentQuestionIndex + 3);

           

        }
    }

    private void SetupQuestionPrefab(GameObject prefab, int index)
    {
        Text questionText = prefab.GetComponentInChildren<Text>();
        questionText.text = csvReader.csvData[index][1]; // Question ���� ������


        ButtonHandler[] buttonHandlers = prefab.GetComponentsInChildren<ButtonHandler>();
        foreach (var handler in buttonHandlers)
        {
            handler.Initialize(scoreManager);
            handler.questionIndex = index;
            // handler.score = ... // ������ �����ϴ� ���� �߰�
        }
    }

    private void SetupKeywordPrefab(GameObject prefab, int index)
    {
        Text keywordText = prefab.GetComponentInChildren<Text>();
        keywordText.text = csvReader.csvData[index][2]; // Keyword ���� ������

        ButtonHandler[] buttonHandlers = prefab.GetComponentsInChildren<ButtonHandler>();

        foreach (var handler in buttonHandlers)
        {
            handler.Initialize(scoreManager);
            handler.questionIndex = index;
            // handler.score = ... // ������ �����ϴ� ���� �߰�
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

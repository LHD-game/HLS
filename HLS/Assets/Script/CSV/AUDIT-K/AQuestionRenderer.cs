using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AQuestionRenderer : MonoBehaviour
{
    public GameObject Keyword; // Ű���� ������
    public GameObject Q1; // ���� ������ 1
    public GameObject Q2; // ���� ������ 2
    public GameObject Q3; // ���� ������ 3
    public GameObject Q4; // ���� ������ 4
    public ScoreManager scoreManager; // ScoreManager �ν��Ͻ�

    private ACSVReader csvReader;
    private int currentQuestionIndex = 1;

    void Start()
    {
        csvReader = GetComponent<ACSVReader>();
        StartCoroutine(WaitForCSVData());
    }

    IEnumerator WaitForCSVData()
    {
        // CSV �����Ͱ� �ε�� ������ ��ٸ��ϴ�.
        while (csvReader.csvData.Count == 0)
        {
            yield return null;
        }

        // ù ȭ���� �������մϴ�.
        RenderQuestions();
    }

    public void RenderQuestions()
    {
        int remainingQuestions = csvReader.csvData.Count - currentQuestionIndex;
        int questionsToRender = Mathf.Min(4, remainingQuestions); // �ִ� 4���� ���� ������

        if (questionsToRender > 0)
        {
            // ù ��° ���� ������ ����
            if (currentQuestionIndex < csvReader.csvData.Count)
                SetupQuestionPrefab(Q1, currentQuestionIndex);

            // �� ��° ���� ������ ����
            if (currentQuestionIndex + 1 < csvReader.csvData.Count)
                SetupQuestionPrefab(Q2, currentQuestionIndex + 1);

            // �� ��° ���� ������ ����
            if (currentQuestionIndex + 2 < csvReader.csvData.Count)
                SetupQuestionPrefab(Q3, currentQuestionIndex + 2);

            // �� ��° ���� ������ ����
            if (currentQuestionIndex + 3 < csvReader.csvData.Count)
                SetupQuestionPrefab(Q4, currentQuestionIndex + 3);
        }
    }

    private void SetupQuestionPrefab(GameObject prefab, int index)
    {
        Text questionText = prefab.GetComponentInChildren<Text>();
        questionText.text = csvReader.csvData[index][1]; // CSV���� �� ��° ���� �����ͷ� ���� ����

        ButtonHandler[] buttonHandlers = prefab.GetComponentsInChildren<ButtonHandler>();
        foreach (var handler in buttonHandlers)
        {
            handler.Initialize(scoreManager);
            handler.questionIndex = index;
        }
    }

    public void ResetQuestions()
    {
        currentQuestionIndex = 1; // ���� �ε��� �ʱ�ȭ
        RenderQuestions(); // ù ��° ���� ������
    }

    public void NextQuestions()
    {
        if (csvReader.csvData.Count >= currentQuestionIndex + 4)
        {
            currentQuestionIndex += 4; // ���� 4�� �������� �̵�
            RenderQuestions();
        }
    }

    public void PreviousQuestions()
    {
        if (currentQuestionIndex > 4)
        {
            currentQuestionIndex -= 4; // ���� 4�� �������� �̵�
            RenderQuestions();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HlsQuestionRenderer : QuestionRendererParent
{
    private HlsScoreManager scoreManager; // ���� �Ŵ���
    private HlsCsvReader csvReader;

    private int currentQuestionIndex = 0; // HLS �˻�� 0��° �������� ����
    private int lastHlsQuestionIndex = 36; // HLS ������ ���� �ε���

    private void Awake()
    {
        csvReader = GetComponent<HlsCsvReader>();
        scoreManager = GetComponent<HlsScoreManager>();
    }

    public void StartQuestion()
    {
        nextButton.onClick.AddListener(NextQuestion);
        previousButton.onClick.AddListener(PreviousQuestion);

        nextButton.interactable = false;
        RenderQuestions();
    }

    public void RenderQuestions()
    {
        // ���� �ؽ�Ʈ ����
        string question = csvReader.csvData[currentQuestionIndex][0];
        questionText.text = question; // ù ��° ���� ����

        // ���� ��ư���� ����
        ClearButtons();

        // ������ ���� ���� (CSV ���Ͽ��� 4���� ������, �� �������� ������ �ش�)
        for (int i = 0; i < 4; i++)
        {
            string choiceText = csvReader.csvData[currentQuestionIndex][i + 1]; // CSV �����Ϳ��� ������ ��������
            if (!string.IsNullOrEmpty(choiceText))
            {
                CreateButton(choiceText, i + 1); // �������� �´� ��ư ����
            }
        }

        // ���� ���� ���¿� ���� ��ư ���� �� nextButton Ȱ��ȭ
        if (userSelections.ContainsKey(currentQuestionIndex))
        {
            int selectedButtonIndex = userSelections[currentQuestionIndex];
            HighlightSelectedButton(selectedButtonIndex);
            nextButton.interactable = true; // �̹� ���õ� ��ư�� ������ ���� ��ư Ȱ��ȭ
        }
        else
        {
            nextButton.interactable = false; // ó������ ������ ���� ��ư ��Ȱ��ȭ
        }
    }

    public void CreateButton(string choiceText, int scoreIndex)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform);
        Text buttonText = newButton.GetComponentInChildren<Text>();
        buttonText.text = choiceText;

        Button button = newButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => OnButtonClick(scoreIndex));

        activeButtons.Add(newButton); // ������ ��ư�� ����Ʈ�� �߰�
        newButton.SetActive(true);
    }

    public void OnButtonClick(int scoreIndex)
    {
        userSelections[currentQuestionIndex] = scoreIndex; // ������ ��ư ����
        nextButton.interactable = true; // ���� ��ư Ȱ��ȭ
        HighlightSelectedButton(scoreIndex);
        scoreManager.AddScore(currentQuestionIndex, scoreIndex); // ���� �߰�
    }

    public void HighlightSelectedButton(int selectedIndex)
    {
        foreach (GameObject button in activeButtons)
        {
            Button answerButton = button.GetComponentInChildren<Button>();
            answerButton.image.color = Color.white; // �⺻ ���·� ����
        }

        if (selectedIndex >= 0 && selectedIndex < activeButtons.Count)
        {
            Button selectedButton = activeButtons[selectedIndex].GetComponentInChildren<Button>();
            selectedButton.image.color = Color.green; // ���õ� ��ư�� ���� ǥ��
        }
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < lastHlsQuestionIndex)
        {
            currentQuestionIndex++;

            // ���ο� ������ ������
            RenderQuestions();
        }
    }

    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            RenderQuestions();
        }
    }
}

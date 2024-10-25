using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 
public class FtnQuestionRenderer : QuestionRendererParent
{
    private FtnScoreManager scoreManager; // ���� �Ŵ���
    private FtnCsvReader csvReader; // CSV ����

    private int currentQuestionIndex = 3; // FTND ù ��° ���� �ε���
    private int lastFTNDQuestionIndex = 8; // FTND ������ ���� �ε���

    void Awake()
    {
        csvReader = GetComponent<FtnCsvReader>();
        scoreManager = GetComponent<FtnScoreManager>();
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
        questionText.text = csvReader.csvData[currentQuestionIndex][0]; // ���� ����
        ClearButtons();
        nextButton.interactable = false;

        // ������ ����
        for (int i = 1; i <= 4; i++)
        {
            string choiceText = csvReader.csvData[currentQuestionIndex][i]; // ������ �ؽ�Ʈ�� CSV���� ������
            if (!string.IsNullOrEmpty(choiceText))
            {
                CreateButton(choiceText, i); // �������� �´� ��ư ����
            }
        }
    }

    private void CreateButton(string choiceText, int score)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform);
        Button button = newButton.GetComponentInChildren<Button>();
        Text buttonText = newButton.GetComponentInChildren<Text>();
        buttonText.text = choiceText;
        newButton.SetActive(true);
        button.onClick.AddListener(() => OnButtonClick(score));

        activeButtons.Add(newButton);
    }

    public void OnButtonClick(int score)
    {
        nextButton.interactable = true;
        scoreManager.AddScore(currentQuestionIndex, score);

        // ���õ� ��ư�� Ȱ��ȭ
        for (int i = 0; i < activeButtons.Count; i++)
        {
            Image buttonImage = activeButtons[i].GetComponent<Image>();
            buttonImage.color = (i == score - 1) ? Color.green : Color.white;
        }
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < lastFTNDQuestionIndex)
        {
            currentQuestionIndex++;
            RenderQuestions();
        }
    }

    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 3)
        {
            currentQuestionIndex--;
            RenderQuestions();
        }
    }
}

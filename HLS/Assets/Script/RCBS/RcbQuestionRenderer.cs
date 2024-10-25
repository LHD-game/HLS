using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RcbQuestionRenderer : QuestionRendererParent
{
    private RcbScoreManager scoreManager;
    private RcbCsvReader csvReader;
    private int currentQuestionIndex = 1; // ù ��° ���� �ε���
    private int lastRcbQuestionIndex = 6; // RCBS ������ ���� �ε���
    private bool isAnswerSelected = false; // �亯 ���� ���� Ȯ�� ����

    void Awake()
    {
        csvReader = GetComponent<RcbCsvReader>();
        scoreManager = GetComponent<RcbScoreManager>();
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
        Debug.Log($"Rendering question {currentQuestionIndex}");

        questionText.text = csvReader.csvData[currentQuestionIndex][0]; // ���� ����

        // ������ Ȱ��ȭ�� ��ư���� ��� ����
        ClearButtons();
        // ����Ʈ �ʱ�ȭ
        isAnswerSelected = false; // ���ο� ���������� ���� ���õ��� ����
        nextButton.interactable = false; // "����" ��ư ��Ȱ��ȭ

        // ��ư�� �������� ����
        for (int i = 1; i <= 7; i++) // RCBS�� 7���� �������� �ִٰ� ����
        {
            string choiceText = csvReader.csvData[currentQuestionIndex][i];
            if (!string.IsNullOrEmpty(choiceText))
            {
                GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform);

                // Null üũ �� ���� Ȱ��ȭ
                if (newButton != null)
                {
                    newButton.SetActive(true);

                    Button button = newButton.GetComponentInChildren<Button>();
                    Text buttonText = newButton.GetComponentInChildren<Text>();
                    Image buttonImage = newButton.GetComponent<Image>(); // �̹��� ������Ʈ Ȯ��

                    if (buttonText != null)
                    {
                        buttonText.text = choiceText; // CSV���� ������ �ؽ�Ʈ
                        buttonText.enabled = true; // �ؽ�Ʈ ���� Ȱ��ȭ
                        Debug.Log($"Button Text {i}: {choiceText}");
                    }

                    int score = i - 1; // 0~6�� ó��

                    // ��ư Ŭ�� �� �̺�Ʈ ����
                    if (button != null)
                    {
                        button.onClick.AddListener(() => OnButtonClick(currentQuestionIndex, score));
                        button.interactable = true; // ��ư ���� Ȱ��ȭ

                        // ����ڰ� ������ ��ư�� ǥ��
                        if (userSelections.ContainsKey(currentQuestionIndex) && userSelections[currentQuestionIndex] == score)
                        {
                            // ����ڰ� ������ ������ �亯�� �ڵ����� Ȱ��ȭ
                            buttonImage.color = Color.green; // ������ ��ư�� �ð������� ǥ�� (���� ����)
                            isAnswerSelected = true; // �̹� ���õ� �亯�̹Ƿ� "����" ��ư Ȱ��ȭ
                            nextButton.interactable = true;
                        }
                    }

                    // RaycastTarget ���� Ȱ��ȭ (�̺�Ʈ �ޱ� ����)
                    if (buttonImage != null)
                    {
                        buttonImage.raycastTarget = true; // Raycast�� Ȱ��ȭ�Ͽ� Ŭ�� �����ϰ� ����
                    }

                    newButton.SetActive(true); // ��ư ���� Ȱ��ȭ
                    activeButtons.Add(newButton); // ����Ʈ�� �߰�
                }
            }
        }
    }

    public void OnButtonClick(int questionIndex, int answerIndex)
    {
        Debug.Log($"Question: {questionIndex}, Answer: {answerIndex}");
        scoreManager.AddScore(questionIndex, answerIndex); // ScoreManager�� AddScore ȣ��
        isAnswerSelected = true; // �亯�� ���õǾ����� ǥ��
        nextButton.interactable = true; // �亯�� ���������Ƿ� "����" ��ư Ȱ��ȭ

        // ����ڰ� ������ �亯 ���
        userSelections[questionIndex] = answerIndex;

        // ������ ��ư�� �ð��� ǥ�� ������Ʈ
        foreach (GameObject button in activeButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.white; // ���õ��� ���� ��ư�� ������� �ʱ�ȭ
            }
        }

        // ������ ��ư�� ���� ����
        Button clickedButton = activeButtons[answerIndex].GetComponent<Button>();
        if (clickedButton != null)
        {
            Image clickedImage = clickedButton.GetComponent<Image>();
            if (clickedImage != null)
            {
                clickedImage.color = Color.green; // ������ ��ư�� �ʷϻ����� ǥ��
            }
        }
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < lastRcbQuestionIndex) // RCBS ������ ���������� ó��
        {
            currentQuestionIndex++;
            RenderQuestions();
        }
        else
        {
            Debug.Log("�� �̻� ������ �����ϴ�."); // RCBS ���� ������ �� ó��
        }
    }

    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 1) // 1�� �������� ����
        {
            currentQuestionIndex--;
            RenderQuestions();
        }
    }
}

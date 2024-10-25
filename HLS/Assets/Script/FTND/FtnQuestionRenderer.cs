using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 
public class FtnQuestionRenderer : QuestionRendererParent
{
    private FtnScoreManager scoreManager; // 점수 매니저
    private FtnCsvReader csvReader; // CSV 리더

    private int currentQuestionIndex = 3; // FTND 첫 번째 질문 인덱스
    private int lastFTNDQuestionIndex = 8; // FTND 마지막 질문 인덱스

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
        questionText.text = csvReader.csvData[currentQuestionIndex][0]; // 질문 설정
        ClearButtons();
        nextButton.interactable = false;

        // 선택지 생성
        for (int i = 1; i <= 4; i++)
        {
            string choiceText = csvReader.csvData[currentQuestionIndex][i]; // 선택지 텍스트를 CSV에서 가져옴
            if (!string.IsNullOrEmpty(choiceText))
            {
                CreateButton(choiceText, i); // 선택지에 맞는 버튼 생성
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

        // 선택된 버튼만 활성화
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

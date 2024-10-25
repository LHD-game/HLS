using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class YfasQuestionRenderer : QuestionRendererParent
{
    private YFASScoreManager scoreManager; // 점수 매니저
    private YfasCsvReader csvReader;

    private int currentQuestionIndex = 0; // YFAS 첫 번째 질문 인덱스
    private int lastYfasQuestionIndex = 26; // YFAS 마지막 질문 인덱스

    private void Awake()
    {
        csvReader = GetComponent<YfasCsvReader>();
        scoreManager = GetComponent<YFASScoreManager>();
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
        // 질문 텍스트 설정
        string question = csvReader.csvData[currentQuestionIndex][0];
        questionText.text = question;

        // 기존 버튼들을 제거
        ClearButtons();

        // 선택지 동적 생성 (CSV 파일에서 선택지 생성)
        for (int i = 1; i < csvReader.csvData[currentQuestionIndex].Length; i++)
        {
            string choiceText = csvReader.csvData[currentQuestionIndex][i];
            CreateButton(choiceText, i - 1);
        }

        // 선택된 상태에 따른 버튼 강조 및 nextButton 활성화
        if (userSelections.ContainsKey(currentQuestionIndex))
        {
            int selectedButtonIndex = userSelections[currentQuestionIndex];
            HighlightSelectedButton(selectedButtonIndex);
            nextButton.interactable = true;
        }
        else
        {
            nextButton.interactable = false;
        }
    }

    public void CreateButton(string choiceText, int scoreIndex)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform);
        Text buttonText = newButton.GetComponentInChildren<Text>();
        buttonText.text = choiceText;

        Button button = newButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => OnButtonClick(scoreIndex));

        activeButtons.Add(newButton);
        newButton.SetActive(true);
    }

    public void OnButtonClick(int scoreIndex)
    {
        userSelections[currentQuestionIndex] = scoreIndex;
        nextButton.interactable = true;
        HighlightSelectedButton(scoreIndex);
        scoreManager.AddScore(currentQuestionIndex, scoreIndex); // 점수 추가
    }

    public void HighlightSelectedButton(int selectedIndex)
    {
        foreach (GameObject button in activeButtons)
        {
            Button answerButton = button.GetComponentInChildren<Button>();
            answerButton.image.color = Color.white;
        }

        if (selectedIndex >= 0 && selectedIndex < activeButtons.Count)
        {
            Button selectedButton = activeButtons[selectedIndex].GetComponentInChildren<Button>();
            selectedButton.image.color = Color.green;
        }
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < lastYfasQuestionIndex)
        {
            currentQuestionIndex++;
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

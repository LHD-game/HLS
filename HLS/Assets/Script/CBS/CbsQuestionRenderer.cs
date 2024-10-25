using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CbsQuestionRenderer : QuestionRendererParent
{
    private CbsScoreManager scoreManager; // 점수 매니저
    private CbsCsvReader csvReader;

    private int currentQuestionIndex = 0; // CBS 설문지 첫 번째 질문 인덱스
    private int lastCbsQuestionIndex = 7; // CBS 마지막 질문 인덱스

    private void Awake()
    {
        csvReader = GetComponent<CbsCsvReader>();
        scoreManager = GetComponent<CbsScoreManager>();
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
        questionText.text = question; // 첫 번째 열이 질문

        // 기존 버튼들을 제거
        ClearButtons();

        // 선택지 동적 생성 (CSV 파일에서 5개의 선택지라고 가정)
        for (int i = 0; i < 5; i++)
        {
            string choiceText = csvReader.csvData[currentQuestionIndex][i + 1]; // CSV 데이터에서 선택지 가져오기
            if (!string.IsNullOrEmpty(choiceText))
            {
                CreateButton(choiceText, i); // 선택지에 맞는 버튼 생성
            }
        }

        // 이전 선택 상태에 따른 버튼 강조 및 nextButton 활성화
        if (userSelections.ContainsKey(currentQuestionIndex))
        {
            int selectedButtonIndex = userSelections[currentQuestionIndex];
            HighlightSelectedButton(selectedButtonIndex);
            nextButton.interactable = true; // 이미 선택된 버튼이 있으면 다음 버튼 활성화
        }
        else
        {
            nextButton.interactable = false; // 처음에는 무조건 다음 버튼 비활성화
        }
    }

    public void CreateButton(string choiceText, int scoreIndex)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform);
        Text buttonText = newButton.GetComponentInChildren<Text>();
        buttonText.text = choiceText;

        Button button = newButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => OnButtonClick(scoreIndex));

        activeButtons.Add(newButton); // 생성된 버튼을 리스트에 추가
        newButton.SetActive(true);
    }

    public void OnButtonClick(int scoreIndex)
    {
        // 기존에 저장된 점수를 덮어씌움으로써 점수가 누적되지 않도록 처리
        userSelections[currentQuestionIndex] = scoreIndex; // 선택한 버튼 저장
        nextButton.interactable = true; // 다음 버튼 활성화
        HighlightSelectedButton(scoreIndex);
        scoreManager.AddScore(currentQuestionIndex, scoreIndex); // 점수 추가
    }

    public void HighlightSelectedButton(int selectedIndex)
    {
        foreach (GameObject button in activeButtons)
        {
            Button answerButton = button.GetComponentInChildren<Button>();
            answerButton.image.color = Color.white; // 기본 상태로 리셋
        }

        if (selectedIndex >= 0 && selectedIndex < activeButtons.Count)
        {
            Button selectedButton = activeButtons[selectedIndex].GetComponentInChildren<Button>();
            selectedButton.image.color = Color.green; // 선택된 버튼을 강조 표시
        }
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < lastCbsQuestionIndex)
        {
            currentQuestionIndex++;

            // 새로운 질문을 렌더링
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

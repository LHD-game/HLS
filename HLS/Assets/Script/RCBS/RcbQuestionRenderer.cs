using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RcbQuestionRenderer : QuestionRendererParent
{
    private RcbScoreManager scoreManager;
    private RcbCsvReader csvReader;
    private int currentQuestionIndex = 1; // 첫 번째 질문 인덱스
    private int lastRcbQuestionIndex = 6; // RCBS 마지막 질문 인덱스
    private bool isAnswerSelected = false; // 답변 선택 여부 확인 변수

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

        questionText.text = csvReader.csvData[currentQuestionIndex][0]; // 질문 설정

        // 기존에 활성화된 버튼들을 모두 제거
        ClearButtons();
        // 리스트 초기화
        isAnswerSelected = false; // 새로운 질문에서는 아직 선택되지 않음
        nextButton.interactable = false; // "다음" 버튼 비활성화

        // 버튼을 동적으로 생성
        for (int i = 1; i <= 7; i++) // RCBS는 7개의 선택지가 있다고 가정
        {
            string choiceText = csvReader.csvData[currentQuestionIndex][i];
            if (!string.IsNullOrEmpty(choiceText))
            {
                GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform);

                // Null 체크 후 강제 활성화
                if (newButton != null)
                {
                    newButton.SetActive(true);

                    Button button = newButton.GetComponentInChildren<Button>();
                    Text buttonText = newButton.GetComponentInChildren<Text>();
                    Image buttonImage = newButton.GetComponent<Image>(); // 이미지 컴포넌트 확인

                    if (buttonText != null)
                    {
                        buttonText.text = choiceText; // CSV에서 가져온 텍스트
                        buttonText.enabled = true; // 텍스트 강제 활성화
                        Debug.Log($"Button Text {i}: {choiceText}");
                    }

                    int score = i - 1; // 0~6점 처리

                    // 버튼 클릭 시 이벤트 연결
                    if (button != null)
                    {
                        button.onClick.AddListener(() => OnButtonClick(currentQuestionIndex, score));
                        button.interactable = true; // 버튼 강제 활성화

                        // 사용자가 선택한 버튼을 표시
                        if (userSelections.ContainsKey(currentQuestionIndex) && userSelections[currentQuestionIndex] == score)
                        {
                            // 사용자가 이전에 선택한 답변을 자동으로 활성화
                            buttonImage.color = Color.green; // 선택한 버튼을 시각적으로 표시 (색상 변경)
                            isAnswerSelected = true; // 이미 선택된 답변이므로 "다음" 버튼 활성화
                            nextButton.interactable = true;
                        }
                    }

                    // RaycastTarget 강제 활성화 (이벤트 받기 위함)
                    if (buttonImage != null)
                    {
                        buttonImage.raycastTarget = true; // Raycast를 활성화하여 클릭 가능하게 설정
                    }

                    newButton.SetActive(true); // 버튼 강제 활성화
                    activeButtons.Add(newButton); // 리스트에 추가
                }
            }
        }
    }

    public void OnButtonClick(int questionIndex, int answerIndex)
    {
        Debug.Log($"Question: {questionIndex}, Answer: {answerIndex}");
        scoreManager.AddScore(questionIndex, answerIndex); // ScoreManager의 AddScore 호출
        isAnswerSelected = true; // 답변이 선택되었음을 표시
        nextButton.interactable = true; // 답변을 선택했으므로 "다음" 버튼 활성화

        // 사용자가 선택한 답변 기록
        userSelections[questionIndex] = answerIndex;

        // 선택한 버튼의 시각적 표시 업데이트
        foreach (GameObject button in activeButtons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.white; // 선택되지 않은 버튼은 흰색으로 초기화
            }
        }

        // 선택한 버튼의 색상 변경
        Button clickedButton = activeButtons[answerIndex].GetComponent<Button>();
        if (clickedButton != null)
        {
            Image clickedImage = clickedButton.GetComponent<Image>();
            if (clickedImage != null)
            {
                clickedImage.color = Color.green; // 선택한 버튼을 초록색으로 표시
            }
        }
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < lastRcbQuestionIndex) // RCBS 마지막 질문까지만 처리
        {
            currentQuestionIndex++;
            RenderQuestions();
        }
        else
        {
            Debug.Log("더 이상 질문이 없습니다."); // RCBS 범위 끝났을 때 처리
        }
    }

    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 1) // 1번 질문부터 시작
        {
            currentQuestionIndex--;
            RenderQuestions();
        }
    }
}

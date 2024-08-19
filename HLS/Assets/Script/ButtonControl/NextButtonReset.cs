using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers; // ToggleButtonManager 리스트
    public ScoreManager scoreManager;
    private Button nextButton; // Next 버튼
    public GameObject resultPanel; // 결과 패널

    private Text nextButtonText; // Next 버튼의 텍스트 컴포넌트
    public Text restartButtonText;

    private int totalQuestions = 36; // 총 질문 개수
    private int questionsPerSet = 4; // 각 세트당 질문 수
    private int currentQuestionIndex = 0; // 현재 질문 인덱스

    void Start()
    {
        // 자식 오브젝트에서 Button 컴포넌트 찾기
        nextButton = GetComponentInChildren<Button>();
        if (nextButton == null)
        {
            Debug.LogError("Next button component is not found on the GameObject or its children.");
            return;
        }

        // 자식 오브젝트에서 Text 컴포넌트 찾기
        nextButtonText = nextButton.GetComponentInChildren<Text>();
        if (nextButtonText == null)
        {
            Debug.LogError("Text component is not found on the Button or its children.");
            return;
        }

        if (restartButtonText == null)
        {
            Debug.LogError("Restart button text is not assigned.");
            return;
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);
        UpdateNextButtonState();
        resultPanel.SetActive(false); // 결과 패널 비활성화
    }

    void Update()
    {
        UpdateNextButtonState();
    }

    public void UpdateNextButtonState()  //업데이트문이 아닌 다른방법 으로 할 수 있음 좋겠음.
    {
        if (nextButton == null)
        {
            Debug.LogError("Next button is not assigned.");
            return;
        }

        bool allQuestionsAnswered = true;
        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            if (toggleButtonManager == null || !toggleButtonManager.IsQuestionAnswered())
            {
                allQuestionsAnswered = false;
                break;
            }
        }

        nextButton.interactable = allQuestionsAnswered;

        if (nextButton.interactable)
        {
            //Debug.Log("Next button is now interactable.");
        }
        else
        {
            //Debug.Log("Next button is now NOT interactable.");
        }

        restartButtonText.text = "처음부터";

        if (currentQuestionIndex >= totalQuestions - questionsPerSet)
        {
            if (nextButtonText != null)
            {
                nextButtonText.text = "결과보기";
            }
        }
        else
        {
            if (nextButtonText != null)
            {
                nextButtonText.text = "다음으로";
            }
        }
    }



    public void OnNextButtonClicked()
    {
        if (nextButton != null && nextButton.interactable)
        {
            foreach (var toggleButtonManager in toggleButtonManagers)
            {
                if (toggleButtonManager != null)
                {
                    toggleButtonManager.ResetButtonStates(); // 버튼 상태 초기화
                }
            }

            // 다음 질문 세트로 이동하는 로직을 여기에 추가
            currentQuestionIndex += questionsPerSet;

            // 마지막 질문 세트이면 결과창을 띄움
            if (currentQuestionIndex >= totalQuestions)
            {
                resultPanel.SetActive(true); // 결과 패널 활성화
                Time.timeScale = 1f; // 게임 일시정지
            }
            else
            {
                UpdateNextButtonState(); // 다음 질문 세트로 이동 후 상태 업데이트
            }
        }
    }
}

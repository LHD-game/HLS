using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers;
    public ScoreManager scoreManager;
    public RaderDraw RD;

    private Button nextButton;
    private Text nextButtonText;
    public Text restartButtonText;
    private int totalQuestions = 36;
    private int questionsPerSet = 4;
    private int currentQuestionIndex = 0;

    void Start()
    {
        nextButton = GetComponentInChildren<Button>();
        if (nextButton == null)
        {
            Debug.LogError("Next 버튼 컴포넌트가 GameObject 또는 자식 오브젝트에서 찾을 수 없습니다.");
            return;
        }

        nextButtonText = nextButton.GetComponentInChildren<Text>();
        if (nextButtonText == null)
        {
            Debug.LogError("Text 컴포넌트가 Button 또는 자식 오브젝트에서 찾을 수 없습니다.");
            return;
        }

        if (restartButtonText == null)
        {
            Debug.LogError("Restart 버튼 텍스트가 할당되지 않았습니다.");
            return;
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);

        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            if (toggleButtonManager != null)
            {
                toggleButtonManager.OnToggleStateChanged += UpdateNextButtonState;  // Update문 대체 부분 (ToggleButtonManager : 토글의 상태가 변경될 때마다를 감지하여 UpdateNextButtonState()를 호출)
            }
        }

        UpdateNextButtonState();
    }

    public void UpdateNextButtonState()
    {
        if (nextButton == null)
        {
            Debug.LogError("Next 버튼이 할당되지 않았습니다.");
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
                restartButtonText.text = "처음부터";
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
                    toggleButtonManager.ResetButtonStates();
                    ResetToggleButtonVisuals(toggleButtonManager); // 버튼 시각적 상태 초기화
                }
            }

            currentQuestionIndex += questionsPerSet;

            if (currentQuestionIndex >= totalQuestions)
            {
                RD.addData();
                Time.timeScale = 1f;
            }
            else
            {
                UpdateNextButtonState();
            }
        }
    }

    private void ResetToggleButtonVisuals(ToggleButtonManager toggleButtonManager)
    {
        foreach (var toggle in toggleButtonManager.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false; // 토글 상태 초기화
            toggle.graphic.CrossFadeColor(Color.white, 0f, true, true); // 그래픽 색상 초기화
        }
    }
}

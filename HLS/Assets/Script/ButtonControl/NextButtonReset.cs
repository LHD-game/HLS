using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers;
    public ScoreManager scoreManager;
    public RaderDraw RD;

    private Button nextButton;
    public Text nextButtonText;  // Text 컴포넌트를 Inspector에서 직접 연결
    //public Text restartButtonText;
    private int totalQuestions = 36;
    private int questionsPerSet = 4;
    private int currentQuestionIndex = 0;

    //삭제 스크립트?
    void Start()
    {
        nextButton = GetComponentInChildren<Button>();  // 이 부분은 기존대로 둡니다.
        if (nextButton == null)
        {
            Debug.LogError("Next 버튼 컴포넌트를 찾을 수 없습니다."); //컴포넌트는 find로 찾는게 아닌 직접 할당 식으로 넣습니다.
            return;
        }

        // Text 컴포넌트 할당 확인
        if (nextButtonText == null)
        {
            Debug.LogError("Next 버튼의 Text 컴포넌트가 할당되지 않았습니다.");
            return;
        }

        /*if (restartButtonText == null)
        {
            Debug.LogError("Restart 버튼 텍스트가 할당되지 않았습니다.");
            return;
        }*/

        nextButton.onClick.AddListener(OnNextButtonClicked);

        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            if (toggleButtonManager != null)
            {
                toggleButtonManager.OnToggleStateChanged += UpdateNextButtonState;
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


        //restartButtonText.text = "처음부터";

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
                //restartButtonText.text = "처음부터";
            }
        }

        nextButton.interactable = allQuestionsAnswered; //text설정 후 활성화
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
                    ResetToggleButtonVisuals(toggleButtonManager);  // 버튼 시각적 상태 초기화
                }
            }

            currentQuestionIndex += questionsPerSet;

            if (currentQuestionIndex >= totalQuestions)
            {
                //WinCtl.Instance.GotoDatailWin();
                //RD.addData();
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
            toggle.isOn = false;  // 토글 상태 초기화
            toggle.graphic.CrossFadeColor(Color.white, 0f, true, true);  // 그래픽 색상 초기화
        }
    }
}

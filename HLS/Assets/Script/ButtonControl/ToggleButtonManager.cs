using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<ToggleButton> toggleButtons; // 모든 버튼 리스트
    private List<bool> buttonStates; // 버튼 상태 리스트
    private bool isQuestionAnswered = false;

    void Start()
    {
        buttonStates = new List<bool>(new bool[toggleButtons.Count]);

        foreach (var toggleButton in toggleButtons)
        {
            toggleButton.button.onClick.AddListener(() => OnButtonClick(toggleButton));
        }
    }

    void OnButtonClick(ToggleButton clickedButton)
    {
        if (clickedButton == null)
        {
            Debug.LogError("Clicked button is null.");
            return;
        }

        // 클릭된 버튼의 상태를 토글
        clickedButton.ToggleButtonState();

        int index = toggleButtons.IndexOf(clickedButton);
        if (index >= 0)
        {
            buttonStates[index] = clickedButton.IsSelected();
        }
        else
        {
            Debug.LogError("Clicked button is not found in the toggleButtons list.");
        }

        // 하나의 버튼만 선택되도록 설정
        foreach (var toggleButton in toggleButtons)
        {
            if (toggleButton != clickedButton)
            {
                toggleButton.ResetButtonState();
            }
        }

        // 질문이 답변되었는지 확인
        isQuestionAnswered = clickedButton.IsSelected();

        // `NextButtonReset`의 상태를 업데이트하여 NextButton 활성화 여부 확인
        var nextButtonReset = FindObjectOfType<NextButtonReset>();
        if (nextButtonReset != null)
        {
            nextButtonReset.UpdateNextButtonState();
        }
        else
        {
            Debug.LogError("NextButtonReset component is not found in the scene.");
        }
    }


    public void ResetButtonStates()
    {
        for (int i = 0; i < toggleButtons.Count; i++)
        {
            toggleButtons[i].ResetButtonState();
            buttonStates[i] = false;
        }
        isQuestionAnswered = false;
    }

    public bool IsQuestionAnswered()
    {
        return isQuestionAnswered;
    }
}

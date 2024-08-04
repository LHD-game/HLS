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
        // 클릭된 버튼의 상태를 토글
        clickedButton.ToggleButtonState();

        // 클릭된 버튼의 상태를 업데이트
        int index = toggleButtons.IndexOf(clickedButton);
        if (index >= 0)
        {
            buttonStates[index] = clickedButton.IsSelected();
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
        isQuestionAnswered = true;
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

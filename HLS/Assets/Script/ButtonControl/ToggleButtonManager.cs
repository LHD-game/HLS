using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<ToggleButton> toggleButtons; // 모든 버튼 리스트
    private List<bool> buttonStates; // 버튼 상태 리스트
    private bool isQuestionAnswered = false;

    // 이벤트 선언
    public delegate void ToggleStateChangedHandler();
    public event ToggleStateChangedHandler OnToggleStateChanged;  // 상태 변경 이벤트

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

        int index = toggleButtons.IndexOf(clickedButton);
        if (index >= 0)
        {
            buttonStates[index] = clickedButton.IsSelected();
        }

        // 질문이 답변되었는지 확인
        isQuestionAnswered = clickedButton.IsSelected();

        // 상태가 변경되었음을 알림
        OnToggleStateChanged?.Invoke();  // 상태 변경 시 이벤트 호출
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<ToggleButton> toggleButtons; // 모든 버튼 리스트

    void Start()
    {
        foreach (var toggleButton in toggleButtons)
        {
            toggleButton.button.onClick.AddListener(() => OnButtonClick(toggleButton));
        }
    }

    void OnButtonClick(ToggleButton clickedButton)
    {
        // 모든 버튼 상태를 초기화
        foreach (var toggleButton in toggleButtons)
        {
            if (toggleButton != clickedButton)
            {
                toggleButton.ResetButtonState();
            }
        }

        // 클릭된 버튼의 상태를 토글
        clickedButton.ToggleButtonState();
    }

    public void ResetButtonStates()
    {
        foreach (var toggleButton in toggleButtons)
        {
            toggleButton.ResetButtonState();
        }
    }
}

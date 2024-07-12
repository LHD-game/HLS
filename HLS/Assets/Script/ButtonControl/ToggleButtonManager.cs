using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<Button> buttons; // 모든 버튼 리스트
    private Button lastClickedButton; // 마지막으로 클릭된 버튼

    void Start()
    {
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
    }

    void OnButtonClicked(Button clickedButton)
    {
        // 마지막으로 클릭된 버튼이 있으면 색상 초기화
        if (lastClickedButton != null)
        {
            var colors = lastClickedButton.colors;
            colors.normalColor = Color.white;
            lastClickedButton.colors = colors;
        }

        // 클릭된 버튼 색상 변경
        var clickedColors = clickedButton.colors;
        clickedColors.normalColor = Color.green;
        clickedButton.colors = clickedColors;

        // 마지막으로 클릭된 버튼 업데이트
        lastClickedButton = clickedButton;
    }

    public void ResetButtonStates()
    {
        foreach (var button in buttons)
        {
            var colors = button.colors;
            colors.normalColor = Color.white;
            button.colors = colors;
        }
        lastClickedButton = null; // 마지막으로 클릭된 버튼 초기화
    }
}

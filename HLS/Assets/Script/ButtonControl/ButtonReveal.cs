using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonColor : MonoBehaviour
{
    private Button button;
    private Color normalColor; // 원래 색깔
    private Color pressedColor = Color.red; // 눌렸을 때 색깔

    void Start()
    {
        button = GetComponent<Button>();
        normalColor = button.colors.normalColor; // 원래 색상 저장
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // 다른 버튼들의 색상을 원래 색으로 되돌리기
        ResetOtherButtonsColor();

        // 현재 버튼의 색상을 눌린 상태로 변경
        ColorBlock colors = button.colors;
        colors.normalColor = pressedColor;
        button.colors = colors;
    }

    void ResetOtherButtonsColor()
    {
        // 현재 버튼을 제외한 모든 버튼들의 색상을 원래 색으로 되돌림
        ToggleButtonColor[] allButtons = FindObjectsOfType<ToggleButtonColor>();
        foreach (ToggleButtonColor otherButton in allButtons)
        {
            if (otherButton != this)
            {
                ColorBlock colors = otherButton.button.colors;
                colors.normalColor = otherButton.normalColor;
                otherButton.button.colors = colors;
            }
        }
    }
}

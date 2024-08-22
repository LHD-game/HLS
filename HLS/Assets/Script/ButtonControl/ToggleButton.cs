using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Button button;
    private bool isSelected;

    void Start()
    {
        isSelected = false;
        ResetButtonState(); // 초기화 시 버튼 상태 설정
    }

    public void ToggleButtonState()
    {
        isSelected = !isSelected;
        UpdateButtonColor();
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void ResetButtonState()
    {
        isSelected = false;
        UpdateButtonColor();
    }

    public void UpdateButtonColor()
    {
        ColorBlock colors = button.colors;
        if (isSelected)
        {
            colors.normalColor = Color.green; // 선택된 상태로 변경
        }
        else
        {
            colors.normalColor = Color.white; // 초기화 상태로 변경
        }
        button.colors = colors;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    public Button[] buttons; // 모든 버튼을 할당할 배열
    public Color selectedColor = Color.red; // 선택된 버튼 색상
    public Color defaultColor = Color.white; // 기본 버튼 색상

    private Button currentSelectedButton;

    void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }

        // 저장된 선택된 버튼을 불러옵니다
        int selectedButtonIndex = PlayerPrefs.GetInt("SelectedButtonIndex", -1);
        if (selectedButtonIndex != -1 && selectedButtonIndex < buttons.Length)
        {
            OnButtonClick(buttons[selectedButtonIndex]);
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        if (currentSelectedButton != null)
        {
            // 이전에 선택된 버튼의 색상을 기본 색상으로 변경
            ColorBlock colors = currentSelectedButton.colors;
            colors.normalColor = defaultColor;
            currentSelectedButton.colors = colors;
        }

        // 클릭된 버튼을 현재 선택된 버튼으로 설정
        currentSelectedButton = clickedButton;

        // 클릭된 버튼의 색상을 선택된 색상으로 변경
        ColorBlock clickedColors = clickedButton.colors;
        clickedColors.normalColor = selectedColor;
        clickedButton.colors = clickedColors;

        // 선택된 버튼의 인덱스를 저장
        int selectedIndex = System.Array.IndexOf(buttons, clickedButton);
        PlayerPrefs.SetInt("SelectedButtonIndex", selectedIndex);
        PlayerPrefs.Save();
    }
}

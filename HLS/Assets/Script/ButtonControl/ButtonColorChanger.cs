using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    public Button[] buttons; // ��� ��ư�� �Ҵ��� �迭
    public Color selectedColor = Color.red; // ���õ� ��ư ����
    public Color defaultColor = Color.white; // �⺻ ��ư ����

    private Button currentSelectedButton;

    void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }

        // ����� ���õ� ��ư�� �ҷ��ɴϴ�
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
            // ������ ���õ� ��ư�� ������ �⺻ �������� ����
            ColorBlock colors = currentSelectedButton.colors;
            colors.normalColor = defaultColor;
            currentSelectedButton.colors = colors;
        }

        // Ŭ���� ��ư�� ���� ���õ� ��ư���� ����
        currentSelectedButton = clickedButton;

        // Ŭ���� ��ư�� ������ ���õ� �������� ����
        ColorBlock clickedColors = clickedButton.colors;
        clickedColors.normalColor = selectedColor;
        clickedButton.colors = clickedColors;

        // ���õ� ��ư�� �ε����� ����
        int selectedIndex = System.Array.IndexOf(buttons, clickedButton);
        PlayerPrefs.SetInt("SelectedButtonIndex", selectedIndex);
        PlayerPrefs.Save();
    }
}

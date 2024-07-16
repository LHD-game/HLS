using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<Button> buttons; // ��� ��ư ����Ʈ
    private Button lastClickedButton; // ���������� Ŭ���� ��ư

    void Start()
    {
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
    }

    void OnButtonClicked(Button clickedButton)
    {
        // ���������� Ŭ���� ��ư�� ������ ���� �ʱ�ȭ
        if (lastClickedButton != null)
        {
            var colors = lastClickedButton.colors;
            colors.normalColor = Color.white;
            lastClickedButton.colors = colors;
        }

        // Ŭ���� ��ư ���� ����
        var clickedColors = clickedButton.colors;
        clickedColors.normalColor = Color.green;
        clickedButton.colors = clickedColors;

        // ���������� Ŭ���� ��ư ������Ʈ
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
        lastClickedButton = null; // ���������� Ŭ���� ��ư �ʱ�ȭ
    }
}

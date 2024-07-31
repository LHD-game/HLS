using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<ToggleButton> toggleButtons; // ��� ��ư ����Ʈ
    private List<bool> buttonStates; // ��ư ���� ����Ʈ

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
        // Ŭ���� ��ư�� ���¸� ���
        clickedButton.ToggleButtonState();

        // Ŭ���� ��ư�� ���¸� ������Ʈ
        int index = toggleButtons.IndexOf(clickedButton);
        if (index >= 0)
        {
            buttonStates[index] = clickedButton.IsSelected();
        }
    }

    public void ResetButtonStates()
    {
        for (int i = 0; i < toggleButtons.Count; i++)
        {
            toggleButtons[i].ResetButtonState();
            buttonStates[i] = false;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<ToggleButton> toggleButtons; // ��� ��ư ����Ʈ

    void Start()
    {
        foreach (var toggleButton in toggleButtons)
        {
            toggleButton.button.onClick.AddListener(() => OnButtonClick(toggleButton));
        }
    }

    void OnButtonClick(ToggleButton clickedButton)
    {
        // ��� ��ư ���¸� �ʱ�ȭ
        foreach (var toggleButton in toggleButtons)
        {
            if (toggleButton != clickedButton)
            {
                toggleButton.ResetButtonState();
            }
        }

        // Ŭ���� ��ư�� ���¸� ���
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

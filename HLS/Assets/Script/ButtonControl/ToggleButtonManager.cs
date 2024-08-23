using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<ToggleButton> toggleButtons; // ��� ��ư ����Ʈ
    private List<bool> buttonStates; // ��ư ���� ����Ʈ
    private bool isQuestionAnswered = false;

    // �̺�Ʈ ����
    public delegate void ToggleStateChangedHandler();
    public event ToggleStateChangedHandler OnToggleStateChanged;  // ���� ���� �̺�Ʈ

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

        int index = toggleButtons.IndexOf(clickedButton);
        if (index >= 0)
        {
            buttonStates[index] = clickedButton.IsSelected();
        }

        // ������ �亯�Ǿ����� Ȯ��
        isQuestionAnswered = clickedButton.IsSelected();

        // ���°� ����Ǿ����� �˸�
        OnToggleStateChanged?.Invoke();  // ���� ���� �� �̺�Ʈ ȣ��
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

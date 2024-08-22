using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonManager : MonoBehaviour
{
    public List<ToggleButton> toggleButtons; // ��� ��ư ����Ʈ
    private List<bool> buttonStates; // ��ư ���� ����Ʈ
    private bool isQuestionAnswered = false;

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
        if (clickedButton == null)
        {
            Debug.LogError("Clicked button is null.");
            return;
        }

        // Ŭ���� ��ư�� ���¸� ���
        clickedButton.ToggleButtonState();

        int index = toggleButtons.IndexOf(clickedButton);
        if (index >= 0)
        {
            buttonStates[index] = clickedButton.IsSelected();
        }
        else
        {
            Debug.LogError("Clicked button is not found in the toggleButtons list.");
        }

        // �ϳ��� ��ư�� ���õǵ��� ����
        foreach (var toggleButton in toggleButtons)
        {
            if (toggleButton != clickedButton)
            {
                toggleButton.ResetButtonState();
            }
        }

        // ������ �亯�Ǿ����� Ȯ��
        isQuestionAnswered = clickedButton.IsSelected();

        // `NextButtonReset`�� ���¸� ������Ʈ�Ͽ� NextButton Ȱ��ȭ ���� Ȯ��
        var nextButtonReset = FindObjectOfType<NextButtonReset>();
        if (nextButtonReset != null)
        {
            nextButtonReset.UpdateNextButtonState();
        }
        else
        {
            Debug.LogError("NextButtonReset component is not found in the scene.");
        }
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private Dictionary<int, List<Button>> questionButtons = new Dictionary<int, List<Button>>();
    private Dictionary<int, Button> currentPressedButtons = new Dictionary<int, Button>();

    void Start()
    {
        // ���� ��Ʈ ���� ��ư�� ã�� ���
        for (int i = 1; i <= 4; i++)
        {
            RegisterButtonsForQuestion(i);
        }
    }

    private void RegisterButtonsForQuestion(int questionNumber)
    {
        List<Button> buttons = new List<Button>();
        string questionTag = "Question" + questionNumber; // �� ������ ��ư�� "Question1", "Question2" �� �±׷� ����

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(questionTag))
        {
            Button btn = obj.GetComponent<Button>();
            if (btn != null)
            {
                buttons.Add(btn);
                btn.onClick.AddListener(() => OnButtonPressed(questionNumber, btn));
            }
        }

        questionButtons[questionNumber] = buttons;
    }

    public void OnButtonPressed(int questionNumber, Button pressedButton)
    {
        if (currentPressedButtons.ContainsKey(questionNumber) && currentPressedButtons[questionNumber] != null)
        {
            ResetButtonState(currentPressedButtons[questionNumber]);
        }

        SetButtonPressedState(pressedButton);
        currentPressedButtons[questionNumber] = pressedButton;
    }

    private void SetButtonPressedState(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.green; // ���� ������ ���� ����
        button.colors = colors;
    }

    private void ResetButtonState(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // �⺻ ������ ���� ����
        button.colors = colors;
    }
}

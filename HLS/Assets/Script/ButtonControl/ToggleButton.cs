using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private Dictionary<int, List<Button>> questionButtons = new Dictionary<int, List<Button>>();
    private Dictionary<int, Button> currentPressedButtons = new Dictionary<int, Button>();

    void Start()
    {
        // 질문 세트 별로 버튼을 찾고 등록
        for (int i = 1; i <= 4; i++)
        {
            RegisterButtonsForQuestion(i);
        }
    }

    private void RegisterButtonsForQuestion(int questionNumber)
    {
        List<Button> buttons = new List<Button>();
        string questionTag = "Question" + questionNumber; // 각 질문의 버튼은 "Question1", "Question2" 등 태그로 구분

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
        colors.normalColor = Color.green; // 눌림 상태일 때의 색상
        button.colors = colors;
    }

    private void ResetButtonState(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // 기본 상태일 때의 색상
        button.colors = colors;
    }
}

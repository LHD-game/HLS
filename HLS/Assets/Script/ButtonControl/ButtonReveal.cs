using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonColor : MonoBehaviour
{
    private Button button;
    private Color normalColor; // ���� ����
    private Color pressedColor = Color.red; // ������ �� ����

    void Start()
    {
        button = GetComponent<Button>();
        normalColor = button.colors.normalColor; // ���� ���� ����
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // �ٸ� ��ư���� ������ ���� ������ �ǵ�����
        ResetOtherButtonsColor();

        // ���� ��ư�� ������ ���� ���·� ����
        ColorBlock colors = button.colors;
        colors.normalColor = pressedColor;
        button.colors = colors;
    }

    void ResetOtherButtonsColor()
    {
        // ���� ��ư�� ������ ��� ��ư���� ������ ���� ������ �ǵ���
        ToggleButtonColor[] allButtons = FindObjectsOfType<ToggleButtonColor>();
        foreach (ToggleButtonColor otherButton in allButtons)
        {
            if (otherButton != this)
            {
                ColorBlock colors = otherButton.button.colors;
                colors.normalColor = otherButton.normalColor;
                otherButton.button.colors = colors;
            }
        }
    }
}

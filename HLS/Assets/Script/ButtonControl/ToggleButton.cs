using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Button button;
    private bool isSelected;

    void Start()
    {
        isSelected = false;
        ResetButtonState(); // �ʱ�ȭ �� ��ư ���� ����
    }

    public void ToggleButtonState()
    {
        isSelected = !isSelected;
        UpdateButtonColor();
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void ResetButtonState()
    {
        isSelected = false;
        UpdateButtonColor();
    }

    public void UpdateButtonColor()
    {
        ColorBlock colors = button.colors;
        if (isSelected)
        {
            colors.normalColor = Color.green; // ���õ� ���·� ����
        }
        else
        {
            colors.normalColor = Color.white; // �ʱ�ȭ ���·� ����
        }
        button.colors = colors;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Button button;
    private ColorBlock defaultColorBlock;
    private ColorBlock selectedColorBlock;
    private bool isSelected = false;

    void Start()
    {
        defaultColorBlock = button.colors;
        selectedColorBlock = button.colors;
        selectedColorBlock.normalColor = Color.yellow ;
    }

    public void ToggleButtonState()
    {
        isSelected = !isSelected;
        button.colors = isSelected ? selectedColorBlock : defaultColorBlock;
    }

    public void SetButtonSelected(bool selected)
    {
        isSelected = selected;
        button.colors = selected ? selectedColorBlock : defaultColorBlock;
    }

    public void ResetButtonState()
    {
        isSelected = false;
        button.colors = defaultColorBlock;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}

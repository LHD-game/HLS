using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Button button;
    private ColorBlock defaultColorBlock;
    private ColorBlock selectedColorBlock;
    private bool isSelected = false;
    public ButtonManager B;

    void Start()
    {
        //button = GetComponent<Button>();
        defaultColorBlock = button.colors ;
        selectedColorBlock = button.colors;
        selectedColorBlock.normalColor = Color.green;

        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        ToggleButtonState();
        Debug.Log(this);
        //B.UpdateButtonStates(this);
    }

    public void ToggleButtonState()
    {
        isSelected = !isSelected;
        button.colors = isSelected ? selectedColorBlock : defaultColorBlock;
    }
/*
    public void ResetButtonState()
    {
        isSelected = false;
        button.colors = defaultColorBlock;
    }*/
}

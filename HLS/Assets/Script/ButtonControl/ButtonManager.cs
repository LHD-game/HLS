using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;
    private List<ButtonToggle> buttonToggles;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        buttonToggles = new List<ButtonToggle>(FindObjectsOfType<ButtonToggle>());
    }

    public void UpdateButtonStates(ButtonToggle selectedButtonToggle)
    {
        foreach (var buttonToggle in buttonToggles)
        {
            if (buttonToggle != selectedButtonToggle)
            {
                buttonToggle.ResetButtonState();
            }
        }
    }

    public void ResetAllButtons()
    {
        foreach (var buttonToggle in buttonToggles)
        {
            buttonToggle.ResetButtonState();
        }
    }
}

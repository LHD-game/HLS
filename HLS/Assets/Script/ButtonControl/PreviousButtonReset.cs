using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviousButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers;

    public void OnClickPreviousButton()
    {
        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            if (toggleButtonManager != null)
            {
                toggleButtonManager.ResetButtonStates();
                ResetToggleButtonVisuals(toggleButtonManager);  // ��ư �ð��� ���� �ʱ�ȭ
            }
        }
    }

    private void ResetToggleButtonVisuals(ToggleButtonManager toggleButtonManager)
    {
        foreach (var toggle in toggleButtonManager.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false;  // ��� ���� �ʱ�ȭ
            toggle.graphic.CrossFadeColor(Color.white, 0f, true, true);  // �׷��� ���� �ʱ�ȭ
        }
    }
}

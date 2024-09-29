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
                ResetToggleButtonVisuals(toggleButtonManager);  // 버튼 시각적 상태 초기화
            }
        }
    }

    private void ResetToggleButtonVisuals(ToggleButtonManager toggleButtonManager)
    {
        foreach (var toggle in toggleButtonManager.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false;  // 토글 상태 초기화
            toggle.graphic.CrossFadeColor(Color.white, 0f, true, true);  // 그래픽 색상 초기화
        }
    }
}

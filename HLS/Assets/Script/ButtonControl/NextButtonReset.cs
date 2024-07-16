using UnityEngine;
using System.Collections.Generic;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers; // ToggleButtonManager 리스트

    public void OnNextButtonClicked()
    {
        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            toggleButtonManager.ResetButtonStates(); // 버튼 상태 초기화
        }
    }
}

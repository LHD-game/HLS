using UnityEngine;
using System.Collections.Generic;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers; // ToggleButtonManager ����Ʈ

    public void OnNextButtonClicked()
    {
        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            toggleButtonManager.ResetButtonStates(); // ��ư ���� �ʱ�ȭ
        }
    }
}

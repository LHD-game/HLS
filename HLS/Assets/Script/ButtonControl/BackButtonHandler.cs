using UnityEngine;
using UnityEngine.UI;

public class BackButtonHandler : MonoBehaviour
{
    public Button backButton;
    public GameObject restartButtonObj;  // Restart 버튼의 오브젝트 자체
    public Text restartButtonText;  // Restart 버튼의 텍스트

    private bool isRestartButtonVisible = false;

    void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        // 초기 상태에서 Restart 버튼을 비활성화
        if (restartButtonObj != null)
        {
            restartButtonObj.SetActive(false);
        }
    }

    private void OnBackButtonClicked()
    {
        if (restartButtonObj != null)
        {
            isRestartButtonVisible = !isRestartButtonVisible;
            restartButtonObj.SetActive(isRestartButtonVisible);
        }
    }
}

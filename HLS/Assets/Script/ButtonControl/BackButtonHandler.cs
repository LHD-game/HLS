using UnityEngine;
using UnityEngine.UI;

public class BackButtonHandler : MonoBehaviour
{
    public Button backButton;
    public GameObject restartButtonObj;  // Restart ��ư�� ������Ʈ ��ü
    public Text restartButtonText;  // Restart ��ư�� �ؽ�Ʈ

    private bool isRestartButtonVisible = false;

    void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        // �ʱ� ���¿��� Restart ��ư�� ��Ȱ��ȭ
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

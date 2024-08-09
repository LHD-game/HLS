using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public Button restartButton; // Restart ��ư
    public ScoreManager scoreManager; // ScoreManager ��ũ��Ʈ
    public QuestionRenderer questionRenderer; // QuestionRenderer ��ũ��Ʈ
    public Text restartButtonText;

    void Start()
    {
        restartButtonText = restartButton.GetComponentInChildren<Text>();
        restartButtonText.text = "�ٽ�";
        // Restart ��ư�� Ŭ�� �̺�Ʈ �߰�
        restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    public void OnRestartButtonClick()
    {   

        // ���� �ʱ�ȭ �� ���� ������ �ʱ�ȭ
        scoreManager.Restart(questionRenderer);
        // toggleButtonManager.ResetButtonStates(); // ��ư ���� �ʱ�ȭ

    }
}

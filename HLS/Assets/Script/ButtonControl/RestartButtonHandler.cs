using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public Button restartButton; // Restart ��ư
    public ScoreManager scoreManager; // ScoreManager ��ũ��Ʈ
    public QuestionRenderer questionRenderer; // QuestionRenderer ��ũ��Ʈ

    void Start()
    {
        // Restart ��ư�� Ŭ�� �̺�Ʈ �߰�
        restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    void OnRestartButtonClick()
    {
        // ���� �ʱ�ȭ �� ���� ������ �ʱ�ȭ
        scoreManager.Restart(questionRenderer);
    }
}

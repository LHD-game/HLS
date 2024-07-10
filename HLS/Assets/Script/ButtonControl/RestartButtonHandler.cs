using UnityEngine;
using UnityEngine.UI;

public class RestartButtonHandler : MonoBehaviour
{
    public ScoreManager scoreManager; // ScoreManager �ν��Ͻ�
    public QuestionRenderer questionRenderer; // QuestionRenderer �ν��Ͻ�

    void Start()
    {
        // Restart ��ư�� Ŭ�� �̺�Ʈ�� �߰�
        GetComponent<Button>().onClick.AddListener(OnRestartButtonClick);
    }

    void OnRestartButtonClick()
    {
        scoreManager.Restart(questionRenderer); // Restart �޼��� ȣ��
    }
}

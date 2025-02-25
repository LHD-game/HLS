using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public int questionIndex; // �� ��ư�� ���� ������ �ε���
    public int score; // �� ��ư�� �߰��� ����
    private ScoreManager scoreManager;

    // ScoreManager�� �����ϴ� �޼ҵ�
    public void Initialize(ScoreManager manager)
    {
        scoreManager = manager;
    }

    // ��ư�� Ŭ���� �� ȣ��Ǵ� �޼ҵ�
    public void OnButtonClick()
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(questionIndex, score);
        }
        else
        {
            Debug.LogError("ScoreManager�� �������� �ʾҽ��ϴ�.");
        }
    }
}

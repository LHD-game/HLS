using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public int questionIndex; // �� ��ư�� ���� ������ �ε���
    public int score; // �� ��ư�� �߰��� ����
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // ��ư�� Ŭ���� �� ȣ��Ǵ� �޼ҵ�
    public void OnButtonClick()
    {
        scoreManager.AddScore(questionIndex, score);
    }
}

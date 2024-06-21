using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public int questionIndex; // 이 버튼이 속한 질문의 인덱스
    public int score; // 이 버튼이 추가할 점수
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // 버튼이 클릭될 때 호출되는 메소드
    public void OnButtonClick()
    {
        scoreManager.AddScore(questionIndex, score);
    }
}

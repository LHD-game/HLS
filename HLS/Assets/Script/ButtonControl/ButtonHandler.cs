using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public int questionIndex; // 이 버튼이 속한 질문의 인덱스
    public int score; // 이 버튼이 추가할 점수
    private ScoreManager scoreManager;

    // ScoreManager를 설정하는 메소드
    public void Initialize(ScoreManager manager)
    {
        scoreManager = manager;
    }

    // 버튼이 클릭될 때 호출되는 메소드
    public void OnButtonClick()
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(questionIndex, score);
        }
        else
        {
            Debug.LogError("ScoreManager가 설정되지 않았습니다.");
        }
    }
}

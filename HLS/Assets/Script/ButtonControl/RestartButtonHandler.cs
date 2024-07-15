using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public Button restartButton; // Restart 버튼
    public ScoreManager scoreManager; // ScoreManager 스크립트
    public QuestionRenderer questionRenderer; // QuestionRenderer 스크립트

    void Start()
    {
        // Restart 버튼에 클릭 이벤트 추가
        restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    void OnRestartButtonClick()
    {
        // 점수 초기화 및 질문 렌더링 초기화
        scoreManager.Restart(questionRenderer);
    }
}

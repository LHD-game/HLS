using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public Button restartButton; // Restart 버튼
    public ScoreManager scoreManager; // ScoreManager 스크립트
    public QuestionRenderer questionRenderer; // QuestionRenderer 스크립트
    public Text restartButtonText;

    void Start()
    {
        restartButtonText = restartButton.GetComponentInChildren<Text>();
        restartButtonText.text = "다시";
        // Restart 버튼에 클릭 이벤트 추가
        restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    public void OnRestartButtonClick()
    {   

        // 점수 초기화 및 질문 렌더링 초기화
        scoreManager.Restart(questionRenderer);
        // toggleButtonManager.ResetButtonStates(); // 버튼 상태 초기화

    }
}

using UnityEngine;
using UnityEngine.UI;

public class RestartButtonHandler : MonoBehaviour
{
    public ScoreManager scoreManager; // ScoreManager 인스턴스
    public QuestionRenderer questionRenderer; // QuestionRenderer 인스턴스

    void Start()
    {
        // Restart 버튼에 클릭 이벤트를 추가
        GetComponent<Button>().onClick.AddListener(OnRestartButtonClick);
    }

    void OnRestartButtonClick()
    {
        scoreManager.Restart(questionRenderer); // Restart 메서드 호출
    }
}

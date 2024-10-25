using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class QuestionRendererParent : MonoBehaviour
{
    public Text questionText; // 질문 표시
    public GameObject buttonPrefab; // 버튼 프리팹 (AnswerButtonPrefab)
    public GameObject buttonPanel; // 버튼들이 생성될 패널
    public Button nextButton; // 다음 버튼
    public Button previousButton; // 이전 버튼

    public List<GameObject> activeButtons = new List<GameObject>(); // 생성된 버튼 리스트
    protected Dictionary<int, int> userSelections = new Dictionary<int, int>();

    // 버튼을 삭제하는 메서드
    public virtual void ClearButtons()
    {
        Debug.Log("ClearButtons 호출됨");

        foreach (Transform child in buttonPanel.transform)
        {
            Destroy(child.gameObject);
        }

        activeButtons.Clear(); // 리스트 비우기
        Debug.Log("activeButtons 리스트 초기화됨");
    }



    // 버튼을 동적으로 생성하는 메서드
    public virtual void CreateButton(string choiceText, int scoreIndex)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform);
        Text buttonText = newButton.GetComponentInChildren<Text>();
        buttonText.text = choiceText;

        Button button = newButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => OnButtonClick(scoreIndex));

        activeButtons.Add(newButton); // 생성된 버튼을 리스트에 추가
        newButton.SetActive(true);
    }

    // 선택된 버튼을 저장하고 활성화하는 메서드 (상속받아 재정의)
    public virtual void OnButtonClick(int scoreIndex)
    {
        Debug.Log("OnButtonClick 호출됨");
    }
}

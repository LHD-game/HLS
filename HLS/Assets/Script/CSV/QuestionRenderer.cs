using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionRenderer : MonoBehaviour
{
    public SurveyCsvReader csvReader;
    public GameObject buttonPrefab;
    public Transform buttonPanel;
    public Text questionText;

    public Button nextButton; // 다음 버튼을 참조할 변수
    public int currentQuestionIndex = 0;
    public List<GameObject> activeButtons = new List<GameObject>();

    // 현재 선택된 버튼을 저장할 변수
    private GameObject selectedButton;

    public IScoreManager scoreManager;

    private void Start()
    {
        // 처음에 다음 버튼 비활성화
        if (nextButton != null)
        {
            nextButton.interactable = false;
        }
    }

    public void ResetRenderer()
    {
        currentQuestionIndex = 0;
        ClearButtons();
        questionText.text = "";
        selectedButton = null;

        // 다음 버튼 비활성화
        if (nextButton != null)
        {
            nextButton.interactable = false;
        }

        Debug.Log("QuestionRenderer 초기화 완료");
    }

    public void setCsvReader()
    {
        if (csvReader == null)
        {
            Debug.LogError("CSV Reader가 설정되지 않았습니다.");
            return;
        }

        StartCoroutine(WaitForCSVData());
    }

    private IEnumerator WaitForCSVData()
    {
        while (csvReader.csvData.Count == 0)
        {
            yield return null;
        }

        Debug.Log("CSV 데이터 로드 완료, 질문 렌더링 시작");
        RenderQuestion();
    }

    public void RenderQuestion()
    {
        if (currentQuestionIndex >= csvReader.csvData.Count)
        {
            Debug.LogError("질문 인덱스가 csvData 범위를 벗어났습니다.");
            return;
        }

        string[] rowData = csvReader.csvData[currentQuestionIndex];
        questionText.text = rowData[0];

        ClearButtons();

        // "다음" 버튼은 선택이 필요할 때까지 비활성화
        if (nextButton != null)
        {
            nextButton.interactable = false;
        }

        for (int i = 1; i < rowData.Length; i++)
        {
            if (string.IsNullOrEmpty(rowData[i]))
            {
                break;
            }

            CreateButton(rowData[i]);
        }
    }

    private void CreateButton(string choiceText)
    {
        GameObject newAnswerPrefab = Instantiate(buttonPrefab, buttonPanel);

        Text buttonText = newAnswerPrefab.transform.Find("Text").GetComponent<Text>();
        buttonText.text = choiceText;

        Button answerButton = newAnswerPrefab.transform.Find("AnswerButtonPrefab").GetComponent<Button>();
        if (answerButton != null)
        {
            answerButton.onClick.AddListener(() => OnAnswerButtonClicked(answerButton));
        }

        activeButtons.Add(newAnswerPrefab);
    }

    private void ClearButtons()
    {
        foreach (GameObject button in activeButtons)
        {
            Destroy(button);
        }
        activeButtons.Clear();
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex + 1 < csvReader.csvData.Count)
        {
            currentQuestionIndex++;
            RenderQuestion();
        }
        else
        {
            Debug.Log("End of survey");
        }
    }

    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            RenderQuestion();
        }
    }


    // OnAnswerButtonClicked 메서드
    private void OnAnswerButtonClicked(Button button)
    {
        SetSelectedButtonColor(button);

        // 선택지 클릭 시 "다음" 버튼 활성화
        if (nextButton != null)
        {
            nextButton.interactable = true;
        }

        // 현재 질문 인덱스와 선택된 버튼 인덱스를 기반으로 점수 추가
        int answerIndex = activeButtons.IndexOf(button.transform.parent.gameObject); // 현재 버튼의 인덱스

        if (scoreManager != null)
        {
            scoreManager.AddScore(currentQuestionIndex, answerIndex);
            Debug.Log($"Score added: Question {currentQuestionIndex}, Answer {answerIndex}");
        }
    }



    private void SetSelectedButtonColor(Button button)
    {
        if (selectedButton != null)
        {
            selectedButton.GetComponent<Image>().color = Color.white;
        }

        button.GetComponent<Image>().color = Color.green;
        selectedButton = button.gameObject;
    }
}

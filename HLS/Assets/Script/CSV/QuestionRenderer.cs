using JetBrains.Annotations;
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
    public Text buttonText;

    public Button nextButton; // 다음 버튼을 참조할 변수
    public int currentQuestionIndex = 0;
    public List<GameObject> activeButtons = new List<GameObject>();

    public GameObject progressBar; // 진행 바 오브젝트
    public GameObject progressStepPrefab; // 진행 단계 하나에 해당하는 프리팹

    private List<GameObject> progressSteps = new List<GameObject>(); // 생성된 진행 단계 저장

    private GameObject selectedButton;
    public IScoreManager scoreManager;

    private void Start()
    {
        InitializeProgressBar();
        UpdateNextButtonState();  // 초기화 시 항상 비활성화
    }

    private void InitializeProgressBar()
    {
        int questionCount = csvReader.csvData.Count;
        for (int i = 0; i < questionCount; i++)
        {
            GameObject step = Instantiate(progressStepPrefab, progressBar.transform);
            progressSteps.Add(step);
        }
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        for (int i = 0; i < progressSteps.Count; i++)
        {
            if (i <= currentQuestionIndex)
            {
                progressSteps[i].GetComponent<Image>().color = Color.blue;
            }
            else
            {
                progressSteps[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }

    public void ResetRenderer()
    {
        currentQuestionIndex = 0;
        ClearButtons();
        questionText.text = "";
        selectedButton = null;
        UpdateNextButtonState();
        UpdateProgressBar();

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
        selectedButton = null;  // 새 질문 로드 시 선택 초기화
        UpdateNextButtonState(); // 새 질문 로드 시 항상 "다음" 비활성화

        UpdateProgressBar();

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
        buttonText = newAnswerPrefab.transform.Find("Text").GetComponent<Text>();
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

    private void OnAnswerButtonClicked(Button button)
    {
        if (selectedButton == button.gameObject)
        {
            DeselectButton(button);
        }
        else
        {
            SetSelectedButtonColor(button);
            UpdateNextButtonState();

            int answerIndex = activeButtons.IndexOf(button.transform.parent.gameObject);
            if (scoreManager != null)
            {
                scoreManager.AddScore(currentQuestionIndex, answerIndex);
                Debug.Log($"Score added: Question {currentQuestionIndex}, Answer {answerIndex}");
            }
        }
    }

    private void DeselectButton(Button button)
    {
        button.GetComponent<Image>().color = Color.white;
        selectedButton = null;
        UpdateNextButtonState();
    }

    private void SetSelectedButtonColor(Button button)
    {
        if (selectedButton != null)
        {
            selectedButton.GetComponent<Image>().color = Color.white;
        }
        selectedButton = button.gameObject;
    }

    private void UpdateNextButtonState()
    {
        if (nextButton != null)
        {
            // 선택된 버튼이 없으면 항상 비활성화
            nextButton.interactable = (selectedButton != null);

            // 마지막 질문에서는 "결과보기"로 텍스트 변경
            if (currentQuestionIndex == csvReader.csvData.Count - 1)
            {
                nextButton.GetComponentInChildren<Text>().text = "결과보기";
            }
            else
            {
                nextButton.GetComponentInChildren<Text>().text = "다음";
            }
        }
    }
}

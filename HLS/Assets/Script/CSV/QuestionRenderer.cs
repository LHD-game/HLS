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
    public List<GameObject> hlsButtonPrefabs; // 인스펙터에 HLS 버튼 프리팹 추가

    public Button nextButton; // 다음 버튼을 참조할 변수
    public int currentQuestionIndex = 0;
    public List<GameObject> activeButtons = new List<GameObject>();

    public GameObject progressBar; // 진행 바 오브젝트
    public GameObject progressStepPrefab; // 진행 단계 하나에 해당하는 프리팹

    private List<GameObject> progressSteps = new List<GameObject>(); // 생성된 진행 단계 저장

    private GameObject selectedButton;
    private GameObject selectedPrefab;
    public IScoreManager scoreManager;

    // HLS 관련 필드
    public GameObject hlsPanel;
    public bool isHLSMode = false;
    public List<Text> hlsQuestions = new List<Text>(); // 인스펙터에서 연결할 HLS 질문 Text
    public List<Transform> hlsButtonPanels = new List<Transform>(); // 인스펙터에서 연결할 HLS 버튼 패널

    private void Start()
    {
        InitializeProgressBar();
        UpdateNextButtonState();  // 초기화 시 항상 비활성화
    }

    public void SetupHLSLayout()
    {
        if (isHLSMode && hlsPanel != null)
        {
            hlsPanel.SetActive(true);
            buttonPanel.gameObject.SetActive(false); // 기본 패널 비활성화
            Debug.Log("Setting up HLS specific layout");
        }
        else
        {
            hlsPanel.SetActive(false);
            buttonPanel.gameObject.SetActive(true); // 기본 패널 활성화
        }
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

        if (isHLSMode)
        {
            SetupHLSLayout();
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
        if (isHLSMode)
        {
            RenderHLSQuestions();
        }
        else
        {
            RenderQuestion();
        }
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
        selectedPrefab = null;
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

    public void RenderHLSQuestions()
    {
        if (csvReader.csvData.Count < 5)
        {
            Debug.LogError("HLS용 데이터를 5개 이상 가져와야 합니다.");
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            string[] rowData = csvReader.csvData[currentQuestionIndex + i]; // 각 질문 데이터 가져오기
            hlsQuestions[i].text = rowData[0]; // 질문 텍스트 설정

            // 기존 버튼 제거
            foreach (Transform child in hlsButtonPanels[i])
            {
                Destroy(child.gameObject);
            }

            // 버튼 생성 로직 - HLS 전용 버튼 프리팹 사용
            for (int j = 1; j < rowData.Length; j++)
            {
                if (string.IsNullOrEmpty(rowData[j])) break;

                GameObject newButton = Instantiate(hlsButtonPrefabs[j - 1], hlsButtonPanels[i]);
                newButton.transform.Find("Text").GetComponent<Text>().text = rowData[j];

                Button btn = newButton.GetComponent<Button>();
                btn.onClick.AddListener(() => OnAnswerButtonClicked(btn));
            }
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
        if (isHLSMode)
        {
            currentQuestionIndex += 5; // HLS 모드에서는 한 번에 5개씩 이동
            RenderHLSQuestions();
        }
        else
        {
            if (currentQuestionIndex + 1 < csvReader.csvData.Count)
            {
                currentQuestionIndex++;
                RenderQuestion();
            }
            else
            {
                Debug.Log("결과보기");
            }
        }
    }

    public void PreviousQuestion()
    {
        if (isHLSMode)
        {
            // HLS 모드에서는 한 번에 5개씩 이동
            currentQuestionIndex -= 5;
            if (currentQuestionIndex < 0)
            {
                currentQuestionIndex = 0; // 인덱스가 0보다 작아지지 않도록 보정
            }
            RenderHLSQuestions();
        }
        else
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                RenderQuestion();
            }
        }
    }


    private void OnAnswerButtonClicked(Button button)
    {
        if (selectedButton == button.gameObject)
        {
            UpdateNextButtonState();
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

    private void SetSelectedButtonColor(Button button)
    {
        if (selectedButton != null)
        {
            selectedButton.GetComponent<Image>().color = Color.white;
        }

        selectedButton = button.gameObject;
        selectedButton.GetComponent<Image>().color = new Color32(92, 114, 207, 255); // 5C72CF 색상
    }

    private void UpdateNextButtonState()
    {
        if (nextButton != null)
        {
            nextButton.interactable = (selectedButton != null);

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

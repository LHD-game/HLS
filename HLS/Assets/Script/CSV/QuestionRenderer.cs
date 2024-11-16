using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using static UnityEngine.PlayerPrefs;
using System.Collections.Specialized;

public class QuestionRenderer : MonoBehaviour
{
    [Header("script")]
    public SurveyCsvReader csvReader;
    [Space(5f)]
    public GameObject buttonPrefab;
    public Transform buttonPanel;
    public Text questionText;
    public Text buttonText;
    public List<GameObject> hlsButtonPrefabs; // �ν����Ϳ� HLS ��ư ������ �߰�

    public Button nextButton; // ���� ��ư�� ������ ����
    public int currentQuestionIndex = 0;
    public List<GameObject> activeButtons = new List<GameObject>();

    public GameObject progressBar; // ���� �� ������Ʈ
    public GameObject progressStepPrefab; // ���� �ܰ� �ϳ��� �ش��ϴ� ������

    private List<GameObject> progressSteps = new List<GameObject>(); // ������ ���� �ܰ� ����

    private GameObject selectedButton;
    private GameObject selectedPrefab;
    public IScoreManager scoreManager;

    // ���� ���� ���� 
    public string Ugen;
    public string Uname;


    // HLS ���� �ʵ�
    /* public GameObject hlsPanel;
     public bool isHLSMode = false;
     public List<Text> hlsQuestions = new List<Text>(); // �ν����Ϳ��� ������ HLS ���� Text
     public List<Transform> hlsButtonPanels = new List<Transform>(); // �ν����Ϳ��� ������ HLS ��ư �г�
 */
    // ���â ���� �ʵ� 
    public Text typeText;
    public Text scoreText;
    public Text noticeText;
    
    private void Start()
    {
        Ugen = PlayerPrefs.GetString("MF");
        Uname = PlayerPrefs.GetString("UserName");
        setCsvReader();
        ResetRenderer();
        InitializeProgressBar();
        UpdateNextButtonState();  // �ʱ�ȭ �� �׻� ��Ȱ��ȭ
        noticeText.text = $"{Uname}���� ������ {scoreText.text}�Դϴ�.";
        Debug.Log($"{Uname}���� ������ {scoreText.text}�Դϴ�.");
    }
   
   /* public void SetupHLSLayout()
    {
        if (isHLSMode && hlsPanel != null)
        {
            hlsPanel.SetActive(true);
            buttonPanel.gameObject.SetActive(false); // �⺻ �г� ��Ȱ��ȭ
            Debug.Log("Setting up HLS specific layout");
        }
        else
        {
            hlsPanel.SetActive(false);
            buttonPanel.gameObject.SetActive(true); // �⺻ �г� Ȱ��ȭ
        }
    }*/

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
        /*if (isHLSMode)
        {
            SetupHLSLayout();
        }*/

        Debug.Log("QuestionRenderer �ʱ�ȭ �Ϸ�");
    }

    public void setCsvReader()
    {
        Debug.Log("setCsvReader ȣ���");
        ResetRenderer();
        if (csvReader == null)
        {
            Debug.LogError("CSV Reader�� �������� �ʾҽ��ϴ�.");
            return;
        }
        csvReader.SetFiles(); // ���� ���� �� ������ �ε� ����
        StartCoroutine(WaitForCSVData());
    }

    private IEnumerator WaitForCSVData()
    {
        while (csvReader.csvData.Count == 0)
        {
            yield return null;
        }
        RenderQuestion();

        Debug.Log("CSV ������ �ε� �Ϸ�, ���� ������ ����");
        
        /*if (isHLSMode)
        {
            RenderHLSQuestions();
        }
        else
        {
            RenderQuestion();
        }*/
    }

    public void RenderQuestion()
    {
        if (currentQuestionIndex >= csvReader.csvData.Count)
        {
            Debug.LogError("���� �ε����� csvData ������ ������ϴ�.");
            return;
        }

        string[] rowData = csvReader.csvData[currentQuestionIndex];
        questionText.text = rowData[0];

        ClearButtons();
        selectedButton = null;  // �� ���� �ε� �� ���� �ʱ�ȭ
        selectedPrefab = null;
        UpdateNextButtonState(); // �� ���� �ε� �� �׻� "����" ��Ȱ��ȭ

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

    /*public void RenderHLSQuestions()
    {
        if (csvReader.csvData.Count < 5)
        {
            Debug.LogError("HLS�� �����͸� 5�� �̻� �����;� �մϴ�.");
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            string[] rowData = csvReader.csvData[currentQuestionIndex + i]; // �� ���� ������ ��������
            hlsQuestions[i].text = rowData[0]; // ���� �ؽ�Ʈ ����

            // ���� ��ư ����
            foreach (Transform child in hlsButtonPanels[i])
            {
                Destroy(child.gameObject);
            }

            // ��ư ���� ���� - HLS ���� ��ư ������ ���
            for (int j = 1; j < rowData.Length; j++)
            {
                if (string.IsNullOrEmpty(rowData[j])) break;

                GameObject newButton = Instantiate(hlsButtonPrefabs[j - 1], hlsButtonPanels[i]);
                newButton.transform.Find("Text").GetComponent<Text>().text = rowData[j];

                Button btn = newButton.GetComponent<Button>();
                btn.onClick.AddListener(() => OnAnswerButtonClicked(btn));
            }
        }
    }*/

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
        /*if (isHLSMode)
        {
            currentQuestionIndex += 5; // HLS ��忡���� �� ���� 5���� �̵�
            RenderHLSQuestions();
        }
        else
        {*/
        if (currentQuestionIndex + 1 < csvReader.csvData.Count)
        {
            currentQuestionIndex++;
            RenderQuestion();
        }
        else
        {
            scoreManager.SetData();
            Debug.Log("�������");
            Debug.Log(scoreText);
        }
        //}
    }

    public void PreviousQuestion()
    {
        /*if (isHLSMode)
        {
            // HLS ��忡���� �� ���� 5���� �̵�
            currentQuestionIndex -= 5;
            if (currentQuestionIndex < 0)
            {
                currentQuestionIndex = 0; // �ε����� 0���� �۾����� �ʵ��� ����
            }
            RenderHLSQuestions();
        }
        else
        {*/
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            RenderQuestion();
        }
        //}
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
        selectedButton.GetComponent<Image>().color = new Color32(92, 114, 207, 255); // 5C72CF ����
    }

    private void UpdateNextButtonState()
    {
        if (nextButton != null)
        {
            nextButton.interactable = (selectedButton != null);

            if (currentQuestionIndex == csvReader.csvData.Count - 1)
            {
                nextButton.GetComponentInChildren<Text>().text = "�������";
            }
            else
            {
                nextButton.GetComponentInChildren<Text>().text = "����";
            }
        }
    }
}

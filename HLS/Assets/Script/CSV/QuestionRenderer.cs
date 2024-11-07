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

    public Button nextButton; // ���� ��ư�� ������ ����
    public int currentQuestionIndex = 0;
    public List<GameObject> activeButtons = new List<GameObject>();

    public GameObject progressBar; // ���� �� ������Ʈ
    public GameObject progressStepPrefab; // ���� �ܰ� �ϳ��� �ش��ϴ� ������

    private List<GameObject> progressSteps = new List<GameObject>(); // ������ ���� �ܰ� ����

    private GameObject selectedButton;
    private GameObject selectedPrefab;
    public IScoreManager scoreManager;

    private void Start()
    {
        InitializeProgressBar();
        UpdateNextButtonState();  // �ʱ�ȭ �� �׻� ��Ȱ��ȭ
    }

    public void SetupHLSLayout()
    {
        // HLS ���� ���̾ƿ� ���� ��
        // ����: Ư�� UI �г� Ȱ��ȭ/��Ȱ��ȭ, ���̾ƿ� ���� ���� ��
        Debug.Log("Setting up HLS specific layout");
        // �ʿ��� ���̾ƿ� ������ ���⿡ �߰�
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

        Debug.Log("QuestionRenderer �ʱ�ȭ �Ϸ�");
    }

    public void setCsvReader()
    {
        if (csvReader == null)
        {
            Debug.LogError("CSV Reader�� �������� �ʾҽ��ϴ�.");
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

        Debug.Log("CSV ������ �ε� �Ϸ�, ���� ������ ����");
        RenderQuestion();
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
            // DeselectButton(button);
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

    private void DeselectButton(Button button)
    {
        return;
    }

    private void SetSelectedButtonColor(Button button)
    {
        if (selectedButton != null)
        {
            // ���� ������ ��ư�� ������� ����
            selectedButton.GetComponent<Image>().color = Color.white;
        }

        // ���õ� ��ư ���� �� ���� ����
        selectedButton = button.gameObject;
        selectedButton.GetComponent<Image>().color = new Color32(92, 114, 207, 255);// 5C72CF ����
    }


    private void UpdateNextButtonState()
    {
        if (nextButton != null)
        {
            // ���õ� ��ư�� ������ �׻� ��Ȱ��ȭ
            nextButton.interactable = (selectedButton);

            // ������ ���������� "�������"�� �ؽ�Ʈ ����
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

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

    public Button nextButton; // ���� ��ư�� ������ ����
    public int currentQuestionIndex = 0;
    public List<GameObject> activeButtons = new List<GameObject>();

    // ���� ���õ� ��ư�� ������ ����
    private GameObject selectedButton;

    public IScoreManager scoreManager;

    private void Start()
    {
        // ó���� ���� ��ư ��Ȱ��ȭ
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

        // ���� ��ư ��Ȱ��ȭ
        if (nextButton != null)
        {
            nextButton.interactable = false;
        }

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

        // "����" ��ư�� ������ �ʿ��� ������ ��Ȱ��ȭ
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


    // OnAnswerButtonClicked �޼���
    private void OnAnswerButtonClicked(Button button)
    {
        SetSelectedButtonColor(button);

        // ������ Ŭ�� �� "����" ��ư Ȱ��ȭ
        if (nextButton != null)
        {
            nextButton.interactable = true;
        }

        // ���� ���� �ε����� ���õ� ��ư �ε����� ������� ���� �߰�
        int answerIndex = activeButtons.IndexOf(button.transform.parent.gameObject); // ���� ��ư�� �ε���

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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers; // ToggleButtonManager ����Ʈ
    public ScoreManager scoreManager;
    private Button nextButton; // Next ��ư
    public GameObject resultPanel; // ��� �г�

    private Text nextButtonText; // Next ��ư�� �ؽ�Ʈ ������Ʈ
    public Text restartButtonText;

    private int totalQuestions = 36; // �� ���� ����
    private int questionsPerSet = 4; // �� ��Ʈ�� ���� ��
    private int currentQuestionIndex = 0; // ���� ���� �ε���

    void Start()
    {
        // �ڽ� ������Ʈ���� Button ������Ʈ ã��
        nextButton = GetComponentInChildren<Button>();
        if (nextButton == null)
        {
            Debug.LogError("Next button component is not found on the GameObject or its children.");
            return;
        }

        // �ڽ� ������Ʈ���� Text ������Ʈ ã��
        nextButtonText = nextButton.GetComponentInChildren<Text>();
        if (nextButtonText == null)
        {
            Debug.LogError("Text component is not found on the Button or its children.");
            return;
        }

        if (restartButtonText == null)
        {
            Debug.LogError("Restart button text is not assigned.");
            return;
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);
        UpdateNextButtonState();
        resultPanel.SetActive(false); // ��� �г� ��Ȱ��ȭ
    }

    void Update()
    {
        UpdateNextButtonState();
    }

    public void UpdateNextButtonState()  //������Ʈ���� �ƴ� �ٸ���� ���� �� �� ���� ������.
    {
        if (nextButton == null)
        {
            Debug.LogError("Next button is not assigned.");
            return;
        }

        bool allQuestionsAnswered = true;
        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            if (toggleButtonManager == null || !toggleButtonManager.IsQuestionAnswered())
            {
                allQuestionsAnswered = false;
                break;
            }
        }

        nextButton.interactable = allQuestionsAnswered;

        if (nextButton.interactable)
        {
            //Debug.Log("Next button is now interactable.");
        }
        else
        {
            //Debug.Log("Next button is now NOT interactable.");
        }

        restartButtonText.text = "ó������";

        if (currentQuestionIndex >= totalQuestions - questionsPerSet)
        {
            if (nextButtonText != null)
            {
                nextButtonText.text = "�������";
            }
        }
        else
        {
            if (nextButtonText != null)
            {
                nextButtonText.text = "��������";
            }
        }
    }



    public void OnNextButtonClicked()
    {
        if (nextButton != null && nextButton.interactable)
        {
            foreach (var toggleButtonManager in toggleButtonManagers)
            {
                if (toggleButtonManager != null)
                {
                    toggleButtonManager.ResetButtonStates(); // ��ư ���� �ʱ�ȭ
                }
            }

            // ���� ���� ��Ʈ�� �̵��ϴ� ������ ���⿡ �߰�
            currentQuestionIndex += questionsPerSet;

            // ������ ���� ��Ʈ�̸� ���â�� ���
            if (currentQuestionIndex >= totalQuestions)
            {
                resultPanel.SetActive(true); // ��� �г� Ȱ��ȭ
                Time.timeScale = 1f; // ���� �Ͻ�����
            }
            else
            {
                UpdateNextButtonState(); // ���� ���� ��Ʈ�� �̵� �� ���� ������Ʈ
            }
        }
    }
}

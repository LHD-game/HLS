using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers; // ToggleButtonManager ����Ʈ
    public ScoreManager scoreManager;
    private Button nextButton; // Next ��ư
    public GameObject resultPanel; // ��� �г�

    private Text nextButtonText; // Next ��ư�� �ؽ�Ʈ
    public Text restartButtonText;

    private int totalQuestions = 36; // �� ���� ����
    private int questionsPerSet = 4; // �� ��Ʈ�� ���� ��
    private int currentQuestionIndex = 0; // ���� ���� �ε���
    

    void Start()
    {
        nextButton = GetComponent<Button>();
        nextButtonText = nextButton.GetComponentInChildren<Text>(); // Next ��ư�� �ؽ�Ʈ ������Ʈ ��������

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
        else
        {
            Debug.LogError("Next button component is not found on the GameObject.");
        }
        UpdateNextButtonState();
        resultPanel.SetActive(false); // ��� �г� ��Ȱ��ȭ
    }

    void Update()
    {
        UpdateNextButtonState();
    }

    void UpdateNextButtonState()
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

        restartButtonText.text = "ó�����ʹٽäӤӤ�";

        // ������ ���� ��Ʈ��� ��ư �ؽ�Ʈ�� "�������"�� ����
        if (currentQuestionIndex >= totalQuestions - questionsPerSet)
        {
            nextButtonText.text = "�������";
        }
        else
        {
            nextButtonText.text = "��������";
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
                Time.timeScale = 0f; // ���� �Ͻ�����
            }
            else
            {
                UpdateNextButtonState(); // ���� ���� ��Ʈ�� �̵� �� ���� ������Ʈ
            }
        }
    }
}

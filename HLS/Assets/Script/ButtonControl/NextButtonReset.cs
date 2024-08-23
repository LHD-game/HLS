using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers;
    public ScoreManager scoreManager;
    private Button nextButton;
    public GameObject resultPanel;
    private Text nextButtonText;
    public Text restartButtonText;
    private int totalQuestions = 36;
    private int questionsPerSet = 4;
    private int currentQuestionIndex = 0;

    void Start()
    {
        nextButton = GetComponentInChildren<Button>();
        if (nextButton == null)
        {
            Debug.LogError("Next ��ư ������Ʈ�� GameObject �Ǵ� �ڽ� ������Ʈ���� ã�� �� �����ϴ�.");
            return;
        }

        nextButtonText = nextButton.GetComponentInChildren<Text>();
        if (nextButtonText == null)
        {
            Debug.LogError("Text ������Ʈ�� Button �Ǵ� �ڽ� ������Ʈ���� ã�� �� �����ϴ�.");
            return;
        }

        if (restartButtonText == null)
        {
            Debug.LogError("Restart ��ư �ؽ�Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);

        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            if (toggleButtonManager != null)
            {
                toggleButtonManager.OnToggleStateChanged += UpdateNextButtonState;  // Update�� ��ü �κ� (ToggleButtonManager : ����� ���°� ����� �����ٸ� �����Ͽ� UpdateNextButtonState()�� ȣ��)
            }
        }

        UpdateNextButtonState();
        resultPanel.SetActive(false);
    }

    public void UpdateNextButtonState()
    {
        if (nextButton == null)
        {
            Debug.LogError("Next ��ư�� �Ҵ���� �ʾҽ��ϴ�.");
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
                    toggleButtonManager.ResetButtonStates();
                }
            }

            currentQuestionIndex += questionsPerSet;

            if (currentQuestionIndex >= totalQuestions)
            {
                resultPanel.SetActive(true);
                Time.timeScale = 1f;
            }
            else
            {
                UpdateNextButtonState();
            }
        }
    }
}

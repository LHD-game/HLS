using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers;
    public ScoreManager scoreManager;
    public RaderDraw RD;

    private Button nextButton;
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
                restartButtonText.text = "ó������";
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
                    ResetToggleButtonVisuals(toggleButtonManager); // ��ư �ð��� ���� �ʱ�ȭ
                }
            }

            currentQuestionIndex += questionsPerSet;

            if (currentQuestionIndex >= totalQuestions)
            {
                RD.addData();
                Time.timeScale = 1f;
            }
            else
            {
                UpdateNextButtonState();
            }
        }
    }

    private void ResetToggleButtonVisuals(ToggleButtonManager toggleButtonManager)
    {
        foreach (var toggle in toggleButtonManager.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false; // ��� ���� �ʱ�ȭ
            toggle.graphic.CrossFadeColor(Color.white, 0f, true, true); // �׷��� ���� �ʱ�ȭ
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers;
    public ScoreManager scoreManager;
    public RaderDraw RD;

    private Button nextButton;
    public Text nextButtonText;  // Text ������Ʈ�� Inspector���� ���� ����
    //public Text restartButtonText;
    private int totalQuestions = 36;
    private int questionsPerSet = 4;
    private int currentQuestionIndex = 0;

    //���� ��ũ��Ʈ?
    void Start()
    {
        nextButton = GetComponentInChildren<Button>();  // �� �κ��� ������� �Ӵϴ�.
        if (nextButton == null)
        {
            Debug.LogError("Next ��ư ������Ʈ�� ã�� �� �����ϴ�."); //������Ʈ�� find�� ã�°� �ƴ� ���� �Ҵ� ������ �ֽ��ϴ�.
            return;
        }

        // Text ������Ʈ �Ҵ� Ȯ��
        if (nextButtonText == null)
        {
            Debug.LogError("Next ��ư�� Text ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        /*if (restartButtonText == null)
        {
            Debug.LogError("Restart ��ư �ؽ�Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }*/

        nextButton.onClick.AddListener(OnNextButtonClicked);

        foreach (var toggleButtonManager in toggleButtonManagers)
        {
            if (toggleButtonManager != null)
            {
                toggleButtonManager.OnToggleStateChanged += UpdateNextButtonState;
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


        //restartButtonText.text = "ó������";

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
                //restartButtonText.text = "ó������";
            }
        }

        nextButton.interactable = allQuestionsAnswered; //text���� �� Ȱ��ȭ
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
                    ResetToggleButtonVisuals(toggleButtonManager);  // ��ư �ð��� ���� �ʱ�ȭ
                }
            }

            currentQuestionIndex += questionsPerSet;

            if (currentQuestionIndex >= totalQuestions)
            {
                //WinCtl.Instance.GotoDatailWin();
                //RD.addData();
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
            toggle.isOn = false;  // ��� ���� �ʱ�ȭ
            toggle.graphic.CrossFadeColor(Color.white, 0f, true, true);  // �׷��� ���� �ʱ�ȭ
        }
    }
}

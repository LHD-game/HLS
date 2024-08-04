using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextButtonReset : MonoBehaviour
{
    public List<ToggleButtonManager> toggleButtonManagers; // ToggleButtonManager ����Ʈ
    public ScoreManager scoreManager;
    private Button nextButton; // Next ��ư

    void Start()
    {
        nextButton = GetComponent<Button>();

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
        else
        {
            Debug.LogError("Next button component is not found on the GameObject.");
        }
        UpdateNextButtonState();
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
        }
    }
}

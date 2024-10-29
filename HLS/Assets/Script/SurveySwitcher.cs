using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SurveySwitcher : MonoBehaviour
{
    [SerializeField] public GameObject surveyPanel;
    [SerializeField] private Transform buttonPanelTransform; // ��ư���� �ִ� Panel�� Transform�� �޾ƿ�
    [SerializeField] private List<Button> surveyButtons; // Inspector���� ��ư���� �Ҵ�
    [SerializeField] private SurveyCsvReader csvReader; // ���� CsvReader

    public ChooseSurvey chooseSurvey;

    private QuestionRendererParent currentQuestionRenderer; // ���� ������
    private QuestionRendererParent previousQuestionRenderer; // ���� ������

    public void OnSurveyButtonClicked(string buttonName)
    {
        surveyPanel.SetActive(true);

        /*// ������ ��ü �� �ʱ�ȭ
        ClearPanel();
        if (previousQuestionRenderer != null)
        {
            previousQuestionRenderer.ClearButtons();
        }

        // ���� �������� ������Ʈ
        previousQuestionRenderer = currentQuestionRenderer;*/
    }

    // ���� ��ư ����
    public void ClearPanel()
    {
        // ButtonPanel �ȿ� �ִ� ���� ��ư�� ��� ����
        foreach (Transform child in buttonPanelTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnClickBack()
    {
        // ���ư��� ��ư Ŭ�� �� �г� �ʱ�ȭ �� ������ �ʱ�ȭ
        surveyPanel.SetActive(false);
        ClearPanel();
        if (currentQuestionRenderer != null)
        {
            currentQuestionRenderer.ClearButtons();
        }
    }
}

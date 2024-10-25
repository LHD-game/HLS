using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveySwitcher : MonoBehaviour
{
    [SerializeField] private GameObject surveyPanel;
    [SerializeField] private Transform buttonPanelTransform; // ��ư���� �ִ� Panel�� Transform�� �޾ƿ�

    [SerializeField] private CsvReaderParent firstQuestionCsvReader;
    private QuestionRendererParent currentQuestionRenderer;

    // ������ ����ߴ� �˻��� �������� ����ϴ� ����
    private QuestionRendererParent previousQuestionRenderer;


    public void OnClickQuestionButton(CsvReaderParent csvReader)
    {

        // ���ο� �˻����� �������� ����
        currentQuestionRenderer = csvReader.GetComponent<QuestionRendererParent>();

        // ���ο� �˻��� ������ ����
        surveyPanel.SetActive(true);
        csvReader.StartLoadCsvData();

        // ���� QuestionRenderer�� ���� ���·� ����
        previousQuestionRenderer = currentQuestionRenderer;
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
        // ���ư��� ��ư Ŭ�� �� �г��� �ʱ�ȭ
        ClearPanel();
        if (currentQuestionRenderer != null)
        {
            currentQuestionRenderer.ClearButtons();
        }
    }
}

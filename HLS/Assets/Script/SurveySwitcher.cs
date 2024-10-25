using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveySwitcher : MonoBehaviour
{
    [SerializeField] private GameObject surveyPanel;
    [SerializeField] private Transform buttonPanelTransform; // 버튼들이 있는 Panel의 Transform을 받아옴

    [SerializeField] private CsvReaderParent firstQuestionCsvReader;
    private QuestionRendererParent currentQuestionRenderer;

    // 이전에 사용했던 검사지 렌더러를 기억하는 변수
    private QuestionRendererParent previousQuestionRenderer;


    public void OnClickQuestionButton(CsvReaderParent csvReader)
    {

        // 새로운 검사지의 렌더러를 설정
        currentQuestionRenderer = csvReader.GetComponent<QuestionRendererParent>();

        // 새로운 검사지 렌더링 시작
        surveyPanel.SetActive(true);
        csvReader.StartLoadCsvData();

        // 현재 QuestionRenderer를 이전 상태로 저장
        previousQuestionRenderer = currentQuestionRenderer;
    }

    // 기존 버튼 제거
    public void ClearPanel()
    {
        // ButtonPanel 안에 있는 기존 버튼들 모두 제거
        foreach (Transform child in buttonPanelTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnClickBack()
    {
        // 돌아가기 버튼 클릭 시 패널을 초기화
        ClearPanel();
        if (currentQuestionRenderer != null)
        {
            currentQuestionRenderer.ClearButtons();
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SurveySwitcher : MonoBehaviour
{
    [SerializeField] public GameObject surveyPanel;
    [SerializeField] private Transform buttonPanelTransform; // 버튼들이 있는 Panel의 Transform을 받아옴
    [SerializeField] private List<Button> surveyButtons; // Inspector에서 버튼들을 할당
    [SerializeField] private SurveyCsvReader csvReader; // 통합 CsvReader

    public ChooseSurvey chooseSurvey;

    private QuestionRendererParent currentQuestionRenderer; // 현재 렌더러
    private QuestionRendererParent previousQuestionRenderer; // 이전 렌더러

    public void OnSurveyButtonClicked(string buttonName)
    {
        surveyPanel.SetActive(true);

        /*// 렌더러 교체 전 초기화
        ClearPanel();
        if (previousQuestionRenderer != null)
        {
            previousQuestionRenderer.ClearButtons();
        }

        // 현재 렌더러를 업데이트
        previousQuestionRenderer = currentQuestionRenderer;*/
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
        // 돌아가기 버튼 클릭 시 패널 초기화 및 렌더러 초기화
        surveyPanel.SetActive(false);
        ClearPanel();
        if (currentQuestionRenderer != null)
        {
            currentQuestionRenderer.ClearButtons();
        }
    }
}

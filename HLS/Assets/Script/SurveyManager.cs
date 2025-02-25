using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SurveyManager : MonoBehaviour
{
    /*public Text surveyTitle; // 질문 텍스트를 표시할 UI 요소
    public Button nextButton;
    public Button previousButton;
    public GameObject buttonPanel; // 버튼이 들어가는 패널
    public GameObject buttonPrefab; // 선택지 버튼 프리팹 추가
    public List<MonoBehaviour> questionRenderers; // 각 검사지에 맞는 QuestionRenderer들

    public Text questionText; // QuestionText (질문을 표시할 곳)
    public Text buttonText; // Text(Legacy) (선택지를 표시할 곳)

    private int currentSurveyIndex = 0;

    void Start()
    {
        LoadSurveyByIndex(0); // 기본 첫 번째 설문지 로드
    }

    public void LoadSurveyByIndex(int index)
    {
        if (index < 0 || index >= questionRenderers.Count)
        {
            Debug.LogError("설문지 인덱스가 범위를 벗어났습니다.");
            return;
        }

        // QuestionRenderer를 통해 질문과 선택지를 로드
        var currentRenderer = questionRenderers[index] as AdkQuestionRenderer;
        if (currentRenderer != null)
        {
            // 질문 텍스트를 업데이트
            //questionText.text = currentRenderer.GetQuestionText();

            // 기존에 생성된 버튼들을 모두 제거
            foreach (Transform child in buttonPanel.transform)
            {
                Destroy(child.gameObject); // 기존 버튼 제거
            }

            // 선택지 텍스트를 업데이트
            for (int i = 0; i < 5; i++)
            {
                // 선택지 버튼을 생성 및 텍스트 설정
                //string choiceText = currentRenderer.GetChoiceText(i + 1);
                //if (!string.IsNullOrEmpty(choiceText))
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform); // 버튼 생성
                    Text buttonText = newButton.GetComponentInChildren<Text>(); // 버튼의 텍스트 찾기
                    if (buttonText != null)
                    {
                        //buttonText.text = choiceText; // 선택지 텍스트 설정
                    }
                }
            }
        }
    }

    public void OnChangeSurvey(int surveyIndex)
    {
        LoadSurveyByIndex(surveyIndex); // 다른 설문지로 변경
    }*/
}

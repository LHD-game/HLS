using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SurveyManager : MonoBehaviour
{
    /*public Text surveyTitle; // ���� �ؽ�Ʈ�� ǥ���� UI ���
    public Button nextButton;
    public Button previousButton;
    public GameObject buttonPanel; // ��ư�� ���� �г�
    public GameObject buttonPrefab; // ������ ��ư ������ �߰�
    public List<MonoBehaviour> questionRenderers; // �� �˻����� �´� QuestionRenderer��

    public Text questionText; // QuestionText (������ ǥ���� ��)
    public Text buttonText; // Text(Legacy) (�������� ǥ���� ��)

    private int currentSurveyIndex = 0;

    void Start()
    {
        LoadSurveyByIndex(0); // �⺻ ù ��° ������ �ε�
    }

    public void LoadSurveyByIndex(int index)
    {
        if (index < 0 || index >= questionRenderers.Count)
        {
            Debug.LogError("������ �ε����� ������ ������ϴ�.");
            return;
        }

        // QuestionRenderer�� ���� ������ �������� �ε�
        var currentRenderer = questionRenderers[index] as AdkQuestionRenderer;
        if (currentRenderer != null)
        {
            // ���� �ؽ�Ʈ�� ������Ʈ
            //questionText.text = currentRenderer.GetQuestionText();

            // ������ ������ ��ư���� ��� ����
            foreach (Transform child in buttonPanel.transform)
            {
                Destroy(child.gameObject); // ���� ��ư ����
            }

            // ������ �ؽ�Ʈ�� ������Ʈ
            for (int i = 0; i < 5; i++)
            {
                // ������ ��ư�� ���� �� �ؽ�Ʈ ����
                //string choiceText = currentRenderer.GetChoiceText(i + 1);
                //if (!string.IsNullOrEmpty(choiceText))
                {
                    GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform); // ��ư ����
                    Text buttonText = newButton.GetComponentInChildren<Text>(); // ��ư�� �ؽ�Ʈ ã��
                    if (buttonText != null)
                    {
                        //buttonText.text = choiceText; // ������ �ؽ�Ʈ ����
                    }
                }
            }
        }
    }

    public void OnChangeSurvey(int surveyIndex)
    {
        LoadSurveyByIndex(surveyIndex); // �ٸ� �������� ����
    }*/
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class QuestionRendererParent : MonoBehaviour
{
    public Text questionText; // ���� ǥ��
    public GameObject buttonPrefab; // ��ư ������ (AnswerButtonPrefab)
    public GameObject buttonPanel; // ��ư���� ������ �г�
    public Button nextButton; // ���� ��ư
    public Button previousButton; // ���� ��ư

    public List<GameObject> activeButtons = new List<GameObject>(); // ������ ��ư ����Ʈ
    protected Dictionary<int, int> userSelections = new Dictionary<int, int>();

    // ��ư�� �����ϴ� �޼���
    public virtual void ClearButtons()
    {
        Debug.Log("ClearButtons ȣ���");

        foreach (Transform child in buttonPanel.transform)
        {
            Destroy(child.gameObject);
        }

        activeButtons.Clear(); // ����Ʈ ����
        Debug.Log("activeButtons ����Ʈ �ʱ�ȭ��");
    }



    // ��ư�� �������� �����ϴ� �޼���
    public virtual void CreateButton(string choiceText, int scoreIndex)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonPanel.transform);
        Text buttonText = newButton.GetComponentInChildren<Text>();
        buttonText.text = choiceText;

        Button button = newButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => OnButtonClick(scoreIndex));

        activeButtons.Add(newButton); // ������ ��ư�� ����Ʈ�� �߰�
        newButton.SetActive(true);
    }

    // ���õ� ��ư�� �����ϰ� Ȱ��ȭ�ϴ� �޼��� (��ӹ޾� ������)
    public virtual void OnButtonClick(int scoreIndex)
    {
        Debug.Log("OnButtonClick ȣ���");
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using JetBrains.Annotations;
using UnityEngine.UI;

public class AdkScoreManager : MonoBehaviour, IScoreManager
{
    private Dictionary<int, int> questionScores = new Dictionary<int, int>(); // �� ������ ���� ���� ����
    public int totalScore { get; set; } // ����

    public QuestionRenderer questionRenderer;
    public string gender;
    public Text level;
    public Text notice;
    public string name;
    public Image targetImage;

    public Dictionary<string, string> ScoreData { get; private set; }

    // ���� �߰� (������ �ε����� �״�� ������ ���)
    public void AddScore(int questionIndex, int answerIndex)
    {
        // ������ �ε����� ������ ó�� (0������ ����)
        int score = answerIndex; // ������ �ε����� �״�� ������ ���
        questionScores[questionIndex] = score; // �ش� ������ ������ ����
        gender = questionRenderer.Ugen; // ���� ���� �ҷ�����
        name = questionRenderer.Uname;
        level = questionRenderer.levelText;
        notice = questionRenderer.noticeText;
        
        // �� ���� ����
        CalculateTotalScore();

        if (gender == "M")
        {
            if (totalScore <= 9)
            {
                level.text = "�������ֱ�";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
                if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
            }

            else if (totalScore <= 19)
            {
                level.text = "�������ֱ�";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/dangerorange");
                if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
            }

            else
            {
                level.text = "���ڿ� ������";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
                if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
            }
        }

        else
        {
            if (totalScore <= 5)
            {
                level.text = "�������ֱ�";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/good");
                if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
            }

            else if (totalScore <= 9)
            {
                level.text = "�������ֱ�";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/dangerorange");
                if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����
            }

            else
            {
                level.text = "���ڿ� ������";
                Sprite newSprite = Resources.Load<Sprite>("sprite/TestUI/danger");
                if (newSprite != null) targetImage.sprite = newSprite; // ��������Ʈ ����

            }
        }

        notice.text = $"{name}���� ������ {totalScore}�� �Դϴ�.";
    }

    // ���� ���
    private void CalculateTotalScore()
    {
        totalScore = 0;
        foreach (int score in questionScores.Values)
        {
            totalScore += score;
        }

        // ���� ����� �޽��� ���
        Debug.Log("Total Score: " + totalScore);
        // UI�� ���� ǥ��
        if (questionRenderer != null && questionRenderer.scoreText != null)
        {
            questionRenderer.scoreText.text = totalScore.ToString();
        }
        else
        {
            Debug.LogError("questionRenderer �Ǵ� scoreText�� null�Դϴ�.");
        }

    }

  

    [Header("script")]
    public ScoreData sd;
    public RaderDraw rd;
    public void SetData()
    {
        ScoreData = new Dictionary<string, string>();

        questionRenderer.OtherTestComplete();
        ScoreData.Add("total", totalScore.ToString());
        rd.addotherData(ScoreData, "AUDIT");
    }

    // ���� �ʱ�ȭ
    public void ResetScores()
    {
        questionScores.Clear();
        totalScore = 0;
        Debug.Log("Scores Reset");
    }
}

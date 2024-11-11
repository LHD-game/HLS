using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseSurvey : MonoBehaviour
{
    public SurveyCsvReader csvReader;
    public SurveySwitcher switcher;
    public QuestionRenderer questionRenderer;

    // �߰�: Title �ؽ�Ʈ�� ���� ����
    public Text titleText;

    public AdkScoreManager adkScoreManager;
    public RcbScoreManager rcbScoreManager;
    public CbsScoreManager cbsScoreManager;
    public SapsScoreManager sapsScoreManager;
    public YFASScoreManager yfasScoreManager;
    public FtnScoreManager ftnScoreManager;
    public HlsScoreManager hlsScoreManager;

    public void ButtonEvent()
    {
        UpdatePanel();
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        Debug.Log($"Button clicked: {clickBtn.name}");

        // 1. ���õ� ��ư�� �̸����� ���� ����
        csvReader.fileName = clickBtn.name;
        csvReader.SetFiles();

        // 2. ScoreManager �Ҵ� - ��ư �̸��� ���� �ش��ϴ� ScoreManager�� �������� ����
        switch (clickBtn.name)
        {
            case "AUDIT":
                questionRenderer.scoreManager = adkScoreManager;
                titleText.text = "AUDIT-K"; // Title �ؽ�Ʈ ������Ʈ
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "RCBS":
                questionRenderer.scoreManager = rcbScoreManager;
                titleText.text = "RCBS"; // Title �ؽ�Ʈ ������Ʈ
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "CBS":
                questionRenderer.scoreManager = cbsScoreManager;
                titleText.text = "CBS"; // Title �ؽ�Ʈ ������Ʈ
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "SAPS":
                questionRenderer.scoreManager = sapsScoreManager;
                titleText.text = "SAPS"; // Title �ؽ�Ʈ ������Ʈ
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "YFAS":
                questionRenderer.scoreManager = yfasScoreManager;
                titleText.text = "YFAS"; // Title �ؽ�Ʈ ������Ʈ
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "FTND":
                questionRenderer.scoreManager = ftnScoreManager;
                titleText.text = "FTND"; // Title �ؽ�Ʈ ������Ʈ
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "HLS":
                questionRenderer.scoreManager = hlsScoreManager;
                titleText.text = "HLS"; // Title �ؽ�Ʈ ������Ʈ
                questionRenderer.isHLSMode = true; // HLS ��� Ȱ��ȭ
                break;
            default:
                Debug.LogWarning("Unknown survey type");
                break;
        }

        // 3. QuestionRenderer �ʱ�ȭ �� CSV ������ �ε� ����
        questionRenderer.ResetRenderer();
        questionRenderer.setCsvReader();

        // 4. SurveyPanel Ȱ��ȭ
        if (!questionRenderer.isHLSMode) {
            switcher.surveyPanel.SetActive(true);
        }
    }

    public void UpdatePanel()
    {
        switcher.ClearPanel();
        switcher.OnClickBack();
    }
}

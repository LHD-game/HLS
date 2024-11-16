using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using System.Collections;

public class ChooseSurvey : MonoBehaviour
{
    [Header("script")]
    public SurveyCsvReader csvReader;
    public SurveySwitcher switcher;
    public QuestionRenderer questionRenderer;

    [Space(5f)]
    // �߰�: Title �ؽ�Ʈ�� ���� ����
    [Header("OBJ")]
    public Text titleText;
    public Transform buttonparent;
    [SerializeField]
    private GameObject[] buttons;
    public GameObject WarningWin;

    [Space(5f)]
    [Header("ScoreManager")]
    public AdkScoreManager adkScoreManager;
    public RcbScoreManager rcbScoreManager;
    public CbsScoreManager cbsScoreManager;
    public SapsScoreManager sapsScoreManager;
    public YFASScoreManager yfasScoreManager;
    public FtnScoreManager ftnScoreManager;
    public HlsScoreManager hlsScoreManager;
    
      

    private void Start()
    {
        /*questionRenderer.setCsvReader();*/
        for (int i = 0; i < buttonparent.childCount; i++)
        {
            Debug.Log(i);
            buttons[i] = buttonparent.GetChild(i).gameObject;
        }

        CheckItDone();
    }
    async public void CheckItDone()//������ ������ ������ �ִ��� Ȯ��
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            GameObject checkImg = buttons[i].transform.Find("check").gameObject;
            Button btn = buttons[i].GetComponent<Button>();
            if (await CheckTodayTest(buttons[i].name))
            {
                checkImg.SetActive(true);
                // �Ϸ� �̺�Ʈ
                btn.onClick.AddListener(ItsDone);
            }
            else
            {
                checkImg.SetActive(false);
                //�˻��̺�Ʈ
                btn.onClick.AddListener(ButtonEvent);
            }
        }
    }

    private void ItsDone()
    {
        StartCoroutine(warningWinCtl());
    }

    public void ButtonEvent()
    {
        UpdatePanel();
        Debug.Log("ButtonEvent() called");

        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        //titleText.text = clickBtn.name;

        // SurveySwitcher�� ���� �г� Ȱ��ȭ
        switcher.OnSurveyButtonClicked(clickBtn.name);
        Debug.Log($"Button clicked: {clickBtn.name}");

        // 1. ���õ� ��ư�� �̸����� ���� ����
        csvReader.fileName = clickBtn.name;
        csvReader.SetFiles();

        questionRenderer.ResetRenderer(); // ���� �ʱ�ȭ �߰�
                                          // ������ �ε� �� ���� ������
        questionRenderer.setCsvReader(); // ������ �ε� ����

        // 2. ScoreManager �Ҵ� - ��ư �̸��� ���� �ش��ϴ� ScoreManager�� �������� ����
        switch (clickBtn.name)
        {
            case "AUDIT":
                questionRenderer.scoreManager = adkScoreManager;
                //questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                //questionRenderer.typeText.text = "���ڿ��ߵ�";
                break;
            case "RCBS":
                questionRenderer.scoreManager = rcbScoreManager;
                //questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                //questionRenderer.typeText.text = "�����ߵ� ����1";
                break;
            case "CBS":
                questionRenderer.scoreManager = cbsScoreManager;
                //questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                //questionRenderer.typeText.text = "�����ߵ� ����2";
                break;
            case "SAPS":
                questionRenderer.scoreManager = sapsScoreManager;
                //questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                //questionRenderer.typeText.text = "����Ʈ���ߵ�";
                break;
            case "YFAS":
                questionRenderer.scoreManager = yfasScoreManager;
                //questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                //questionRenderer.typeText.text = "�����ߵ�";
                break;
            case "FTND":
                questionRenderer.scoreManager = ftnScoreManager;
                //questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                //questionRenderer.typeText.text = "����ƾ�ߵ�";
                break;
            case "HLS":
                questionRenderer.scoreManager = hlsScoreManager;
                //questionRenderer.isHLSMode = true; // HLS ��� Ȱ��ȭ
                //questionRenderer.typeText.text = "HLS �ǰ�";
                break;
            default:
                Debug.LogWarning("Unknown survey type");
                break;
        }

        string testName = clickBtn.transform.Find("smallTxt").GetComponent<Text>().text;
        questionRenderer.typeText.text = $"{testName} \n�ڰ����� �׽�Ʈ ���";
    }

    IEnumerator warningWinCtl()
    {
        WarningWin.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        WarningWin.SetActive(false);

    }

    async private Task<bool> CheckTodayTest(string nowSurv)
    {
        if (await FireBase.SDataCheck(PlayerPrefs.GetString("UserID"), nowSurv, System.DateTime.Now.ToString("yy MM dd")))
        {
            return true;
        }
        else
            return false;
    }

    public void UpdatePanel()
    {
        questionRenderer.setCsvReader(); // �ʱ�ȭ
        switcher.ClearPanel();
        switcher.OnClickBack();
    }
}

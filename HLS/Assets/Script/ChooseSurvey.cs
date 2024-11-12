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
    //public Text titleText;
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
        CheckItDone();
        /*for (int i = 0; i < buttonparent.childCount; i++)
        {
            Debug.Log(i);
            buttons[i] = buttonparent.GetChild(i).gameObject;
        }*/
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
                // ���� ��� Ŭ�� �̺�Ʈ ����
                btn.onClick.RemoveAllListeners();

                // �� Ŭ�� �̺�Ʈ �߰�
                btn.onClick.AddListener(ItsDone);
            }
            else
                checkImg.SetActive(false);
        }
    }

    private void ItsDone()
    {
        StartCoroutine(warningWinCtl());
    }

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
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "RCBS":
                questionRenderer.scoreManager = rcbScoreManager;
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "CBS":
                questionRenderer.scoreManager = cbsScoreManager;
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "SAPS":
                questionRenderer.scoreManager = sapsScoreManager;
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "YFAS":
                questionRenderer.scoreManager = yfasScoreManager;
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "FTND":
                questionRenderer.scoreManager = ftnScoreManager;
                questionRenderer.isHLSMode = false; // HLS ��� ��Ȱ��ȭ
                break;
            case "HLS":
                questionRenderer.scoreManager = hlsScoreManager;
                questionRenderer.isHLSMode = true; // HLS ��� Ȱ��ȭ
                break;
            default:
                Debug.LogWarning("Unknown survey type");
                break;
        }

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
        switcher.ClearPanel();
        switcher.OnClickBack();
    }
}

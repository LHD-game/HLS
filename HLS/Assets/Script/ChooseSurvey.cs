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
    // 추가: Title 텍스트를 위한 변수
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
    async public void CheckItDone()//서버에 오늘자 데이터 있는지 확인
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            GameObject checkImg = buttons[i].transform.Find("check").gameObject;
            Button btn = buttons[i].GetComponent<Button>();
            if (await CheckTodayTest(buttons[i].name))
            {
                checkImg.SetActive(true);
                // 완료 이벤트
                btn.onClick.AddListener(ItsDone);
            }
            else
            {
                checkImg.SetActive(false);
                //검사이벤트
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

        // SurveySwitcher를 통해 패널 활성화
        switcher.OnSurveyButtonClicked(clickBtn.name);
        Debug.Log($"Button clicked: {clickBtn.name}");

        // 1. 선택된 버튼의 이름으로 파일 설정
        csvReader.fileName = clickBtn.name;
        csvReader.SetFiles();

        questionRenderer.ResetRenderer(); // 상태 초기화 추가
                                          // 데이터 로드 및 질문 렌더링
        questionRenderer.setCsvReader(); // 데이터 로드 시작

        // 2. ScoreManager 할당 - 버튼 이름에 따라 해당하는 ScoreManager를 동적으로 설정
        switch (clickBtn.name)
        {
            case "AUDIT":
                questionRenderer.scoreManager = adkScoreManager;
                //questionRenderer.isHLSMode = false; // HLS 모드 비활성화
                //questionRenderer.typeText.text = "알코올중독";
                break;
            case "RCBS":
                questionRenderer.scoreManager = rcbScoreManager;
                //questionRenderer.isHLSMode = false; // HLS 모드 비활성화
                //questionRenderer.typeText.text = "쇼핑중독 유형1";
                break;
            case "CBS":
                questionRenderer.scoreManager = cbsScoreManager;
                //questionRenderer.isHLSMode = false; // HLS 모드 비활성화
                //questionRenderer.typeText.text = "쇼핑중독 유형2";
                break;
            case "SAPS":
                questionRenderer.scoreManager = sapsScoreManager;
                //questionRenderer.isHLSMode = false; // HLS 모드 비활성화
                //questionRenderer.typeText.text = "스마트폰중독";
                break;
            case "YFAS":
                questionRenderer.scoreManager = yfasScoreManager;
                //questionRenderer.isHLSMode = false; // HLS 모드 비활성화
                //questionRenderer.typeText.text = "과식중독";
                break;
            case "FTND":
                questionRenderer.scoreManager = ftnScoreManager;
                //questionRenderer.isHLSMode = false; // HLS 모드 비활성화
                //questionRenderer.typeText.text = "니코틴중독";
                break;
            case "HLS":
                questionRenderer.scoreManager = hlsScoreManager;
                //questionRenderer.isHLSMode = true; // HLS 모드 활성화
                //questionRenderer.typeText.text = "HLS 건강";
                break;
            default:
                Debug.LogWarning("Unknown survey type");
                break;
        }

        string testName = clickBtn.transform.Find("smallTxt").GetComponent<Text>().text;
        questionRenderer.typeText.text = $"{testName} \n자가진단 테스트 결과";
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
        questionRenderer.setCsvReader(); // 초기화
        switcher.ClearPanel();
        switcher.OnClickBack();
    }
}

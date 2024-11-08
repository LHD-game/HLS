using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseSurvey : MonoBehaviour
{
    public SurveyCsvReader csvReader;
    public SurveySwitcher switcher;
    public QuestionRenderer questionRenderer;

    // 추가: Title 텍스트를 위한 변수
    public Text titleText;

    public AdkScoreManager adkScoreManager;
    public RcbScoreManager rcbScoreManager;
    public CbsScoreManager cbsScoreManager;
    public SapsScoreManager sapsScoreManager;
    public YFASScoreManager yfasScoreManager;
    public FtnScoreManager ftnScoreManager;
    public HlsScoreManager hlsScoreManager;


    public void SetupHLSLayout()
    {
        // HLS 전용 레이아웃 변경 논리
        // 예시: 특정 UI 패널 활성화/비활성화, 레이아웃 설정 변경 등
        Debug.Log("Setting up HLS specific layout");
        // 필요한 레이아웃 조작을 여기에 추가
    }

    public void ButtonEvent()
    {
        UpdatePanel();
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        Debug.Log($"Button clicked: {clickBtn.name}");

        // 1. 선택된 버튼의 이름으로 파일 설정
        csvReader.fileName = clickBtn.name;
        csvReader.SetFiles();

        // 2. ScoreManager 할당 - 버튼 이름에 따라 해당하는 ScoreManager를 동적으로 설정
        switch (clickBtn.name)
        {
            case "AUDIT":
                questionRenderer.scoreManager = adkScoreManager;
                titleText.text = "AUDIT-K"; // Title 텍스트 업데이트
                break;
            case "RCBS":
                questionRenderer.scoreManager = rcbScoreManager;
                titleText.text = "RCBS"; // Title 텍스트 업데이트
                break;
            case "CBS":
                questionRenderer.scoreManager = cbsScoreManager;
                titleText.text = "CBS"; // Title 텍스트 업데이트
                break;
            case "SAPS":
                questionRenderer.scoreManager = sapsScoreManager;
                titleText.text = "SAPS"; // Title 텍스트 업데이트
                break;
            case "YFAS":
                questionRenderer.scoreManager = yfasScoreManager;
                titleText.text = "YFAS"; // Title 텍스트 업데이트
                break;
            case "FTND":
                questionRenderer.scoreManager = ftnScoreManager;
                titleText.text = "FTND"; // Title 텍스트 업데이트
                break;
            case "HLS":
                questionRenderer.scoreManager = hlsScoreManager;
                titleText.text = "HLS"; // Title 텍스트 업데이트

                // HLS 전용 레이아웃 설정
                questionRenderer.SetupHLSLayout();
                break;
            default:
                Debug.LogWarning("Unknown survey type");
                break;
        }



        // 3. QuestionRenderer 초기화 후 CSV 데이터 로드 시작
        questionRenderer.ResetRenderer();
        questionRenderer.setCsvReader();

        // 4. SurveyPanel 활성화
        switcher.surveyPanel.SetActive(true);
    }

    public void UpdatePanel()
    {
        switcher.ClearPanel();
        switcher.OnClickBack();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using System;
using UnityEngine.EventSystems;

public class WinCtl : MonoBehaviour
{
    public static WinCtl Instance;
    [Header("scripts")]
    public graph graph;
    public ChooseSurvey choose;
    public WindowStack windowStack;
    public soluWinctl soluWinctl_;
    //public SurveySwitcher surveySwitcher;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    [Space(5f)]
    [Header("Windows")]
    //public GameObject WellcomeWin;
    public GameObject MainWin;
    public GameObject HelpWin;
    public GameObject Menu;
    public GameObject Loading;
    [Space(5f)]
    public GameObject ResolutWin;
    public GameObject HistoryWin;
    public GameObject DatailWin;
    public GameObject TestCompeletWin;
    public GameObject TCBtn;
    [Space(5f)]
    public GameObject SurveyWin;
    public GameObject PrintsurveyWin;
    public GameObject OtherTestResoult;
    [Space(5f)]
    public GameObject SolutionWin;
    public GameObject PrintSolutionWin;

    [Space(5f)]
    private GameObject WinCtl_;

    private bool coRisRunning = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        // stageVal.text = stage.ToString();
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                BackAction();
            }
        }
    }

    private IEnumerator WinSetting(GameObject nowWin, GameObject Withopen)
    {
        coRisRunning = true;
        Debug.Log($"Loadingopen {nowWin}");

        Loading.SetActive(true);


        yield return new WaitForSeconds(0.2f);
        WinCtl_.SetActive(false);
        WinCtl_ = nowWin;
        WinCtl_.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        coRisRunning = false;
        if (Withopen == null) ;
        else
            Withopen.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Debug.Log($"Loadingclose {nowWin}");
        Loading.SetActive(false);
    }

    public void GotoMainScence()
    {
        SceneManager.LoadScene("Main");
    }
    public void GotoMainWin()
    {
        windowStack.reset();
        StartCoroutine(WinSetting(MainWin, null));
    }
    /*public void GotoResolutWin()
    {
        windowStack.push(nameof(HistoryWin));
        StartCoroutine(WinSetting(ResolutWin));
        HistoryWin.SetActive(true);
    }*/
    public void GotoTCWin()
    {
        StartCoroutine(WinSetting(ResolutWin, null));
        TestCompeletWin.SetActive(true);
        HistoryWin.SetActive(true);
        DatailWin.SetActive(true);
        SurveyWin.SetActive(false);
        StartCoroutine(Waitco(ResolutWin));
    }

    private IEnumerator Waitco(GameObject Window)
    {
        Debug.Log("While");
        while (coRisRunning)
        {
            Debug.Log("While");
            yield return new WaitForSeconds(0.1f);
        }
        Window.SetActive(false);
        Window.SetActive(true);
    }

    public void GotoHistoryWin()  //검사 결과모음 창
    {
        windowStack.reset();
        windowStack.push(nameof(MainWin));
        StartCoroutine(WinSetting(HistoryWin, ResolutWin));
        graph.printTestResoult();
    }
    public void GotoSurveyWin()  //설문 선택 창
    {
        windowStack.reset();
        windowStack.push(nameof(MainWin));
        StartCoroutine(WinSetting(SurveyWin, null));
        PrintsurveyWin.SetActive(false);
        choose.CheckItDone();
    }
    public void GotoPrintsurveyWin() //설문출력화면
    {
        windowStack.push(nameof(SurveyWin));
        StartCoroutine(WinSetting(PrintsurveyWin, SurveyWin));
        StartCoroutine(Waitco(PrintsurveyWin));
    }
    public void GotoOtherTestResoult()
    {
        StartCoroutine(WinSetting(OtherTestResoult, SurveyWin));
    }
    public void GotoDatailWin()  //검사 결과 상세보기 창
    {
        graph.DetailP();
        windowStack.push(WinCtl_.name);
        StartCoroutine(WinSetting(DatailWin, ResolutWin));
        ResolutWin.SetActive(true);
    }
    public void GotoSolutionWin() //처방 선택 창
    {
        windowStack.reset();
        windowStack.push(nameof(MainWin));
        soluWinctl_.ResetWin();
        StartCoroutine(WinSetting(SolutionWin, null));
    }
    public void GotoPrintSolutionWin() //처방 출력 창
    {
        windowStack.push(nameof(SolutionWin));
        StartCoroutine(WinSetting(PrintSolutionWin, SolutionWin));
    }
    public void GotoHelpWin() //help창
    {
        windowStack.reset();
        windowStack.push(nameof(MainWin));
        StartCoroutine(WinSetting(HelpWin, null));
    }

    public IEnumerator appStart()  //화면 초기화
    {
        //yield return new WaitForSeconds(0.5f);
        //WellcomeWin.SetActive(false);
        //열고
        MainWin.SetActive(true);
        ResolutWin.SetActive(true);
        HistoryWin.SetActive(true);
        SurveyWin.SetActive(true);
        PrintsurveyWin.SetActive(true);
        DatailWin.SetActive(true);
        SolutionWin.SetActive(true);
        TestCompeletWin.SetActive(true);
        TCBtn.SetActive(true);
        HelpWin.SetActive(true);
        Menu.SetActive(true);
        PrintSolutionWin.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //닫기
        MainWin.SetActive(false);
        ResolutWin.SetActive(false);
        HistoryWin.SetActive(false);
        SurveyWin.SetActive(false);
        PrintsurveyWin.SetActive(false);
        DatailWin.SetActive(false);
        SolutionWin.SetActive(false);
        TestCompeletWin.SetActive(false);
        TCBtn.SetActive(false);
        HelpWin.SetActive(false);
        Menu.SetActive(false);
        PrintSolutionWin.SetActive(false);
        WinCtl_ = MainWin;
        StartCoroutine(WinSetting(MainWin, null));
    }

    public void BackAction()
    {
        string funcName = windowStack.pop();
        Type t = this.GetType();
        MethodInfo method = t.GetMethod("Goto" + funcName);
        if (method != null)
        {
            method.Invoke(this, null);
        }
    }

    bool isquit = false;
    public void GotoQuit()
    {
        if (isquit)
            isquit = false;
        else
            isquit = true;

        StartCoroutine(AppQuit());
    }

    IEnumerator AppQuit()
    {
        if (isquit)
        {
            AndroidToast.I.ShowToastMessage("뒤로가기 버튼을 한 번 더 누르시면 종료됩니다.");
            yield return new WaitForSeconds(2f);
            isquit = false;
        }
        else
        {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
            Application.Quit();
#endif
        }


    }


    public void setonoff(GameObject window)
    {
        if (window.activeSelf)
            window.SetActive(false);
        else
            window.SetActive(true);
    }

    public IEnumerator warningWinCtl(GameObject Window)
    {
        Window.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        Window.SetActive(false);

    }
}
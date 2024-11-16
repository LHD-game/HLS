using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCtl : MonoBehaviour
{
    public static WinCtl Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        /*if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    [Header("Windows")]
    //public GameObject WellcomeWin;
    public GameObject MainWin;
    public GameObject HistWin;
    public GameObject ResolutWin;
    public GameObject surveyWin;
    public GameObject DatailWin;
    public GameObject SolutionWin;
    public GameObject TCWin;
    public GameObject TCBtn;
    public GameObject SolutionDetailWin;
    public GameObject HelpWin;
    public GameObject Menu;
    public GameObject Loading;

    GameObject WinCtl_;

    private bool coRisRunning=false;
    // Start is called before the first frame update
    void Start()
    {
        Loading.SetActive(true);
        StartCoroutine(appStart());
    }

    private IEnumerator WinSetting(GameObject nowWin)
    {
        coRisRunning = true;
        Debug.Log("Loadingopen");
        Loading.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        WinCtl_.SetActive(false);
        WinCtl_ = nowWin;
        WinCtl_.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        coRisRunning = false;   
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Loadingclose");
        Loading.SetActive(false);
    }

    public void GotoMainScence()
    {
        SceneManager.LoadScene("Main");
    }
    public void GotoMainWin()
    {
        StartCoroutine(WinSetting(MainWin));
    }
    public void GotoResolutWin()
    {
        StartCoroutine(WinSetting(ResolutWin));
        HistWin.SetActive(true);
    }
    public void GotoTCWin()
    {
        StartCoroutine(WinSetting(ResolutWin));
        TCWin.SetActive(true);
        HistWin.SetActive(true);
        DatailWin.SetActive(true);
        StartCoroutine(Waitco(ResolutWin));
    }

    private IEnumerator Waitco(GameObject Window)
    {
        while (coRisRunning)
        {
            Debug.Log("While");
            yield return new WaitForSeconds(0.1f);
        }
        Window.SetActive(false);
        Window.SetActive(true);
    }

    public void GotoHistWin()
    {
        StartCoroutine(WinSetting(HistWin));
        StartCoroutine(Waitco(ResolutWin));
    }
    public void GotosurveyWin()
    {
        StartCoroutine(WinSetting(surveyWin));
    }
    public void GotoDatailWin()
    {
        StartCoroutine(WinSetting(DatailWin));
        ResolutWin.SetActive(true);
    }
    public void GotoSolutionWin()
    {
        StartCoroutine(WinSetting(SolutionWin));
    }
    public void GotoHelpWin()
    {
        StartCoroutine(WinSetting(HelpWin));
    }
    public void OpenSolutionWin()
    {
        SolutionDetailWin.SetActive(true);
    }

    private IEnumerator appStart()
    {
        yield return new WaitForSeconds(0.5f);
        //WellcomeWin.SetActive(false);
        MainWin.SetActive(false);
        ResolutWin.SetActive(false);
        HistWin.SetActive(false);
        surveyWin.SetActive(false);
        DatailWin.SetActive(false);
        SolutionWin.SetActive(false);
        TCWin.SetActive(false);
        TCBtn.SetActive(false);
        HelpWin.SetActive(false);
        Menu.SetActive(false);
        SolutionDetailWin.SetActive(false);
        WinCtl_ = MainWin;
        StartCoroutine(WinSetting(MainWin));
    }
}

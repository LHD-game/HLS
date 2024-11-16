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
    public GameObject Menu;
    public GameObject Loading;

    GameObject WinCtl_;
    // Start is called before the first frame update
    void Start()
    {
        Loading.SetActive(true);
        StartCoroutine(appStart());
    }

    private IEnumerator WinSetting(GameObject nowWin)
    {
        Loading.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        WinCtl_.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        WinCtl_ = nowWin;
        WinCtl_.SetActive(true);
        yield return new WaitForSeconds(0.3f);
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
        TCBtn.SetActive(true);
        HistWin.SetActive(true);
        DatailWin.SetActive(true);
    }
    public void GotoHistWin()
    {
        StartCoroutine(WinSetting(HistWin));
        ResolutWin.SetActive(true);
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
        Menu.SetActive(false);
        SolutionDetailWin.SetActive(false);
        WinCtl_ = MainWin;
        StartCoroutine(WinSetting(MainWin));
    }
}

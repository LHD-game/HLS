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
    public GameObject SolutionDetailWin;
    public GameObject Menu;
    public GameObject Loading;

    GameObject WinCtl_;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(appStart());
    }

    private void WinSetting(GameObject nowWin)
    {
        WinCtl_.SetActive(false);
        WinCtl_ = nowWin;
        WinCtl_.SetActive(true);
    }

    public void GotoMainScence()
    {
        SceneManager.LoadScene("Main");
    }
    public void GotoMainWin()
    {
        WinSetting(MainWin);
    }
    public void GotoResolutWin()
    {
        WinSetting(ResolutWin);
        HistWin.SetActive(true);
    }
    public void GotoTCWin()
    {
        WinSetting(ResolutWin);
        TCWin.SetActive(true);
        HistWin.SetActive(true);
        DatailWin.SetActive(true);
    }
    public void GotoHistWin()
    {
        WinSetting(HistWin);
        ResolutWin.SetActive(true);
    }
    public void GotosurveyWin()
    {
        WinSetting(surveyWin);
    }
    public void GotoDatailWin()
    {
        WinSetting(DatailWin);
        ResolutWin.SetActive(true);
    }
    public void GotoSolutionWin()
    {
        WinSetting(SolutionWin);
    }
    public void OpenSolutionWin()
    {
        SolutionDetailWin.SetActive(true);
    }

    private IEnumerator appStart()
    {
        Loading.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //WellcomeWin.SetActive(false);
        MainWin.SetActive(false);
        ResolutWin.SetActive(false);
        HistWin.SetActive(false);
        surveyWin.SetActive(false);
        DatailWin.SetActive(false);
        SolutionWin.SetActive(false);
        TCWin.SetActive(false);
        Menu.SetActive(false);
        SolutionDetailWin.SetActive(false);
        WinCtl_ = MainWin;
        WinSetting(MainWin);
        yield return new WaitForSeconds(0.5f);
        Loading.SetActive(false);
    }
}

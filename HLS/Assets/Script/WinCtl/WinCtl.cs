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
    public GameObject surveyWin;
    public GameObject DatailWin;
    public GameObject SolutionWin;
    public GameObject SolutionDetailWin;
    public GameObject Menu;

    GameObject WinCtl_; 
    // Start is called before the first frame update
    void Start()
    {
        ReSetWin();
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
    public void GotoHistWin()
    {
        WinSetting(HistWin);
    }
    public void GotosurveyWin()
    {
        WinSetting(surveyWin);
    }
    public void GotoDatailWin()
    {
        WinSetting(DatailWin);
    }
    public void GotoSolutionWin()
    {
        WinSetting(SolutionWin);
    }
    public void OpenSolutionWin()
    {
       SolutionDetailWin.SetActive(true);
    }

    private void ReSetWin()
    {
        //WellcomeWin.SetActive(false);
        MainWin.SetActive(false);
        HistWin.SetActive(false);
        surveyWin.SetActive(false);
        DatailWin.SetActive(false);
        SolutionWin.SetActive(false);
        Menu.SetActive(false);
        WinCtl_ = MainWin;
        WinSetting(MainWin);
    }
}

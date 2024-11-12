using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ResolutWinCtrl : MonoBehaviour
{
    [Header("UserSetting")]
    public Text Name;
    public Text Report;

    [Header("ResoultWinSetting")]
    public GameObject resoultWin;
    public GameObject HisInfo;
    public GameObject HisCalendar;
    public GameObject Hissolu;
    public GameObject gotohisbutton;
    public GameObject DetailInfo;

    [Header("script")]
    public graph graph;

    /*private void Start()
    {
        resoultWin.SetActive(false);
        setResolutWin("노란불");
    }*/
    public void setResolutWin(string Sign)
    {
        graph.inputData();
        WinCtl.Instance.GotoTCWin();
        resoultWin.SetActive(true);
        HisInfo.SetActive(false);
        HisCalendar.SetActive(false);
        Hissolu.SetActive(false);
        gotohisbutton.SetActive(true);
        DetailInfo.SetActive(false);
        Name.text = $"{PlayerPrefs.GetString("UserName")}님의 검진 결과";
        Report.text = $"<b><size=13>{PlayerPrefs.GetString("UserName")}님은 {Sign}이에요!</size></b>\n\n라이프 스타일의 개선을 위해서\n나에게 맞는 처방을 받아보세요!";
    }

    public void resolutGotoHisBtn()
    {
        WinCtl.Instance.GotoHistWin();
        WinCtl.Instance.DatailWin.SetActive(false);
        resetresolutWin();
    }

    public void resetresolutWin()
    {
        WinCtl.Instance.DatailWin.SetActive(false);
        resoultWin.SetActive(false);
        HisInfo.SetActive(true);
        HisCalendar.SetActive(true);
        Hissolu.SetActive(true);
        gotohisbutton.SetActive(false);
        DetailInfo.SetActive(true);
    }
}

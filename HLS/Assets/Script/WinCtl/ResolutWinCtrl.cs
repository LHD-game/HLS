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
    public GameObject DetailPrintBtn;

    [Header("script")]
    public graph graph;

    /*private void Start()
    {
        resoultWin.SetActive(false);
        setResolutWin("�����");
    }*/
    public void setResolutWin(string Sign)
    {
        setresolutWin();
        WinCtl.Instance.GotoTCWin();
        Name.text = $"{PlayerPrefs.GetString("UserName")}���� ���� ���";
        Report.text = $"<b><size=13>{PlayerPrefs.GetString("UserName")}���� {Sign}�̿���!</size></b>\n\n������ ��Ÿ���� ������ ���ؼ�\n������ �´� ó���� �޾ƺ�����!";

    }

    public void resolutGotoHisBtn()
    {
        resetresolutWin();
        WinCtl.Instance.DatailWin.SetActive(false);
        WinCtl.Instance.GotoHistoryWin();
    }

    public void resetresolutWin()
    {
        resoultWin.SetActive(false);
        HisInfo.SetActive(true);
        HisCalendar.SetActive(true);
        Hissolu.SetActive(true);
        gotohisbutton.SetActive(false);
        DetailInfo.SetActive(true);
        DetailPrintBtn.SetActive(true);
    }
    public void setresolutWin()
    {
        graph.inputData();
        resoultWin.SetActive(true);
        HisInfo.SetActive(false);
        HisCalendar.SetActive(false);
        Hissolu.SetActive(false);
        gotohisbutton.SetActive(true);
        DetailInfo.SetActive(false);
        DetailPrintBtn.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class HisWinCtrl : MonoBehaviour
{

    [Header("UserSetting")]
    public Text Name;

    public void setHisWin()
    {
        Name.text = $"{PlayerPrefs.GetString("UserName")}님의 검진 결과";
    }
}

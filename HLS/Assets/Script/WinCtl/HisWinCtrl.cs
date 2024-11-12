using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class HisWinCtrl : MonoBehaviour
{

    [Header("UserSetting")]
    public Text Name;

    private void Start()
    {
        setHisWin();
    }
    public void setHisWin()
    {
        Name.text = $"{PlayerPrefs.GetString("UserName")}님의 지난 결과를 확인 해보세요";
    }
}

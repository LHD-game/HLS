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
        Name.text = $"{PlayerPrefs.GetString("UserName")}���� ���� ����� Ȯ�� �غ�����";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTextCtrl : MonoBehaviour
{
    public Text smallTxt;


    private void Start()
    {
        smallTxt = transform.Find("smallTxt").GetComponent<Text>();
        EventTrigger eventTrigger = this.GetComponent<EventTrigger>();

        if (smallTxt == null)
        {
            Debug.LogError("smallTxt�� ������� �ʾҽ��ϴ�. �̸� �Ǵ� ��θ� Ȯ���ϼ���.");
        }

        EventTrigger.Entry Downentry = new EventTrigger.Entry();
        Downentry.eventID = EventTriggerType.PointerDown;
        Downentry.callback.AddListener((data) => { PointerDown(); });

        //�̺�Ʈ Ʈ���� - PointExit
        EventTrigger.Entry upentry = new EventTrigger.Entry();
        upentry.eventID = EventTriggerType.PointerUp;
        upentry.callback.AddListener((data) => { PointerUp(); });

        eventTrigger.triggers.Add(Downentry);
        eventTrigger.triggers.Add(upentry);
    }


    public void PointerDown()
    {
        smallTxt.color = new Color32(219,219,219,255);
        Debug.Log("down");
    }

    public void PointerUp()
    {
        smallTxt.color = new Color32(071,088,162,255);
        Debug.Log("up");
    }
}

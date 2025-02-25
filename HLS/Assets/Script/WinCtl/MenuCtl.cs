using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MenuCtl : MonoBehaviour
{
    public GameObject surSub;
    public GameObject hisSub;
    public GameObject solSub;

    private GameObject subCtl_;
    private void Start()
    {
        subCtl_ = surSub;
    }
    public void SubMenuSetting(GameObject nowSub)
    {
        GameObject Button = EventSystem.current.currentSelectedGameObject;

        //StartCoroutine(subCloseAnima(subCtl_.GetComponent<RectTransform>()));
        subCtl_.SetActive(false);
        subCtl_ = nowSub;
        RectTransform CtlSub = subCtl_.GetComponent<RectTransform>();
        int count = CtlSub.childCount;
        CtlSub.sizeDelta = new Vector2(CtlSub.rect.width, 25 * count);
        subCtl_.transform.parent.position = new Vector2(Button.transform.position.x-50, Button.transform.position.y);
        subCtl_.SetActive(true);
        //StartCoroutine(subOpenAnima(subCtl_.GetComponent<RectTransform>()));
    }

    IEnumerator subOpenAnima(RectTransform CtlSub)
    {
        int count = CtlSub.childCount;

        /*for(; CtlSub.rect.height < 25*count-1;)
        {
            CtlSub.sizeDelta = new Vector2(CtlSub.rect.width, CtlSub.rect.height + 1);
            Debug.Log(CtlSub.rect.height);
            yield return new WaitForSeconds(0.001f);
        }*/

        CtlSub.sizeDelta = new Vector2(CtlSub.rect.width, 25 * count);
        yield return 0;
    }
    IEnumerator subCloseAnima(RectTransform CtlSub)
    {
        int count = CtlSub.childCount;

        /*for(; CtlSub.rect.height<2 ;)
        {
            CtlSub.sizeDelta = new Vector2(CtlSub.rect.width, CtlSub.rect.height - 1);
            yield return new WaitForSeconds(0.01f);
        }*/

        CtlSub.sizeDelta = new Vector2(CtlSub.rect.width,1);
        yield return 0;
    }
}


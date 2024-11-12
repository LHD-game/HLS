using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class soluWinctl : MonoBehaviour
{
    public CSVRenderer csvRenderer;

    [Header("GridWin")]
    public Text Foryoutxt;
    public Text Name;
    public Text RecentScore;
    [Header("PrintWin")]
    public Image UpImg; // �� �� �̹���
    public Image Simbol; // �ɺ� �̹���
    public Text TitleTxt; // Ÿ��ƲText
    public Transform IconParent; // Icon �̹��� �θ�
    public GameObject Icon; // Icon �̹��� ������
    public Transform exParent;
    public GameObject exPrefab; // ���� ��� ������
    public Transform SoluParent;
    public GameObject SoluPrefab; // ���� ��� ������

    public GameObject Loading; // Loadingȭ��(�ӽ�)
    private int index = 0;

    /*----------------------ó�� Grid ȭ��-------------------------------------------------*/

    private void Start()
    {
        setSoluWin();
    }
    public void setSoluWin()
    {
        string name = PlayerPrefs.GetString("UserName");
        Name.text = name;
        RecentScore.text = PlayerPrefs.GetString("RecentScore");
        Foryoutxt.text = $"<color=#32438B>{name}���� ����</color> \n\n����ó��";
    }

    /*----------------------ó�� ��� ȭ��-------------------------------------------------*/
    public void FindTitleIndex()
    {
        ResetWin();
        string ButtonName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(ButtonName);
        for (int i = 0; i <= csvRenderer.SolutionData.Count; i++)
        {
            if (csvRenderer.SolutionData[i]["title"].ToString().Equals(ButtonName))
            {
                index = i;
                break;
            }
            else
            {
                continue;
            }
        }
        StartCoroutine(setSoluPrintWin());
    }
    /*public void setexWin()
    {
        ResetWin();
        setTitle();
        inputExplain();
        //inputButton();
    }*/

    public void SoluBackBtn(GameObject soluWin)
    {
        ResetWin();
        soluWin.SetActive(false);
    }
    IEnumerator setSoluPrintWin()
    {
        Loading.SetActive(true);
        WinCtl.Instance.OpenSolutionWin();
        yield return new WaitForFixedUpdate();
        setTopImg();
        setTitle();
        setSimbol();
        inputobjs();
        yield return new WaitForSeconds(0.5f);
        Loading.SetActive(false);

    }

    void setTitle()
    {
        string Title = csvRenderer.SolutionData[index]["title"].ToString();
        string TitleText = csvRenderer.SolutionData[index]["titletxt"].ToString().Replace("! ","!\n");
        TitleTxt.text = $"<size=20px><color=#32438B>{Title}</color></size>\n\n\"{TitleText}\"";
    }
    void inputobjs()
    {
        PrintExText("explain", exParent, exPrefab);
        PrintSoluText("solution", SoluParent, SoluPrefab);
        Inputicon("icon", IconParent, Icon);
    }

    void setSimbol()
    {
        string simbolName = csvRenderer.SolutionData[index]["simbol"].ToString();
        Simbol.sprite = Resources.Load($"sprite/SoluImg/SimbolImg/{simbolName}", typeof(Sprite)) as Sprite;
    }
    void setTopImg()
    {
        string upimgName = csvRenderer.SolutionData[index]["upimg"].ToString();
        UpImg.sprite = Resources.Load($"sprite/SoluImg/UpImg/{upimgName}", typeof(Sprite)) as Sprite;
    }

    /// <summary>
    /// 1. text������ ����
    /// 2. ������ text�� explain�ֱ�
    /// 3. ���� exp�� ���� ��� 1,2 �ݺ�
    /// 4. solu���� �Ȱ��� ����
    /// 5. icon�� ����ϰ� ��
    /// </summary>
    /// 
    public void PrintExText(string inputobj, Transform Parent, GameObject Prefab)
    {
        for(int i=1; i<20;i++)
        {
            string[] inputtext = csvRenderer.SolutionData[index][inputobj+i].ToString().Split('/');
            if(inputtext[0].Equals("X"))
            {
                break;
            }
            else
            {
                GameObject TextIns = Instantiate(Prefab, Parent); // �θ� ����
                Text Textt = TextIns.GetComponent<Text>();
                Textt.text = $"<size=13px><color=#32438B>{inputtext[0]}</color></size>\n{inputtext[1]}\n\n";

                TextIns.SetActive(false);
                TextIns.SetActive(true);
            }
        }
    }

    public void PrintSoluText(string inputobj, Transform Parent, GameObject Prefab)
    {
        for (int i = 1; i < 20; i++)
        {
            string[] inputtext = csvRenderer.SolutionData[index][inputobj + i].ToString().Split('/');
            if (inputtext[0].Equals("X"))
            {
                break;
            }
            else
            {
                GameObject TextIns = Instantiate(Prefab, Parent); // �θ� ����
                Transform commentBg = TextIns.transform.Find("commentbg");
                Text Number = commentBg.Find("Number").GetComponent<Text>();
                Text Title = commentBg.Find("commentTitle").GetComponent<Text>();
                Text Text = Title.gameObject.transform.Find("comment Txt").GetComponent<Text>();
                Number.text = i.ToString();
                Title.text = inputtext[0];
                Text.text = inputtext[1];

                TextIns.SetActive(false);
                TextIns.SetActive(true);
            }
        }
    }

    public void Inputicon(string inputobj, Transform Parent, GameObject Prefab)
    {
        for (int i = 1; i < 20; i++)
        {
            string[] IconTxt =  csvRenderer.SolutionData[index][inputobj + i].ToString().Split('/');
            //Debug.Log(IconTxt[0] + "<" + IconTxt[1]);
            if (IconTxt[0].Equals("X"))
            {
                break;
            }
            else
            {
                GameObject IconPrefab = Instantiate(Prefab, Parent); // �θ� ����
                Transform PicArt = IconPrefab.transform.Find("PicArt");
                PicArt.GetComponent<Image>().sprite = Resources.Load($"sprite/SoluImg/IconImg/{IconTxt[0]}", typeof(Sprite)) as Sprite;
                IconPrefab.GetComponentInChildren<Text>().text = IconTxt[1];


                IconPrefab.SetActive(false);
                IconPrefab.SetActive(true);
            }
        }
    }

    public void ResetWin()
    {
        foreach (Transform child in SoluParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in exParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in IconParent)
        {
            Destroy(child.gameObject);
        }
    }
}

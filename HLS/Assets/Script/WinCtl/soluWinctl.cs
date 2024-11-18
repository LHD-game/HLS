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
    public Image UpImg; // 윗 쪽 이미지
    public Image Simbol; // 심볼 이미지
    public Text TitleTxt; // 타이틀Text
    public Transform IconParent; // Icon 이미지 부모
    public GameObject Icon; // Icon 이미지 프리펩
    public Transform exParent;
    public GameObject exPrefab; // 설명 출력 프리팹
    public Transform SoluParent;
    public GameObject SoluPrefab; // 조언 출력 프리팹

    public GameObject Loading; // Loading화면(임시)
    private int index = 0;

    /*----------------------처방 Grid 화면-------------------------------------------------*/

    private void Start()
    {
        setSoluWin();
    }
    public void setSoluWin()
    {
        string name = PlayerPrefs.GetString("UserName");
        Name.text = name;
        RecentScore.text = PlayerPrefs.GetString("RecentScore");
        Foryoutxt.text = $"<color=#32438B>{name}님을 위한</color> \n\n맞춤처방";
    }

    /*----------------------처방 출력 화면-------------------------------------------------*/
    public void FindTitleIndex()
    {
        WinCtl.Instance.Loading.SetActive(true);
        string ButtonName = EventSystem.current.currentSelectedGameObject.name;

        ResetWin();
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

    public void SoluBackBtn(GameObject soluWin)
    {
        ResetWin();
        soluWin.SetActive(false);
    }
    private IEnumerator setSoluPrintWin()
    {
        WinCtl.Instance.PrintSolutionWin.SetActive(true);
        setTopImg();
        setTitle();
        setSimbol();
        inputobjs();
        yield return new WaitForSeconds(0.1f);
        WinCtl.Instance.GotoPrintSolutionWin();

    }

    void setTitle()
    {
        string Title = csvRenderer.SolutionData[index]["title"].ToString();
        string TitleText = csvRenderer.SolutionData[index]["titletxt"].ToString().Replace("! ","!\n");
        TitleTxt.text = $"<size=20px><color=#32438B>{Title}</color></size>\n\n\"{TitleText}\"";
        Debug.Log("settitle");
    }
    void inputobjs()
    {
        PrintExText("explain", exParent, exPrefab);
        PrintSoluText("solution", SoluParent, SoluPrefab);
        Inputicon("icon", IconParent, Icon);
        Debug.Log("setobj");
    }

    void setSimbol()
    {
        string simbolName = csvRenderer.SolutionData[index]["simbol"].ToString();
        Simbol.sprite = Resources.Load($"sprite/SoluImg/SimbolImg/{simbolName}", typeof(Sprite)) as Sprite;
        Debug.Log("setsim");
    }
    void setTopImg()
    {
        string upimgName = csvRenderer.SolutionData[index]["upimg"].ToString();
        UpImg.sprite = Resources.Load($"sprite/SoluImg/UpImg/{upimgName}", typeof(Sprite)) as Sprite;
        Debug.Log("settop");
    }

    /// <summary>
    /// 1. text프리펩 생성
    /// 2. 프리펩 text에 explain넣기
    /// 3. 아직 exp가 남은 경우 1,2 반복
    /// 4. solu에도 똑같이 적용
    /// 5. icon도 비슷하게 감
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
                GameObject TextIns = Instantiate(Prefab, Parent); // 부모 지정
                Text Textt = TextIns.GetComponent<Text>();
                Textt.text = $"<size=13px><color=#32438B>{inputtext[0]}</color></size>\n{inputtext[1]}\n\n";

                TextIns.SetActive(false);
                TextIns.SetActive(true);
            }
        }
        Debug.Log("setEx");
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
                GameObject TextIns = Instantiate(Prefab, Parent); // 부모 지정
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
        Debug.Log("setsolu");
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
                GameObject IconPrefab = Instantiate(Prefab, Parent); // 부모 지정
                Transform PicArt = IconPrefab.transform.Find("PicArt");
                PicArt.GetComponent<Image>().sprite = Resources.Load($"sprite/SoluImg/IconImg/{IconTxt[0]}", typeof(Sprite)) as Sprite;
                IconPrefab.GetComponentInChildren<Text>().text = IconTxt[1];


                IconPrefab.SetActive(false);
                IconPrefab.SetActive(true);
            }
        }
        Debug.Log("seticon");
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

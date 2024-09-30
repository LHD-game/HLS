using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class soluWinctl : MonoBehaviour
{
    public CSVRenderer csvRenderer;


    public GameObject Title; // 타이틀obj
    public GameObject Text; // 출력 프리팹
    public GameObject SoluButton; // 출력 프리팹
    private int index = 0;
    public void FindTitleIndex()
    {
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
            setexWin();
    }
    public void setexWin()
    {
        ResetWin();
        setTitle();
        inputExplain();
        inputButton();
    }

    public void setSoluWin()
    {
        ResetWin();
        setTitle();
        inputsolution();
    }

    void setTitle()
    {
        GameObject Titleobj = Instantiate(Title, parent); // 부모 지정
        Text Textt = Titleobj.GetComponent<Text>();
        Textt.text = csvRenderer.SolutionData[index]["title"].ToString();
    }
    void inputExplain()
    {
        PrintSoluText("explain");
    }
    void inputsolution()
    {

        PrintSoluText("solution");
    }

    void inputButton()
    {
        GameObject Button = Instantiate(SoluButton, parent); // 부모 지정
        Button btn = Button.GetComponentInChildren<Button>();
        btn.onClick.AddListener(setSoluWin);
    }
    /// <summary>
    /// 1. text프리펩 생성
    /// 2. 프리펩 text에 explain넣기
    /// 3. 아직 exp가 남은 경우 1,2 반복
    /// 4. solu에도 똑같이 적용
    /// </summary>
    /// 
    public Transform parent;
    public void PrintSoluText(string inputobj)
    {
        for(int i=1; i<20;i++)
        {
            string inputtext = csvRenderer.SolutionData[index][inputobj+i].ToString();
            if(inputtext.Equals("X"))
            {
                break;
            }
            else
            {
                GameObject TextIns = Instantiate(Text, parent); // 부모 지정
                Text Textt = TextIns.GetComponent<Text>();
                Textt.text = inputtext;
            }
        }
    }

    public void ResetWin()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}

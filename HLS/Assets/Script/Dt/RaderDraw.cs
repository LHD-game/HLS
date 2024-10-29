using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class RaderDraw : MonoBehaviour
{
    private const string Pattern = @"\\n";
    public ScoreData scuns;
    public RadarChart chart;
    public RadarChart chartBg;
    public ScoreManager scoreManager;
    //public int segments; // �׷����� ���׸�Ʈ �� data.Length
    public RectTransform WheelPrent;

    [Header("Bar")]
    public float radius; // ���� ������
    //public float animationSpeed = 1f; // �ִϸ��̼� �ӵ�
    public Slider slider; // �����̴� UI
    public Slider[] bars;
    public RectTransform BarPos;

    [Space(10f)]
    public GameObject Mysign;
    public Text scoreTxt;

    [Header("Data")]
    public string[] TitleTxts;
    [Space(5f)]
    public int[] data;
    [Space(5f)]
    public Text Date;
    public Text adviceTxt;
    [Space(5f)]
    public int Score;


    private void Start()
    {
        scuns = GameObject.FindGameObjectWithTag("ScoreData").GetComponent<ScoreData>();
        if (WheelPrent.rect.width > WheelPrent.rect.height)
            chart.Radius = WheelPrent.rect.height / 3f;
        else
            chart.Radius = WheelPrent.rect.width / 3f;

        chartBg.Radius = chart.Radius;
        //Debug.Log(chart.Radius);

        //Debug.Log(WheelPrent.rect.width + " , " + WheelPrent.rect.height);

        chart.Angle = 70;
        chartBg.Angle = chart.Angle;
        for (int i = 0; i < data.Length; i++)
        {
            chart.DataSource.AddGroup(TitleTxts[i]);
        }
        Debug.Log("Start");
    }

    public void buttonC(int index)
    {
        GetData(index);  //����
        DetailPrint(index);
        SetDate(index);
        CreateBars(index);
        WinCtl.Instance.GotoDatailWin();
    }

    void DetailPrint(int index)
    {
        Score = Convert.ToInt32(scuns.ScoreData_[index]["total"]);
        scoreTxt.text = Score.ToString();
    }

    void GetData(int index)
    {
        for (int i = 0; i < data.Length; i++) //segment -> header.Length-1
        {
            chart.DataSource.SetValue("MyScore", TitleTxts[i], Int32.Parse(scuns.ScoreData_[index][scuns.header[i+1]].ToString()));
        }
    }

    public void addData()
    {
        string[] data = new string[] { System.DateTime.Now.ToString("yy MM dd"), scoreManager.scores[0].ToString(), scoreManager.scores[1].ToString(), 
            scoreManager.scores[2].ToString(), scoreManager.scores[3].ToString(), scoreManager.scores[4].ToString(), scoreManager.scores[5].ToString(), 
            scoreManager.scores[6].ToString(), scoreManager.scores[7].ToString(), scoreManager.scores[8].ToString(),scoreManager.totalScore.ToString()};

        scuns.SetData(data,data[0]);

        Debug.Log("������ ����=" + scuns.ScoreData_.Count);

        buttonC(scuns.ScoreData_.Count-1);
    }

    void CreateBars(int index)
    {
        bars = new Slider[data.Length];

        float angleStep = 360f / data.Length;

        for (int i = 0; i < data.Length; i++)
        {
            float angle = (i * angleStep) -110;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle + 90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, BarPos.transform.position + barPosition, Quaternion.identity, BarPos.transform);
            bars[i] = bar;

            //���� ������ ����
            RectTransform thisRect = bar.GetComponent<RectTransform>();

            if (WheelPrent.rect.width > WheelPrent.parent.GetComponent<RectTransform>().rect.width)
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.height / 3), thisRect.rect.height);
            else
                thisRect.sizeDelta = new Vector2((WheelPrent.parent.GetComponent<RectTransform>().rect.width / 3), thisRect.rect.height);

            // ������ ȸ�� ����
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            //������ �� ����
            string LabelTxt = Regex.Replace(chart.DataSource.GetGroupName(i), Pattern, " ");
            bar.GetComponentInChildren<Text>().text = LabelTxt;//scuns.header[i + 1].ToString();

        }

        UpdateGraph();
    }

    void UpdateGraph()
    {
        for (int i = 0; i < data.Length; i++)
            if (Score < 92)
                Mysign.GetComponent<Image>().color = Color.red;
            else if (92 < Score && Score < 114)
                Mysign.GetComponent<Image>().color = Color.yellow;
            else if (114 < Score)
                Mysign.GetComponent<Image>().color = Color.green;
    }

    void SetDate(int index)
    {
        string date = scuns.ScoreData_[index]["date"].ToString();
        string[] dates = date.Split(" ");
        Date.text = $"{dates[1]}�� {dates[2]}��\n����Ʈ ���";
        adviceTxt.text = $"{PlayerPrefs.GetString("UserName")} ���� \n������ ��Ÿ�� ������\n<color=#32438B><size=15>{scuns.ScoreData_[index]["total"].ToString()}���̿���!</size></color>";
    }
}

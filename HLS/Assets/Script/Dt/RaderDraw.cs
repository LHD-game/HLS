using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RaderDraw : MonoBehaviour
{
    public ScoreData scuns;
    public RadarChart chart;
    public ScoreManager scoreManager;
    //public int segments; // 그래프의 세그먼트 수 data.Length
    public RectTransform WheelPrent;

    [Header("Bar")]
    public float radius; // 원의 반지름
    //public float animationSpeed = 1f; // 애니메이션 속도
    public Slider slider; // 슬라이더 UI
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
    [Space(5f)]
    public int Score;


    private void Start()
    {
        if (WheelPrent.parent.GetComponent<RectTransform>().rect.width > WheelPrent.rect.height)
            chart.Radius = WheelPrent.rect.height / 3f;
        else
            chart.Radius = WheelPrent.parent.GetComponent<RectTransform>().rect.width / 3f;

        chart.Angle = 10;
        for (int i = 0; i < data.Length; i++)
        {
            chart.DataSource.AddGroup(TitleTxts[i]);
        }
    }

    public void buttonC(int index)
    {
        GetData(index);
        DetailPrint(index);
        SetDate(scuns.ScoreData_[index]["date"].ToString());
        CreateBars();
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

        //Debug.Log("데이터 갯수=" + scuns.ScoreData_.Count);

        buttonC(scuns.ScoreData_.Count-1);
    }

    void CreateBars()
    {
        bars = new Slider[data.Length];

        float angleStep = 360f / data.Length;

        for (int i = 0; i < data.Length; i++)
        {
            float angle = (i * angleStep) - 90;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle + 90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, BarPos.transform.position + barPosition, Quaternion.identity, BarPos.transform);
            bars[i] = bar;

            //막대 사이즈 조절
            RectTransform thisRect = bar.GetComponent<RectTransform>();

            if (WheelPrent.rect.width > WheelPrent.parent.GetComponent<RectTransform>().rect.width)
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.height / 3), thisRect.rect.height);
            else
                thisRect.sizeDelta = new Vector2((WheelPrent.parent.GetComponent<RectTransform>().rect.width / 3), thisRect.rect.height);

            // 막대의 회전 조절
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);

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

    void SetDate(string date)
    {
        Date.text = date;
    }
}

using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RaderDraw : MonoBehaviour
{
    public ScoreData scuns;
    public RadarChart chart;
    public ScoreManager scoreManager;
    public int segments; // 그래프의 세그먼트 수
    public RectTransform WheelPrent;

    [Space(10f)]
    public GameObject Mysign;
    public Text scoreTxt;

    [Header("Data")]
    public string[] TitleTxts;
    [Space(5f)]
    public int[] data;
    [Space(5f)]
    public Text Date;
    //public string DateTxt;
    [Space(5f)]
    public int Score;


    private void Start()
    {
           segments = data.Length;
        if (WheelPrent.rect.width > WheelPrent.rect.height)
            chart.Radius = WheelPrent.rect.height / 3f;
        else
            chart.Radius = WheelPrent.rect.width / 3f;

        chart.Angle = 10;
        for (int i = 0; i < segments; i++)
        {
            chart.DataSource.AddGroup(TitleTxts[i]);
        }
        //buttonC(0);
        
    }

    public void buttonC(int index)
    {
        GetData(index);
        DetailPrint(index);
        SetDate(scuns.ScoreData_[index]["date"].ToString());
        WinCtl.Instance.GotoDatailWin();
    }

    void DetailPrint(int index)
    {
        Score = (int)scuns.ScoreData_[index]["total"];
        scoreTxt.text = Score.ToString();
    }

    void GetData(int index)
    {
        //Debug.Log("ind " + index);
        for (int i = 0; i < segments; i++) //segment -> header.Length-1
        {
            //Debug.Log(scuns.header[i] + scuns.ScoreData_[index][scuns.header[i]]);
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



    int plusScore()
    {
        int P = 0;
        for (int i = 0; i < data.Length; i++)
        {
            P += data[i];
        }
        return P;
    }

    void SetDate(string date)
    {
        Date.text = date;
    }
}

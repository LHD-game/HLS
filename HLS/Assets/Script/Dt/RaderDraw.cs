using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RaderDraw : MonoBehaviour
{
    public RadarChart chart;
    public ScoreManager scoreManager;
    public int segments; // �׷����� ���׸�Ʈ ��
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

    ScoreData sd;

    private void Start()
    {
        sd = ScoreData.Instance;
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
        buttonC(0);
        
    }

    public void buttonC(int index)
    {
        GetData(index);
        DetailPrint(index);
        SetDate(sd.ScoreData_[index]["date"].ToString());
    }

    void DetailPrint(int index)
    {
        Score = (int)sd.ScoreData_[index]["total"];
        scoreTxt.text = Score.ToString();
    }

    void GetData(int index)
    {
        Debug.Log("ind " + index);
        for (int i = 0; i < segments; i++) //segment -> header.Length-1
        {
            Debug.Log(sd.header[i] + sd.ScoreData_[index][sd.header[i]]);
            chart.DataSource.SetValue("MyScore", TitleTxts[i], Int32.Parse(sd.ScoreData_[index][sd.header[i+1]].ToString()));
        }
    }

    public void addData()
    {
        string[] data = new string[] { System.DateTime.Now.ToString("yyMMdd"), scoreManager.scores[0].ToString(), scoreManager.scores[1].ToString(), 
            scoreManager.scores[2].ToString(), scoreManager.scores[3].ToString(), scoreManager.scores[4].ToString(), scoreManager.scores[5].ToString(), 
            scoreManager.scores[6].ToString(), scoreManager.scores[7].ToString(), scoreManager.scores[8].ToString(),scoreManager.totalScore.ToString()};
        
        sd.SetData(data);

        Debug.Log("������ ����=" + sd.ScoreData_.Count);

        buttonC(sd.ScoreData_.Count-1);
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

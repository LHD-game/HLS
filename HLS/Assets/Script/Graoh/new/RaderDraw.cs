using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;

public class RaderDraw : MonoBehaviour
{
    public RadarChart chart;

    public int segments = 10; // 그래프의 세그먼트 수
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
    public string DateTxt;
    [Space(5f)]
    public int Score;


    public void buttonClick()
    {
        segments = data.Length;
        Score = plusScore();
        scoreTxt.text = Score.ToString();
        GetData();
    }

    void GetData()
    {
        chart.DataSource.HasCategory("My");
        if (WheelPrent.rect.width > WheelPrent.rect.height)
            chart.Radius = WheelPrent.rect.height / 3;
        else
            chart.Radius = WheelPrent.rect.width / 3;
         
        for (int i = 0; i < segments; i++)
        {
            chart.DataSource.SetValue("1", TitleTxts[i], data[i]);
        }
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
}

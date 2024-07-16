using System;
using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using static ChartAndGraph.GraphChartBase;
using Random = UnityEngine.Random;

public class graph : MonoBehaviour
{
    public GraphChart chart;

    public Text testDate;
    public Text testResult;
        
    void Start()
    {
        chart.DataSource.VerticalViewSize = 100;    // Vertical Axis의 값 설정

        chart.DataSource.StartBatch();
        
        chart.DataSource.ClearCategory("History");
        
        // 오늘 기준 3일 전, 0~100까지 랜덤 값 넣기
        chart.DataSource.AddPointToCategory("History", System.DateTime.Now + System.TimeSpan.FromDays(-3f), Random.Range(0, 101));
        chart.DataSource.AddPointToCategory("History", System.DateTime.Now + System.TimeSpan.FromDays(-2f), Random.Range(0, 101));
        chart.DataSource.AddPointToCategory("History", System.DateTime.Now + System.TimeSpan.FromDays(-1f), Random.Range(0, 101));
        chart.DataSource.AddPointToCategory("History", System.DateTime.Now + System.TimeSpan.FromDays(0f), Random.Range(0, 101));
        
        chart.DataSource.EndBatch();
    }

    public void OnPointClick(GraphEventArgs args)
    {
        testDate.text = "날짜 : " + args.XString;
        testResult.text = "점수 : " + args.YString;
    }

    void Update()
    {
        
    }
}

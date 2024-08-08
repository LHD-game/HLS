using System;
using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using static ChartAndGraph.GraphChartBase;
using ChartAndGraph.Axis;
using Random = UnityEngine.Random;

public class graph : MonoBehaviour
{
    public GraphChart chart;
    public RaderDraw raderDraw;

    public Text testDate;
    public Text testResult;

    public HorizontalAxis horiAxis;
    private List<Dictionary<string, object>> SD_;

    int index;

    void Start()
    {
        chart.DataSource.VerticalViewSize = 144;    // Vertical Axis의 값 설정
        chart.DataSource.GetPointCount("History");
        horiAxis = chart.GetComponent<HorizontalAxis>();
        horiAxis.MainDivisions.Total = chart.DataSource.GetPointCount("History");
    }

    public void inputData()  //데이터 가져와 넣기
    {
        List<Dictionary<string, object>> SD_ = ScoreData.Instance.ScoreData_; //데이터 list
        chart.DataSource.StartBatch();
        chart.DataSource.ClearCategory("History");
        string[] Header = ScoreData.Instance.header;
        //Debug.Log("데이터갯수=" + SD_.Count);
        for (int i = 0; i < SD_.Count; i++)
        {
            chart.DataSource.AddPointToCategory("History", Convert.ToDateTime(SD_[i]["date"]), (int)SD_[i]["total"]); //날짜, total값 가져오기
        }
        chart.DataSource.GetPointCount("History");
        chart.DataSource.EndBatch();
    }

    public void OnPointClick(GraphEventArgs args)
    {
        testDate.text = "날짜 : " + args.XString;
        testResult.text = "점수 : " + args.YString;
        index = args.Index; //리스트에서 특정 날짜 값 뺴는 용도
        Debug.Log("ind = "+index);
    }

    public void DetailP()
    {
        raderDraw.buttonC(index); //디테일창 출력
    }

    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using static ChartAndGraph.GraphChartBase;
using ChartAndGraph.Axis;
using Random = UnityEngine.Random;

public class graphV2 : MonoBehaviour
{
    public GraphChart chart;
    public RaderDraw raderDraw;
    public ScoreData ScoreData;

    public Text testDate;
    public Text testResult;

    public HorizontalAxis horiAxis;
    private List<Dictionary<string, object>> SD_;

    int index;

    void Start()
    {
        chart.DataSource.VerticalViewSize = 144;    // Vertical Axis의 값 설정
        horiAxis = chart.GetComponent<HorizontalAxis>();
        horiAxis.MainDivisions.Total = chart.DataSource.GetPointCount("History")-1;
        horiAxis.SubDivisions.Total = 1;
        // chart.CustomNumberFormat = (d, i) => {return d.ToString("00.00");}; //그래프 Number Format 수정
        chart.CustomDateTimeFormat = (date) => { return date.ToString("MM/dd"); }; //그래프 Date Format 수정
    }

    public void inputData()  //데이터 가져와 넣기
    {
        SD_ = ScoreData.ScoreData_; //데이터 list
        chart.Scrollable = false;
        chart.DataSource.StartBatch();
        chart.DataSource.ClearCategory("History");
        for (int i = 1; i < SD_.Count+1; i++)
        {
            DateTime Date = Convert.ToDateTime(SD_[SD_.Count - i]["date"]);
            int TotalData = Convert.ToInt32(SD_[SD_.Count - i]["total"]);
            // double dDate = Date.Month+(Date.Day/100.0);
            // int iDate = Date.Month*100+Date.Day;
            chart.DataSource.AddPointToCategory("History", Date, TotalData); //날짜, total값 가져오기
            if (i == 6) break;
        }
        // chart.ClearHorizontalCustomDivisions();
        // for (int i = 1; i < SD_.Count+1; i++)
        // {
        //     DateTime Date = Convert.ToDateTime(SD_[SD_.Count - i]["date"]);
        //     chart.AddHorizontalAxisDivision(ChartDateUtility.DateToValue(Date), true);
        // }
        Debug.Log("데이터갯수=" + chart.DataSource.GetPointCount("History"));
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
}
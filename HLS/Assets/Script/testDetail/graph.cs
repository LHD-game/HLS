using System;
using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using static ChartAndGraph.GraphChartBase;
using ChartAndGraph.Axis;
using Maything.UI.CalendarSchedulerUI;
using Random = UnityEngine.Random;

public class graph : MonoBehaviour
{
    public GraphChart chart;
    public RaderDraw raderDraw;
    public ScoreData ScoreData;
    public DemoCodeToSchedule schedule;

    public Text testDate;
    public Text testResult;

    public HorizontalAxis horiAxis;
    private List<Dictionary<string, object>> SD_;

    int index;
    
    void Start()
    {
        if (chart == null)
            return;
        
        chart.DataSource.VerticalViewSize = 144;    // Vertical Axis의 값 설정
        // chart.CustomDateTimeFormat = (date) => { return date.ToString("MM/dd"); }; //그래프 Date Format 수정
    }
    

    public void inputData()  //데이터 가져와 넣기
    {
        SD_ = GameObject.FindGameObjectWithTag("ScoreData").GetComponent<ScoreData>().ScoreData_; ; //데이터 list
        chart.DataSource.StartBatch();
        chart.DataSource.ClearCategory("History");
        DateTime[] eachDate = new DateTime[SD_.Count+1];
        int[] eachTotal = new int[SD_.Count+1];
        for (int i = 1; i < SD_.Count+1; i++)
        {
            DateTime Date = Convert.ToDateTime(SD_[SD_.Count - i]["date"]);
            eachDate[i-1] = Date;
            int TotalData = Convert.ToInt32(SD_[SD_.Count - i]["total"]);
            eachTotal[i-1] = TotalData;
            chart.DataSource.AddPointToCategory("History", Date, TotalData); //날짜, total값 가져오기
            if (i == 6) break;
        }
        Debug.Log("데이터갯수=" + chart.DataSource.GetPointCount("History"));
        chart.DataSource.EndBatch();
        WinCtl.Instance.GotoHistWin();
    }

    public void inputSchedule()
    {
        SD_ = ScoreData.ScoreData_; //데이터 list
        Debug.Log("개수:"+SD_.Count);
        for (int i = 1; i < SD_.Count+1; i++)
        {
            DateTime Date = Convert.ToDateTime(SD_[SD_.Count - i]["date"]);
            int TotalData = Convert.ToInt32(SD_[SD_.Count - i]["total"]);
            schedule.addSingleTestScore(Date, TotalData);
            Debug.Log("스케쥴"+i);
        }
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

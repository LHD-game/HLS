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
    public RaderDraw raderDraw;

    public Text testDate;
    public Text testResult;

    int index;

    void Start()
    {
        chart.DataSource.VerticalViewSize = 144;    // Vertical Axis의 값 설정

        //chart.DataSource.StartBatch();
        
        //chart.DataSource.ClearCategory("History");
        inputData(); //먼저안넣으면 그래프 출력 안됨.. 수정이 필요하지만 현재 머리가 돌아가지 않음으로 이후는 맡깁니다>_0
                    //이거 이제 될 것도 같?지만 현재 자러가야하는 상태이므로 맡깁니다.. >_0
        // 오늘 기준 3일 전, 0~100까지 랜덤 값 넣기
        /*chart.DataSource.AddPointToCategory("History", System.DateTime.Now + System.TimeSpan.FromDays(-3f), Random.Range(0, 101));
        chart.DataSource.AddPointToCategory("History", System.DateTime.Now + System.TimeSpan.FromDays(-2f), Random.Range(0, 101));
        chart.DataSource.AddPointToCategory("History", System.DateTime.Now + System.TimeSpan.FromDays(-1f), Random.Range(0, 101));
        chart.DataSource.AddPointToCategory("History", System.DateTime.Now + System.TimeSpan.FromDays(0f), Random.Range(0, 101));*/
        chart.DataSource.GetPointCount("History");
        //chart.DataSource.EndBatch();
    }

    public void inputData()  //데이터 가져와 넣기
    {
        chart.DataSource.StartBatch();
        chart.DataSource.ClearCategory("History");
        List<Dictionary<string, object>> SD_ = ScoreData.Instance.ScoreData_; //데이터 list
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

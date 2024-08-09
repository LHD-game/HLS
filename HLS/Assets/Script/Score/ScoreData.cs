using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ScoreData : MonoBehaviour
{
    public string id;

    void Awake()
    {
        SetList();
        testData();
    }

    public List<Dictionary<string, object>> ScoreData_ = new List<Dictionary<string, object>>();
    string Headers = "date,sunlight,water,air,rest,exercise,nutrition,temperance,trust,gpc,total"; //11
    public string[] header;
    private void Start()
    {
    }

    void testData()
    {
        for(int i=0;i<9;i++)
        {
            string[] data = new string[] { System.DateTime.Now.AddDays(-10 + i).ToString("yyMMdd"), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), };

            SetData(data);
        }
    }

    public void GetData(int index,int[] data_) //헤더삽입
    {
        for (var i = 0; i < header.Length - 1; i++)
        {
            data_[i]= (int)ScoreData_[index][header[i + 1]];
}
    }
    public void SetList() //헤더설정
    {
        header = Regex.Split(Headers, ",");
        /*foreach (string i in header)
            Debug.Log(i);*/
    }
    public void SetData(string[] Data) //데이터 삽입
    {
        int totalScore=0;

        var entry = new Dictionary<string, object>();

        Debug.Log("헤더길이=" + header.Length);
        Debug.Log("데이터길이=" + Data.Length);
        for (var j = 0; j < header.Length-1; j++)
        {
            Debug.Log("j="+j+ "data=" + Data[j]);
            string value = Data[j];
            if(j==0)
            {
                string year = value.ToString().Substring(0, 2); // 24
                string month = value.ToString().Substring(2, 2); // MM
                string day = value.ToString().Substring(4, 2); // dd
                value = //year + " " +
                        month + " " + day;  //중간에 공백 없으면 날짜 변환 불가..
            }
            else if (j > 0)
            { 
                totalScore += Int32.Parse(value);
                Debug.Log("total = " + totalScore);
            }/*
            if (j == header.Length-1)
                value = totalScore.ToString();*/
            Debug.Log(header[j] + "= " + value);
            entry[header[j]] = value;

        }
        entry[header[10]] = totalScore;
        Debug.Log("전체합=" + totalScore);

        ScoreData_.Add(entry);
    }
}


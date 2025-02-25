using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Firebase.Firestore;
using System.Collections;
using System.Threading.Tasks;

public class ScoreData : MonoBehaviour
{
    public static ScoreData Instance;
    void Awake()
    {
        
        WinCtl.Instance.Loading.SetActive(true);
        Set();
    }

    //유저 아이디
    public string id;
    //유저 데이터
    public List<Dictionary<string, object>> ScoreData_;

    //테이터 키
    string Headers = "sunlight,water,air,rest,exercise,nutrition,temperance,trust,gpc,total,date"; //11
    string otherHeaders = "total,date"; //11
    public string[] header;
    public string[] otherheader;


    public GameObject WarningWin;
    
    async public void Set() //초기세팅
    {
        ScoreData_ = new List<Dictionary<string, object>>();
        SetList();
        id = PlayerPrefs.GetString("UserID");

        await SetData();

        StartCoroutine(WinCtl.Instance.appStart());
    }

    public void GetData(int index,int[] data_) //그래프에 헤더삽입
    {
        for (var i = 0; i < header.Length; i++)
        {
            data_[i]= Convert.ToInt32(ScoreData_[index][header[i + 1]]);
}
    }
    public void SetList() //리스트 헤더설정
    {
        header = Regex.Split(Headers, ",");
        otherheader = Regex.Split(otherHeaders, ",");
        //Debug.Log("헤더길이=" + header.Length);
    }

    //데이터로드& 앱에 삽입
    async public Task SetData() 
    {
        ScoreData_ = await FireBase.Dataload("HLS", id);

        WinCtl.Instance.Loading.SetActive(true);
    }

    //데이터 서버 업로드
    async public Task addData(string surveyType, Dictionary<string, object> Data, string[] header) 
    {
        var entry = new Dictionary<string, object>();

        var Header = new List<string>();
        /*for (var j = 0; j < header.Length; j++)
        {
            Header.Add = Data[j,0];
        }*/

        //Debug.Log("헤더길이=" + header.Length);
        for (var j = 0; j < header.Length; j++)
        {
            entry[header[j]] = Data[header[j]];
        }
        ScoreData_.Add(entry);

        await FireBase.DataUpload(id, header, surveyType, entry);
        await SetData();
    }
}


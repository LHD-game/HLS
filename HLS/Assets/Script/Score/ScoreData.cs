using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Firebase.Firestore;
using System.Collections;

public class ScoreData : MonoBehaviour
{
    public static ScoreData Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        var obj = GameObject.FindGameObjectsWithTag("ScoreData");
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Set();
    }

    public string id;
    //string surveyType = "HLS";

    public List<Dictionary<string, object>> ScoreData_;
    string Headers = "sunlight,water,air,rest,exercise,nutrition,temperance,trust,gpc,total,date"; //11
    string otherHeaders = "total,date"; //11
    public string[] header;
    public string[] otherheader;


    public GameObject WarningWin;
    
    public void Set()
    {
        ScoreData_ = new List<Dictionary<string, object>>();
        SetList();
        id = PlayerPrefs.GetString("UserID");
        Dataload("HLS", id);
        //Debug.Log("ScoreData" + ScoreData_.Count);
    }

    public void GetData(int index,int[] data_) //헤더삽입
    {
        for (var i = 0; i < header.Length; i++)
        {
            data_[i]= Convert.ToInt32(ScoreData_[index][header[i + 1]]);
}
    }
    public void SetList() //헤더설정
    {
        header = Regex.Split(Headers, ",");
        otherheader = Regex.Split(otherHeaders, ",");
        //Debug.Log("헤더길이=" + header.Length);
    }
    public void SetData(Dictionary<string, string> Data, string Date, string surveyType) //데이터 삽입
    {
        var entry = new Dictionary<string, object>();

        //Debug.Log("헤더길이=" + header.Length);
        for (var j = 0; j < header.Length; j++)
        {
            entry[header[j]] = Data[header[j]];
        }
        ScoreData_.Add(entry);

        DataUpload(surveyType,entry);
    }

    /*async public void testCheck()
    {

        string today = System.DateTime.Now.ToString("yy MM dd");
        
        if (await FireBase.SDataCheck(id, surveyType, today))
        {
            StartCoroutine(warningWinCtl());
            Debug.Log("이미 설문 진행 함");
        }
        else
        {
            WinCtl.Instance.GotosurveyWin(); //설문창이동
        }
    }

    IEnumerator warningWinCtl()
    {
        WarningWin.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        WarningWin.SetActive(false);

    }*/

    //|-----------------------서버에서 데이터를 저장하는 과정----------------------|
    async public void DataUpload(string surveyType,Dictionary<string,object> Data)
    {
        int dataLenth = Data.Count;
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        //CollectionReference userRef = db.Collection("surveyData");
        //파이어베이스에서 데이터 로드
        for (int i = 0; i < dataLenth; i++)
        {
            if (surveyType == "HLS")
            {
                await FireBase.ScoreDataSave(surveyType, id, header[i],
                                           Data[header[i]].ToString(),
                                           Data["date"].ToString());
            }
            else
            {
                await FireBase.ScoreDataSave(surveyType, id, otherheader[i],
                                             Data[otherheader[i]].ToString(),
                                             Data["date"].ToString());
            }
        }
    }
    //|-------------------------------------------------------------------------|
    //|-----------------------서버에서 데이터를 로드하는 과정----------------------|
    async public void Dataload(string surtype, string UserID)
    {
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query allData = db.Collection("user").Document(UserID)
                                    .Collection(surtype);
        QuerySnapshot allDataSnapshot = await allData.GetSnapshotAsync();
        
        foreach (DocumentSnapshot documentSnapshot in allDataSnapshot.Documents)
        {
            //파이어베이스에서 데이터 로드
            ScoreData_.Add(await FireBase.ScoreDataLoad(documentSnapshot, surtype, id));
        }

        //Debug.Log("FireBaseLoad" + ScoreData_.Count);
    }
    //|-------------------------------------------------------------------------|
}


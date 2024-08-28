using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Firebase.Firestore;
using System.Collections;

public class ScoreData : MonoBehaviour
{
    public string id;
    public string surveyType = "healthydata";


    public List<Dictionary<string, object>> ScoreData_ = new List<Dictionary<string, object>>();
    string Headers = "date,sunlight,water,air,rest,exercise,nutrition,temperance,trust,gpc,total"; //11
    public string[] header;


    public GameObject WarningWin;

    public void LoginSet()
    {
        SetList();
        Dataload(surveyType, id);
    }
    /*void testData()
    {
        for (int i = 0; i < 3; i++)
        {
            string[] data = new string[] { System.DateTime.Now.AddDays(-10 + i).ToString("yy MM dd"), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), UnityEngine.Random.Range(4, 17).ToString(), };

            SetData(data,data[0]);
        }
    }*/

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
    }
    public void SetData(string[] Data, string Date) //데이터 삽입
    {
        var entry = new Dictionary<string, object>();

        Debug.Log("헤더길이=" + header.Length);
        for (var j = 0; j < header.Length; j++)
        {
            string value = Data[j]; //await FireBase.ScoreDataLoad(surveyType, id, header[j]); //
            if (header[j] == "date")
            {
                Debug.Log(value);
            }
            Debug.Log(header[j] + "= " + value);
            entry[header[j]] = value;

        }
        ScoreData_.Add(entry);

        DataUpload();
    }

    async public void testCheck()
    {

        string today = System.DateTime.Now.ToString("yy MM dd");
        //await는 비동기 값을 동기화할때까지 기다려 달라는 뜻.
        //Data값 = FireBase.DataLoad([유저 ID], [Key값]) | Dictionary형 자료임으로 Key(string)를 통해 Data(string)를 찾음
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

    }

    //|-----------------------서버에서 데이터를 저장하는 과정----------------------|
    async public void DataUpload()
    {
        int dataLenth = header.Length;
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        //CollectionReference userRef = db.Collection("surveyData");
        //파이어베이스에서 데이터 로드
        for (int i = 0; i < dataLenth; i++)
        {
            await FireBase.ScoreDataSave(surveyType, id, header[i],
                                         ScoreData_[ScoreData_.Count - 1][header[i]].ToString(), 
                                         ScoreData_[ScoreData_.Count - 1]["date"].ToString());
        }
    }
    //|-------------------------------------------------------------------------|
    //|-----------------------서버에서 데이터를 로드하는 과정----------------------|
    async public void Dataload(string surType, string UserID)
    {
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query allData = db.Collection("user").Document(UserID)
                                    .Collection(surType);
        QuerySnapshot allDataSnapshot = await allData.GetSnapshotAsync();

        foreach (DocumentSnapshot documentSnapshot in allDataSnapshot.Documents)
        {
            //파이어베이스에서 데이터 로드
            ScoreData_.Add(await FireBase.ScoreDataLoad(documentSnapshot, surveyType, id));
        }

    }
    //|-------------------------------------------------------------------------|
}


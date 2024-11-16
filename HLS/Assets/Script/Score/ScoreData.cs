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

    public void GetData(int index,int[] data_) //�������
    {
        for (var i = 0; i < header.Length; i++)
        {
            data_[i]= Convert.ToInt32(ScoreData_[index][header[i + 1]]);
}
    }
    public void SetList() //�������
    {
        header = Regex.Split(Headers, ",");
        otherheader = Regex.Split(otherHeaders, ",");
        //Debug.Log("�������=" + header.Length);
    }
    public void SetData(Dictionary<string, string> Data, string Date, string surveyType) //������ ����
    {
        var entry = new Dictionary<string, object>();

        //Debug.Log("�������=" + header.Length);
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
            Debug.Log("�̹� ���� ���� ��");
        }
        else
        {
            WinCtl.Instance.GotosurveyWin(); //����â�̵�
        }
    }

    IEnumerator warningWinCtl()
    {
        WarningWin.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        WarningWin.SetActive(false);

    }*/

    //|-----------------------�������� �����͸� �����ϴ� ����----------------------|
    async public void DataUpload(string surveyType,Dictionary<string,object> Data)
    {
        int dataLenth = Data.Count;
        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        //CollectionReference userRef = db.Collection("surveyData");
        //���̾�̽����� ������ �ε�
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
    //|-----------------------�������� �����͸� �ε��ϴ� ����----------------------|
    async public void Dataload(string surtype, string UserID)
    {
        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query allData = db.Collection("user").Document(UserID)
                                    .Collection(surtype);
        QuerySnapshot allDataSnapshot = await allData.GetSnapshotAsync();
        
        foreach (DocumentSnapshot documentSnapshot in allDataSnapshot.Documents)
        {
            //���̾�̽����� ������ �ε�
            ScoreData_.Add(await FireBase.ScoreDataLoad(documentSnapshot, surtype, id));
        }

        //Debug.Log("FireBaseLoad" + ScoreData_.Count);
    }
    //|-------------------------------------------------------------------------|
}


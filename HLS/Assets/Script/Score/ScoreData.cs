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

    //���� ���̵�
    public string id;
    //���� ������
    public List<Dictionary<string, object>> ScoreData_;

    //������ Ű
    string Headers = "sunlight,water,air,rest,exercise,nutrition,temperance,trust,gpc,total,date"; //11
    string otherHeaders = "total,date"; //11
    public string[] header;
    public string[] otherheader;


    public GameObject WarningWin;
    
    async public void Set() //�ʱ⼼��
    {
        ScoreData_ = new List<Dictionary<string, object>>();
        SetList();
        id = PlayerPrefs.GetString("UserID");

        await SetData();

        StartCoroutine(WinCtl.Instance.appStart());
    }

    public void GetData(int index,int[] data_) //�׷����� �������
    {
        for (var i = 0; i < header.Length; i++)
        {
            data_[i]= Convert.ToInt32(ScoreData_[index][header[i + 1]]);
}
    }
    public void SetList() //����Ʈ �������
    {
        header = Regex.Split(Headers, ",");
        otherheader = Regex.Split(otherHeaders, ",");
        //Debug.Log("�������=" + header.Length);
    }

    //�����ͷε�& �ۿ� ����
    async public Task SetData() 
    {
        ScoreData_ = await FireBase.Dataload("HLS", id);

        WinCtl.Instance.Loading.SetActive(true);
    }

    //������ ���� ���ε�
    async public Task addData(string surveyType, Dictionary<string, object> Data, string[] header) 
    {
        var entry = new Dictionary<string, object>();

        var Header = new List<string>();
        /*for (var j = 0; j < header.Length; j++)
        {
            Header.Add = Data[j,0];
        }*/

        //Debug.Log("�������=" + header.Length);
        for (var j = 0; j < header.Length; j++)
        {
            entry[header[j]] = Data[header[j]];
        }
        ScoreData_.Add(entry);

        await FireBase.DataUpload(id, header, surveyType, entry);
        await SetData();
    }
}


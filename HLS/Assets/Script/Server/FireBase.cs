using System.Collections;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class FireBase : MonoBehaviour
{
    //���� ���� �κ�
    //------------------------------���� ���� �ҷ����� ex)SurveyDataLoad("AUDIT", "A1-1")
    async public static Task<string> SurveyDataLoad(string surveyId, string key)
    {                                   //���� ���� �ҷ����� surveyId = �������� �̸� | key = ����
        string data = "EMPTY";
        Dictionary<string, object> survey;

        //���̾�̽� �ʱ�ȭ
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //���̾�̽��� ���� ������ �ҷ��´�.
        DocumentReference docRef = db.Collection("SurveyList").Document(surveyId);
        await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            //�ش� ���� ���� ���� �Ǵ�
            if (snapshot.Exists)
            {
                //�������� ���� ����
                survey = snapshot.ToDictionary();
                data = survey[key].ToString();
                Debug.Log(data);
            }
            else
            {
                Debug.Log(String.Format("�������� ���� {0}�� �������� ����", snapshot.Id));
            }
        });
        return data;
    }
    //------------------------------������ �ҷ�����         ex)SurveyDataLoad("tzav...", "Name")
    async public static Task<string> DataLoad(string userId, string key)
    {                                   //������ �ҷ����� userId = ����Id(��ȣȭ��) | key = �׸�
        string data = "EMPTY";
        Dictionary<string, object> user;

        //���̾�̽� �ʱ�ȭ
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //���̾�̽��� ���� ������ �ҷ��´�.
        DocumentReference docRef = db.Collection("user").Document(userId);
        await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            //�ش� ���� ���� ���� �Ǵ�
            if (snapshot.Exists)
            {
                //�������� �׸� ����
                user = snapshot.ToDictionary();
                data = user[key].ToString();
                Debug.Log(data);
            }
            else
            {
                Debug.Log(String.Format("�׸� {0}�� �������� ����", snapshot.Id));
            }
        });
        return data;
    }
    //------------------------------������ �����ϱ�         ex)DataSave(uid, "Name", name);
    async public static Task DataSave(string UID, string Key, string Data)
    {                                   //������ �����ϱ�

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        CollectionReference userRef = db.Collection("user");
        if (await DataCheck(UID))
        {
            await userRef.Document(UID).UpdateAsync(new Dictionary<string, object>(){
            { Key, Data },     //Ű���� �����Ͱ�
        });
        }
        else
        {
            await userRef.Document(UID).SetAsync(new Dictionary<string, object>(){
            { Key, Data },     //Ű���� �����Ͱ�
        });
        }
    }



    async public static Task<Dictionary<string, object>> ScoreDataLoad(DocumentSnapshot documentSnapshot, string surType, string UserID)
    {
        var entry = new Dictionary<string, object>();
        //int totalScore = 0;
        Dictionary<string, object> ddata = documentSnapshot.ToDictionary();
        foreach (KeyValuePair<string, object> pair in ddata)
        {
            if (pair.Key.ToString() == "date") ;

            entry[pair.Key] = pair.Value.ToString();
        }
        //entry["total"] = totalScore;

        return entry;
    }
    //
    async public static Task ScoreDataSave(string surType ,string UserID, string Key, string Data,string Date)
    {                                   //������ �����ϱ�

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID);
        //Debug.Log(surType);
        if (await DataCheck(UserID)&& await SDataCheck(UserID, surType, Date))
        {
            await docRef.Collection(surType).Document(Date).UpdateAsync(new Dictionary<string, object>(){
                { Key, Data },     //Ű���� �����Ͱ�
            });
        }
        else
        {
            await docRef.Collection(surType).Document(Date).SetAsync(new Dictionary<string, object>(){
                { Key, Data },     //Ű���� �����Ͱ�
            });
        }
    }
    async public static Task<bool> DataCheck(string UserID)
    {                                   //������ ���� Ȯ���ϱ�
        //��ȣȭ �ؼ� �Է�
        //string En_UserID = AESEncrypt128(UserID);

        bool data = false;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        //Debug.Log(UserID);
        if (snapshot.Exists)
        {
            //Debug.Log("���� �˻� ����");
            data = true;
        }
        else
        {
            //Debug.Log("���� �˻� ����");
        }
        return data;
    }
    
    async public static Task<bool> SDataCheck(string UserID,string sur,string Date)
    {                                   //������ ���� Ȯ���ϱ�
        //��ȣȭ �ؼ� �Է�
        //string En_UserID = AESEncrypt128(UserID);

        bool data = false;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID).Collection(sur).Document(Date);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        //Debug.Log(UserID);
        if (snapshot.Exists)
        {
            //Debug.Log("��������");
            data = true;
        }
        else
        {
            //Debug.Log("����������");
        }
        return data;
    }

    //|-----------------------�������� �����͸� �����ϴ� ����----------------------|
    async public static Task DataUpload(string UserID,string[] Header ,string surveyType, Dictionary<string, object> Data)
    {
        int dataLenth = Data.Count;
        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        //CollectionReference userRef = db.Collection("surveyData");
        //���̾�̽����� ������ �ε�
        for (int i = 0; i < dataLenth; i++)
        {
                await FireBase.ScoreDataSave(surveyType, UserID, Header[i],
                                           Data[Header[i]].ToString(),
                                           Data["date"].ToString());
        }
    }
    //|-------------------------------------------------------------------------|
    //|-----------------------�������� �����͸� �ε��ϴ� ����----------------------|
    async public static Task<List<Dictionary<string, object>>> Dataload(string surtype, string UserID)
    {
        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query allData = db.Collection("user").Document(UserID).Collection(surtype);
        QuerySnapshot allDataSnapshot = await allData.GetSnapshotAsync();

        var dataList = new List<Dictionary<string, object>>();

        foreach (DocumentSnapshot documentSnapshot in allDataSnapshot.Documents)
        {
            //���̾�̽����� ������ �ε�
            dataList.Add(await ScoreDataLoad(documentSnapshot, surtype, UserID));
        }

        return dataList;

        //Debug.Log("FireBaseLoad" + ScoreData_.Count);
    }
    //|-------------------------------------------------------------------------|
}

using System.Collections;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class FireBase : MonoBehaviour
{
    //���� ���� �κ�
    async public static Task<string> DataLoad(string UserID, string Key)
    {                                   //������ �ҷ�����
        string data = "EMPTY";

        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //���̾�̽����� ������ �ε�(user �÷����� UserID�� ã��)
        DocumentReference docRef = db.Collection("user").Document(UserID);
        //ã�� ������ �ҷ��ͼ� ����

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists) //�α���
        {
            Debug.Log("���� �ҷ����� ����");
            Dictionary<string, object> ddata = snapshot.ToDictionary();
            //��ȣȭ �ؼ� �����Ϳ� �ֱ�
            Debug.Log(Key);
            data = ddata[Key].ToString();

        }
        else
        {
            Debug.Log("���� �ҷ����� ����");
            data = "ERROR";
        }
        return data;
    }
    async public static Task DataSave(string UserID, string Key, string Data)
    {                                   //������ �����ϱ�

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        CollectionReference userRef = db.Collection("user");
        if (snapshot.Exists) //�α���
        {
            if (await DataCheck(UserID))
            {
                await userRef.Document(UserID).UpdateAsync(new Dictionary<string, object>(){
                { Key, Data },     //Ű���� �����Ͱ�
            });
            }
            else
            {
                await userRef.Document(UserID).SetAsync(new Dictionary<string, object>(){
                { Key, Data },     //Ű���� �����Ͱ�
            });
            }

        }
        else
        {
            Debug.Log("���� �ҷ����� ����");
            SceneManager.LoadSceneAsync("LoginScene"); //mainȭ������
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
        Query allData = db.Collection("user").Document(UserID)
                                    .Collection(surtype);
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

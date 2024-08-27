using System.Collections;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;
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
        CollectionReference userRef = db.Collection("user");
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

    async public static Task<Dictionary<string, object>> ScoreDataLoad(DocumentSnapshot documentSnapshot, string surType, string UserID)
    {
        var entry = new Dictionary<string, object>();
        //int totalScore = 0;
        Dictionary<string, object> ddata = documentSnapshot.ToDictionary();
        foreach (KeyValuePair<string, object> pair in ddata)
        {
            if (pair.Key.ToString() == "date") ;
                //Debug.Log("��¥");
            //else
                //totalScore += Int32.Parse(pair.Value.ToString());

            //Debug.Log("key = " + pair.Key + "\ndata = " + pair.Value);

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
        Debug.Log(UserID);
        if (snapshot.Exists)
        {
            Debug.Log("���� �˻� ����");
            data = true;
        }
        else
        {
            Debug.Log("���� �˻� ����");
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
        Debug.Log(UserID);
        if (snapshot.Exists)
        {
            Debug.Log("���� �˻� ����");
            data = true;
        }
        else
        {
            Debug.Log("���� �˻� ����");
        }
        return data;
    }
}

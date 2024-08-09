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

    async public static Task<string> ScoreDataLoad(string surType, string UserID, string Key, string Data)
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
    //
    async public static Task ScoreDataSave(string surType ,string UserID, string Key, string Data)
    {                                   //������ �����ϱ�

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID);
        if (await DataCheck(UserID)&& await SDataCheck(UserID, surType))
        {
            await docRef.Collection("surveyData").Document(surType).UpdateAsync(new Dictionary<string, object>(){
                { Key, Data },     //Ű���� �����Ͱ�
            });
        }
        else
        {
            await docRef.Collection("surveyData").Document(surType).SetAsync(new Dictionary<string, object>(){
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
    
    async public static Task<bool> SDataCheck(string UserID,string sur)
    {                                   //������ ���� Ȯ���ϱ�
        //��ȣȭ �ؼ� �Է�
        //string En_UserID = AESEncrypt128(UserID);

        bool data = false;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID).Collection("surveyData").Document(sur);
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

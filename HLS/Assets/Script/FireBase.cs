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
        //��ȣȭ �ؼ� �Է�
        string En_UserID = AESEncrypt128(UserID);
        string En_Key = AESEncrypt128(Key);

        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //���̾�̽����� ������ �ε�(user �÷����� UserID�� ã��)
        DocumentReference docRef = db.Collection("user").Document(En_UserID);
        //ã�� ������ �ҷ��ͼ� ����

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Debug.Log("���� �ҷ����� ����");
            Dictionary<string, object> ddata = snapshot.ToDictionary();
            //��ȣȭ �ؼ� �����Ϳ� �ֱ�
            Debug.Log(En_Key);
            data = ddata[En_Key].ToString();
            data = AESDecrypt128(data);
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
        //��ȣȭ �ؼ� �Է�
        string En_UserID = AESEncrypt128(UserID);
        string En_Key = AESEncrypt128(Key);
        string En_Data = AESEncrypt128(Data);

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference userRef = db.Collection("user");
        if (await DataCheck(UserID))
        {
            await userRef.Document(En_UserID).UpdateAsync(new Dictionary<string, object>(){
                { En_Key, En_Data },     //Ű���� �����Ͱ�
            });
        }
        else
        {
            await userRef.Document(En_UserID).SetAsync(new Dictionary<string, object>(){
                { En_Key, En_Data },     //Ű���� �����Ͱ�
            });
        }
    }
    async public static Task<bool> DataCheck(string UserID)
    {                                   //������ ���� Ȯ���ϱ�
        //��ȣȭ �ؼ� �Է�
        string En_UserID = AESEncrypt128(UserID);

        bool data = false;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(En_UserID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        Debug.Log(En_UserID);
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

    //��ȣȭ �κ�

    //��ȣȭŰ ����
    private static readonly string PASSWORD = "MadeByHLSandRHDs";
    //����Ű ����
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);


    // ��ȣȭ
    private static string AESEncrypt128(string plain)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = myRijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();

        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    // ��ȣȭ
    private static string AESDecrypt128(string encrypt)
    {
        byte[] encryptBytes = Convert.FromBase64String(encrypt);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream(encryptBytes);

        ICryptoTransform decryptor = myRijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        memoryStream.Close();

        return plainString;
    }
}

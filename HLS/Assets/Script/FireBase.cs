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
    //서버 구현 부분
    async public static Task<string> DataLoad(string UserID, string Key)
    {                                   //데이터 불러오기
        string data = "EMPTY";
        //암호화 해서 입력
        string En_UserID = AESEncrypt128(UserID);
        string En_Key = AESEncrypt128(Key);

        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //파이어베이스에서 데이터 로드(user 컬렉션중 UserID를 찾음)
        DocumentReference docRef = db.Collection("user").Document(En_UserID);
        //찾은 정보를 불러와서 진행

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Debug.Log("유저 불러오기 성공");
            Dictionary<string, object> ddata = snapshot.ToDictionary();
            //복호화 해서 데이터에 넣기
            Debug.Log(En_Key);
            data = ddata[En_Key].ToString();
            data = AESDecrypt128(data);
        }
        else
        {
            Debug.Log("유저 불러오기 실패");
            data = "ERROR";
        }
        return data;
    }
    async public static Task DataSave(string UserID, string Key, string Data)
    {                                   //데이터 저장하기
        //암호화 해서 입력
        string En_UserID = AESEncrypt128(UserID);
        string En_Key = AESEncrypt128(Key);
        string En_Data = AESEncrypt128(Data);

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference userRef = db.Collection("user");
        if (await DataCheck(UserID))
        {
            await userRef.Document(En_UserID).UpdateAsync(new Dictionary<string, object>(){
                { En_Key, En_Data },     //키값과 데이터값
            });
        }
        else
        {
            await userRef.Document(En_UserID).SetAsync(new Dictionary<string, object>(){
                { En_Key, En_Data },     //키값과 데이터값
            });
        }
    }
    async public static Task<bool> DataCheck(string UserID)
    {                                   //데이터 유무 확인하기
        //암호화 해서 입력
        string En_UserID = AESEncrypt128(UserID);

        bool data = false;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(En_UserID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        Debug.Log(En_UserID);
        if (snapshot.Exists)
        {
            Debug.Log("유저 검색 성공");
            data = true;
        }
        else
        {
            Debug.Log("유저 검색 실패");
        }
        return data;
    }

    //암호화 부분

    //암호화키 생성
    private static readonly string PASSWORD = "MadeByHLSandRHDs";
    //인증키 정의
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);


    // 암호화
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

    // 복호화
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

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
    //서버 구현 부분
    async public static Task<string> DataLoad(string UserID, string Key)
    {                                   //데이터 불러오기
        string data = "EMPTY";

        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //파이어베이스에서 데이터 로드(user 컬렉션중 UserID를 찾음)
        DocumentReference docRef = db.Collection("user").Document(UserID);
        //찾은 정보를 불러와서 진행

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists) //로그인
        {
            Debug.Log("유저 불러오기 성공");
            Dictionary<string, object> ddata = snapshot.ToDictionary();
            //복호화 해서 데이터에 넣기
            Debug.Log(Key);
            data = ddata[Key].ToString();

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

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        CollectionReference userRef = db.Collection("user");
        if (snapshot.Exists) //로그인
        {
            if (await DataCheck(UserID))
            {
                await userRef.Document(UserID).UpdateAsync(new Dictionary<string, object>(){
                { Key, Data },     //키값과 데이터값
            });
            }
            else
            {
                await userRef.Document(UserID).SetAsync(new Dictionary<string, object>(){
                { Key, Data },     //키값과 데이터값
            });
            }

        }
        else
        {
            Debug.Log("유저 불러오기 실패");
            SceneManager.LoadSceneAsync("LoginScene"); //main화면으로
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
    {                                   //데이터 저장하기

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID);
        //Debug.Log(surType);
        if (await DataCheck(UserID)&& await SDataCheck(UserID, surType, Date))
        {
            await docRef.Collection(surType).Document(Date).UpdateAsync(new Dictionary<string, object>(){
                { Key, Data },     //키값과 데이터값
            });
        }
        else
        {
            await docRef.Collection(surType).Document(Date).SetAsync(new Dictionary<string, object>(){
                { Key, Data },     //키값과 데이터값
            });
        }
    }
    async public static Task<bool> DataCheck(string UserID)
    {                                   //데이터 유무 확인하기
        //암호화 해서 입력
        //string En_UserID = AESEncrypt128(UserID);

        bool data = false;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        //Debug.Log(UserID);
        if (snapshot.Exists)
        {
            //Debug.Log("유저 검색 성공");
            data = true;
        }
        else
        {
            //Debug.Log("유저 검색 실패");
        }
        return data;
    }
    
    async public static Task<bool> SDataCheck(string UserID,string sur,string Date)
    {                                   //데이터 유무 확인하기
        //암호화 해서 입력
        //string En_UserID = AESEncrypt128(UserID);

        bool data = false;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID).Collection(sur).Document(Date);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        //Debug.Log(UserID);
        if (snapshot.Exists)
        {
            //Debug.Log("설문존재");
            data = true;
        }
        else
        {
            //Debug.Log("설문비존재");
        }
        return data;
    }

    //|-----------------------서버에서 데이터를 저장하는 과정----------------------|
    async public static Task DataUpload(string UserID,string[] Header ,string surveyType, Dictionary<string, object> Data)
    {
        int dataLenth = Data.Count;
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        //CollectionReference userRef = db.Collection("surveyData");
        //파이어베이스에서 데이터 로드
        for (int i = 0; i < dataLenth; i++)
        {
                await FireBase.ScoreDataSave(surveyType, UserID, Header[i],
                                           Data[Header[i]].ToString(),
                                           Data["date"].ToString());
        }
    }
    //|-------------------------------------------------------------------------|
    //|-----------------------서버에서 데이터를 로드하는 과정----------------------|
    async public static Task<List<Dictionary<string, object>>> Dataload(string surtype, string UserID)
    {
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query allData = db.Collection("user").Document(UserID)
                                    .Collection(surtype);
        QuerySnapshot allDataSnapshot = await allData.GetSnapshotAsync();

        var dataList = new List<Dictionary<string, object>>();

        foreach (DocumentSnapshot documentSnapshot in allDataSnapshot.Documents)
        {
            //파이어베이스에서 데이터 로드
            dataList.Add(await ScoreDataLoad(documentSnapshot, surtype, UserID));
        }

        return dataList;

        //Debug.Log("FireBaseLoad" + ScoreData_.Count);
    }
    //|-------------------------------------------------------------------------|
}

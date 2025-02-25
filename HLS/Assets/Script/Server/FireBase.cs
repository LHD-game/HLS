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
    //서버 구현 부분
    //------------------------------설문 문항 불러오기 ex)SurveyDataLoad("AUDIT", "A1-1")
    async public static Task<string> SurveyDataLoad(string surveyId, string key)
    {                                   //설문 문항 불러오기 surveyId = 설문조사 이름 | key = 문항
        string data = "EMPTY";
        Dictionary<string, object> survey;

        //파이어베이스 초기화
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //파이어베이스에 설문 문항을 불러온다.
        DocumentReference docRef = db.Collection("SurveyList").Document(surveyId);
        await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            //해당 문서 존재 여부 판단
            if (snapshot.Exists)
            {
                //문서에서 문항 추출
                survey = snapshot.ToDictionary();
                data = survey[key].ToString();
                Debug.Log(data);
            }
            else
            {
                Debug.Log(String.Format("설문조사 문항 {0}이 존재하지 않음", snapshot.Id));
            }
        });
        return data;
    }
    //------------------------------데이터 불러오기         ex)SurveyDataLoad("tzav...", "Name")
    async public static Task<string> DataLoad(string userId, string key)
    {                                   //데이터 불러오기 userId = 유저Id(암호화된) | key = 항목
        string data = "EMPTY";
        Dictionary<string, object> user;

        //파이어베이스 초기화
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //파이어베이스에 유저 문서를 불러온다.
        DocumentReference docRef = db.Collection("user").Document(userId);
        await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            //해당 문서 존재 여부 판단
            if (snapshot.Exists)
            {
                //문서에서 항목 추출
                user = snapshot.ToDictionary();
                data = user[key].ToString();
                Debug.Log(data);
            }
            else
            {
                Debug.Log(String.Format("항목 {0}이 존재하지 않음", snapshot.Id));
            }
        });
        return data;
    }
    //------------------------------데이터 저장하기         ex)DataSave(uid, "Name", name);
    async public static Task DataSave(string UID, string Key, string Data)
    {                                   //데이터 저장하기

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        CollectionReference userRef = db.Collection("user");
        if (await DataCheck(UID))
        {
            await userRef.Document(UID).UpdateAsync(new Dictionary<string, object>(){
            { Key, Data },     //키값과 데이터값
        });
        }
        else
        {
            await userRef.Document(UID).SetAsync(new Dictionary<string, object>(){
            { Key, Data },     //키값과 데이터값
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
        Query allData = db.Collection("user").Document(UserID).Collection(surtype);
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

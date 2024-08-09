using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class SaveScoreData : MonoBehaviour
{
    public string surveyType = "healthydata";
    public ScoreData scuns;

    //|-----------------------서버에서 데이터를 저장하는 과정----------------------|
    async public void DataUpload()
    {

        int dataLenth = scuns.header.Length - 1;
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference userRef = db.Collection("surveyData");
        //파이어베이스에서 데이터 로드
        for (int i = 0; i < dataLenth; i++)
        {
            await FireBase.ScoreDataSave(surveyType,scuns.id, scuns.header[i], scuns.ScoreData_[scuns.ScoreData_.Count-1][scuns.header[i]].ToString());
        }

    }
    //|-------------------------------------------------------------------------|


}

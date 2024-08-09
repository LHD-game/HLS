using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

public class SaveScoreData : MonoBehaviour
{
    public string surveyType = "healthydata";
    public ScoreData scuns;

    //|-----------------------�������� �����͸� �����ϴ� ����----------------------|
    async public void DataUpload()
    {

        int dataLenth = scuns.header.Length - 1;
        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference userRef = db.Collection("surveyData");
        //���̾�̽����� ������ �ε�
        for (int i = 0; i < dataLenth; i++)
        {
            await FireBase.ScoreDataSave(surveyType,scuns.id, scuns.header[i], scuns.ScoreData_[scuns.ScoreData_.Count-1][scuns.header[i]].ToString());
        }

    }
    //|-------------------------------------------------------------------------|


}

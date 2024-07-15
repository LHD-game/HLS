using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;

public class FireBase : MonoBehaviour
{
    async public static Task<string> DataLoad(string UserID, string Key)
    {
        string data = "EMPTY";

        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //파이어베이스에서 데이터 로드(user 컬렉션중 UserID를 찾음)
        DocumentReference docRef = db.Collection("user").Document(UserID);
        //찾은 정보를 불러와서 진행

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Debug.Log("유저 검색 성공");
            Dictionary<string, object> ddata = snapshot.ToDictionary();
            data = ddata[Key].ToString();
        }
        else
        {
            Debug.Log("유저 검색 실패");
            data = "ERROR";
        }
        return data;
    }
    async public static Task DataSave(string UserID, string Key, string Data)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference userRef = db.Collection("user");
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
    async public static Task<bool> DataCheck(string UserID)
    {
        bool data = false;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("user").Document(UserID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            data = true;
        }
        return data;
    }
}

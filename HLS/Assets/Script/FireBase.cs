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

        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //���̾�̽����� ������ �ε�(user �÷����� UserID�� ã��)
        DocumentReference docRef = db.Collection("user").Document(UserID);
        //ã�� ������ �ҷ��ͼ� ����

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Debug.Log("���� �˻� ����");
            Dictionary<string, object> ddata = snapshot.ToDictionary();
            data = ddata[Key].ToString();
        }
        else
        {
            Debug.Log("���� �˻� ����");
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

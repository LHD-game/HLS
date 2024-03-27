using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko �� �α��� �ؼ� DBȮ���ϸ� �����ϱ�.

public class FirebaseTest : MonoBehaviour
{
    void Start()
    {

        //���̾�̽��� ������ ���ε�
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference userRef = db.Collection("user");
        userRef.Document("01062341524").SetAsync(new Dictionary<string, object>(){
        { "Name", "LSY" },
        { "Password", "kkkk1!" }
        });




        //���̾�̽����� ������ �ε�
        DocumentReference docRef = db.Collection("user").Document("01062341524");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> user = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in user)
                {
                    Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
                }
            }
            else
            {
                Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko �� �α��� �ؼ� DBȮ���ϸ� �����ϱ�.

public class FirebaseTest : MonoBehaviour
{
    public Text Phone;
    public Text Pw;

    public void Login()
    {
        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //���̾�̽����� ������ �ε�(user �÷����� Phone.text��� �̸��� ������ ã��)
        DocumentReference docRef = db.Collection("user").Document(Phone.text);
        //�ҷ��� ������ ����ȭ�� �� ����
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            //�ҷ��� ���� ���� ���� üũ
            if (snapshot.Exists)
            {
                Debug.Log("���� ����");
                //Phone.text�� �����ϴ� �����ϰ��, �ʵ� �κп� ������ Dictionary������ ������
                Dictionary<string, object> user = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in user);

                //��й�ȣ ��ġ ���� üũ
                if (Pw.text == user["Password"].ToString())
                {
                    Debug.Log("�α��� ����");
                }
                else
                {
                    Debug.Log("�α��� ����");
                }
            }
            else
            {
                Debug.Log("���� ����");
            }
        });
    }
    public void Signin()
    {
        //���̾�̽� ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        //user �÷��� ����
        CollectionReference userRef = db.Collection("user");
        //Phone.text������ ã�Ƽ� �ش� ������ �Ʒ� �������� ��� (���� ��� ����)
        userRef.Document(Phone.text).SetAsync(new Dictionary<string, object>(){
        { "Name", "NONE" },         //�̸� �Ǵ� ������ (����)
        { "Password", Pw.text }     //��й�ȣ ���� (����)
        });
    }
}

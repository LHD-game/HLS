using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko 에 로그인 해서 DB확인하며 진행하기.

public class FirebaseTest : MonoBehaviour
{
    public Text Phone;
    public Text Pw;

    public void Login()
    {
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        //파이어베이스에서 데이터 로드(user 컬렉션중 Phone.text라는 이름에 문서를 찾음)
        DocumentReference docRef = db.Collection("user").Document(Phone.text);
        //불러온 정보를 동기화한 뒤 진행
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            //불러온 정보 존재 여부 체크
            if (snapshot.Exists)
            {
                Debug.Log("계정 있음");
                //Phone.text가 존재하는 문서일경우, 필드 부분에 값들을 Dictionary형으로 가져옴
                Dictionary<string, object> user = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in user);

                //비밀번호 일치 여부 체크
                if (Pw.text == user["Password"].ToString())
                {
                    Debug.Log("로그인 성공");
                }
                else
                {
                    Debug.Log("로그인 실패");
                }
            }
            else
            {
                Debug.Log("계정 없음");
            }
        });
    }
    public void Signin()
    {
        //파이어베이스 연동
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        //user 컬랙션 지정
        CollectionReference userRef = db.Collection("user");
        //Phone.text문서를 찾아서 해당 문서를 아래 내용으로 덮어씀 (없을 경우 생성)
        userRef.Document(Phone.text).SetAsync(new Dictionary<string, object>(){
        { "Name", "NONE" },         //이름 또는 계정명 (예시)
        { "Password", Pw.text }     //비밀번호 지정 (예시)
        });
    }
}

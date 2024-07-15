using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko 에 로그인 해서 DB확인하며 진행하기.

public class FirebaseTest : MonoBehaviour
{
    public Text IdText;     //ID값 
    public Text PwText;     //PW값
    public Text NameText;   //유저명
    public Text NameSpace;  //유저명 결과창

    //FireBase.DataSave([유저 ID], [Key값], [Data값])  | 데이터 저장(덮어쓰기)
    //FireBase.DataLoad([유저 ID], [Key값])            | 데이터 불러오기
    //FireBase.DataCheck([유저 ID]])                   | 데이터 확인

    //|-----------------------서버에서 데이터를 읽어오는 과정----------------------|
    //async는 비동기 작업이 있었다는 의미, FireBase는 비동기로 서버를 운영함.
    async public void Login()
    {
        string Id = IdText.text;    //자료형변환
        string Pw = PwText.text;

        //await는 비동기 값을 동기화할때까지 기다려 달라는 뜻.
        //Data값 = FireBase.DataLoad([유저 ID], [Key값]) | Dictionary형 자료임으로 Key(string)를 통해 Data(string)를 찾음
        if (Pw == await FireBase.DataLoad(Id, "Password"))
        {
            Debug.Log("로그인 성공");
            NameSpace.text = await FireBase.DataLoad(Id, "Name");
        }
        else
        {
            Debug.Log("로그인 실패");
        }
    }

    //|-----------------------서버에서 데이터를 저장하는 과정----------------------|
    async public void Signin()
    {
        string Id = IdText.text;    //자료형변환
        string Pw = PwText.text;
        string Name = NameText.text;

        //|-------------------서버에서 데이터 유무 확인 과정----------------|
        //FireBase.DataLoad([유저 ID]) | Bool값(true, false)으로 나옴
        if (await FireBase.DataCheck(Id))
        {
            Debug.Log("가입 실패 - 이미 있는 ID");
            return;
        }
        else
        {
            //----------------------------------------------------------------|

            //FireBase.DataSave([유저 ID], [Key값], [Data값]) | Dictionary형 자료임으로 Key(string)와 Data(string)를 동시에 저장
            //ID와 Key가 겹칠시 자동으로 덮어쓰기되므로 주의
            await FireBase.DataSave(Id, "Name", Name);
            await FireBase.DataSave(Id, "Password", Pw);

            Debug.Log("가입 성공");
        }
    }
    //|-------------------------------------------------------------------------|
}

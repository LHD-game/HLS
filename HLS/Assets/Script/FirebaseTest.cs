using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko 에 로그인 해서 DB확인하며 진행하기.

public class FirebaseTest : MonoBehaviour
{
    // TODO: 아래 필드명들은 데이터 스키마로 명확히 정리되어야 함 (JSON 트리 예제 작성되면 가장 좋음)
    //     PascalCase 보다 camelCase 로 변수명 선언하면 좋을 것
    public Text IdText;     //ID값 
    public Text PwText;     //PW값
    public Text IdTextSignIn;     //ID값 
    public Text PwTextSignIn;     //PW값
    public Text NameText;   //유저명
    Regex regex = new Regex(@"[a-zA-Z0-9]{1,25}@[a-zA-Z0-9]{1,20}\.[a-zA-Z]{1,5}$");  //e-mail형식
    // TODO: 아래 필드명은 namespace 라는 예약어를 연상시켜 개선 필요
    //     namespace MyNamespace
    //     {
    //       예약어로서 네임스페이스 선언에 사용되므로, 표기법만 다르다고 해서 적절한 네이밍이 아님
    //     }
    public Text UserNameSpace;  //유저명 결과창

    public ScoreData scuns;
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
            UserNameSpace.text = await FireBase.DataLoad(Id, "Name");
            scuns.id = Id;
            scuns.LoginSet();
            WinCtl.Instance.GotoMain(); //main화면으로
        }
        else
        {
            Debug.Log("로그인 실패");
        }
    }

    //|-----------------------서버에서 데이터를 저장하는 과정----------------------|
    async public void Signin()
    {
        string Id = IdTextSignIn.text;    //자료형변환
        string Pw = PwTextSignIn.text;
        string Name = NameText.text;

        if ((regex.IsMatch(Id)))    //이메일 형식 확인
        {
            Debug.Log("email이 양식과 일치합니다.");
            //|-------------------서버에서 데이터 유무 확인 과정----------------|
            //FireBase.DataLoad([유저 ID]) | Bool값(true, false)으로 나옴
            if (await FireBase.DataCheck(Id))
            {
                Debug.Log("가입 실패 - 이미 있는 ID");
            }
            else
            {
                //----------------------------------------------------------------|

                //FireBase.DataSave([유저 ID], [Key값], [Data값]) | Dictionary형 자료임으로 Key(string)와 Data(string)를 동시에 저장
                //ID와 Key가 겹칠시 자동으로 덮어쓰기되므로 주의
                await FireBase.DataSave(Id, "Password", Pw);
                await FireBase.DataSave(Id, "Name", Name);

                Debug.Log("가입 성공");
            }
        }
        else   //이메일 형식이 다를 경우
        {
            Debug.Log("email이 양식과 일치하지 않습니다.");
        }

    }
    //|-------------------------------------------------------------------------|
}

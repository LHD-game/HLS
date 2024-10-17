using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko 에 로그인 해서 DB확인하며 진행하기.

public class FirebaseTest : MonoBehaviour
{
    // TODO: 아래 필드명들은 데이터 스키마로 명확히 정리되어야 함 (JSON 트리 예제 작성되면 가장 좋음)
    //     PascalCase 보다 camelCase 로 변수명 선언하면 좋을 것
    public InputField IdText;     //ID값 
    public InputField PwText;     //PW값
    public InputField IdTextSignIn;     //ID값 
    public InputField PwTextSignIn;     //PW값
    public InputField PwTextSignIn2;    //PW확인값
    public InputField NameText;   //유저명
    public InputField BirthText;  //생년월일
    public Image Male;   //성별
    public Image Female; //성별
    public Image CheckAll; //약관체크전체
    bool checkAll = true;
    public Image Check1; //약관체크1
    bool check1 = true;
    public Image Check2; //약관체크2
    bool check2 = true;
    public Image Check3; //약관체크3
    bool check3 = true;
    public Text MsgText;  //안내문구
    public Text LoginError;     //로그인오류문구
    public GameObject MsgWin;   //안내문구창
    public GameObject GotoLgiBtn;   //안내문구
    string mf = "";             //회원가입시 사용할 성별값
    bool termBool = false;          //회원가입시 약관 확인

    Regex regexPass = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9]).{8,16}$", RegexOptions.IgnorePatternWhitespace); //비밀번호 형식
    Regex regex = new Regex(@"[a-zA-Z0-9]{1,25}@[a-zA-Z0-9]{1,20}\.[a-zA-Z]{1,5}$");  //e-mail 형식
    Regex regex2 = new Regex(@"^\d{4}\d{2}\d{2}$");  //생년월일 형식
    // TODO: 아래 필드명은 namespace 라는 예약어를 연상시켜 개선 필요
    //     namespace MyNamespace
    //     {
    //       예약어로서 네임스페이스 선언에 사용되므로, 표기법만 다르다고 해서 적절한 네이밍이 아님
    //     }

    public ScoreData sd;
    //FireBase.DataSave([유저 ID], [Key값], [Data값])  | 데이터 저장(덮어쓰기)
    //FireBase.DataLoad([유저 ID], [Key값])            | 데이터 불러오기
    //FireBase.DataCheck([유저 ID]])                   | 데이터 확인

    //|-----------------------서버에서 데이터를 읽어오는 과정----------------------|
    //async는 비동기 작업이 있었다는 의미, FireBase는 비동기로 서버를 운영함.
    async public void Login()
    {
        string Id = IdText.text;    //자료형변환
        string Pw = PwText.text;
        string DecryptPw;     //복호화된PW값

        //await는 비동기 값을 동기화할때까지 기다려 달라는 뜻.
        //복호화 과정
        DecryptPw = await DecryptAsync(await FireBase.DataLoad(Id, "Password"));

        //Data값 = FireBase.DataLoad([유저 ID], [Key값]) | Dictionary형 자료임으로 Key(string)를 통해 Data(string)를 찾음
        if (DecryptPw == Pw)
        {
            Debug.Log("로그인 성공");
            await setDefaultData(Id);
            sd = GameObject.FindGameObjectWithTag("ScoreData").GetComponent<ScoreData>();
            sd.Set();
        }
        else if (DecryptPw == null) //로그인 실패시 아이디 비번 오류 발생
        {
            LoginError.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("로그인 실패");
        }

        SceneManager.LoadSceneAsync("Main"); //main화면으로
    }

    //|-----------------------서버에서 데이터를 저장하는 과정----------------------|
    async public void Signin()
    {
        MsgText.text = "";
        bool emailBool = false;
        bool pwBool = false;
        bool birthBool = false;
        bool mfBool = false;

        string Id = IdTextSignIn.text;    //자료형변환
        string Pw = PwTextSignIn.text;
        string Pw2 = PwTextSignIn2.text;
        string Name = NameText.text;
        string Birth = BirthText.text;

        if (regex.IsMatch(Id))    //이메일 형식 확인
        {
            Debug.Log("email이 양식과 일치합니다.");
            //|-------------------서버에서 데이터 유무 확인 과정----------------|
            //FireBase.DataLoad([유저 ID]) | Bool값(true, false)으로 나옴
            if (await FireBase.DataCheck(Id))
            {
                MsgText.text = "이미 가입된 ID입니다.";
                MsgWin.SetActive(true);
                Debug.Log("가입 실패 - 이미 있는 ID");
            }
            else
            {
                emailBool = true;
            }
        }
        else
        {
            MsgText.text = MsgText.text + "email이 양식과 일치하지 않습니다.\n";
        }

        if (!regexPass.IsMatch(Pw))   //비밀번호 확인
        {
            MsgText.text = MsgText.text + "비밀번호가 양식과 일치하지 않습니다.\n";
        }
        else if(Pw != Pw2)
        {
            MsgText.text = MsgText.text + "비밀번호가 일치하지 않습니다.\n";
        }
        else
        {
            pwBool = true;
        }

        if (regex2.IsMatch(Birth))   //생년월일 확인
        {
            DateTime date;
            bool isValid = DateTime.TryParseExact(Birth, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (isValid)
            {
                birthBool = true;
            }
            else
            {
                MsgText.text = MsgText.text + "생년월일이 양식과 일치하지 않습니다.\n";
            }
        }
        else
        {
            MsgText.text = MsgText.text + "생년월일이 양식과 일치하지 않습니다.\n";
        }

        if (mf != "")
        {
            mfBool = true;
        }
        else
        {
            MsgText.text = MsgText.text + "성별이 선택되지 않았습니다.\n";
        }

        if (!termBool)
        {
            MsgText.text = MsgText.text + "약관이 동의되지 않았습니다.\n";
        }

        if (emailBool == true && pwBool == true && birthBool == true && mfBool == true && termBool == true)
        {

            //FireBase.DataSave([유저 ID], [Key값], [Data값]) | Dictionary형 자료임으로 Key(string)와 Data(string)를 동시에 저장
            //ID와 Key가 겹칠시 자동으로 덮어쓰기되므로 주의
            await FireBase.DataSave(Id, "Name", Name);
            await FireBase.DataSave(Id, "Password", Encrypt(Pw));
            await FireBase.DataSave(Id, "Birth", Birth);
            await FireBase.DataSave(Id, "MF", mf);

            MsgText.text = "가입성공";
            MsgWin.SetActive(true);
            GotoLgiBtn.SetActive(true);
            Debug.Log("가입 성공");
        }
        else
        {
            MsgWin.SetActive(true);
        }
        Debug.Log(Id +" "+ Pw + " " + Pw2 + " " + Name + " " + Birth + " " + mf);
        emailBool = false;
        pwBool = false;
        birthBool = false;
    }


    //|-------------------------성별선택----------------------------|
    public void MColorChange()
    {
        Male.color = new Color(50 / 255f, 66 / 255f, 139 / 255f, 1f);
        Male.sprite = Resources.Load<Sprite>("UI/block_02");
        Male.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(50 / 255f, 66 / 255f, 139 / 255f, 1f);
        Female.color = new Color(233 / 255f, 233 / 255f, 233 / 255f, 1f);
        Female.sprite = Resources.Load<Sprite>("UI/block_01");
        Female.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(137 / 255f, 137 / 255f, 137 / 255f, 1f);
        mf = "Male";
    }
    public void FColorChange()
    {
        Male.color = new Color(233 / 255f, 233 / 255f, 233 / 255f, 1f);
        Male.sprite = Resources.Load<Sprite>("UI/block_01");
        Male.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(137 / 255f, 137 / 255f, 137 / 255f, 1f);
        Female.color = new Color(50 / 255f, 66 / 255f, 139 / 255f, 1f);
        Female.sprite = Resources.Load<Sprite>("UI/block_02");
        Female.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(50 / 255f, 66 / 255f, 139 / 255f, 1f);
        mf = "Female";
    }

    //|-------------------------약관----------------------------|
    public void TermCheckAll ()
    {
        if (checkAll)
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            checkAll = false;
            Check1.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            check1 = false;
            Check2.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            check2 = false;
            Check3.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            check3 = false;
            termBool = true;
        }
        else
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            checkAll = true;
            Check1.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            check1 = true;
            Check2.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            check2 = true;
            Check3.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            check3 = true;
            termBool = false;
        }
        Debug.Log("a"+checkAll+"1"+check1+"2"+check2+"3"+check3);
    }
    public void TermCheck1()
    {
        if (check1)
        {
            Check1.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            check1 = false;
        }
        else
        {
            Check1.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            check1 = true;
        }

        if (!check1 && !check2 && !check3)
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            checkAll = false;
            termBool = true;
        }
        else
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            checkAll = true;
            termBool = false;
        }
        Debug.Log("a" + checkAll + "1" + check1 + "2" + check2 + "3" + check3);
    }
    public void TermCheck2()
    {
        if (check2)
        {
            Check2.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            check2 = false;
        }
        else
        {
            Check2.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            check2 = true;
        }

        if (!check1 && !check2 && !check3)
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            checkAll = false;
            termBool = true;
        }
        else
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            checkAll = true;
            termBool = false;
        }
        Debug.Log("a" + checkAll + "1" + check1 + "2" + check2 + "3" + check3);
    }
    public void TermCheck3()
    {
        if (check3)
        {
            Check3.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            check3 = false;
        }
        else
        {
            Check3.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            check3 = true;
        }

        if (!check1 && !check2 && !check3)
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            checkAll = false;
            termBool = true;
        }
        else
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            checkAll = true;
            termBool = false;
        }
        Debug.Log("a" + checkAll + "1" + check1 + "2" + check2 + "3" + check3);
    }

    //|-------------------------암호화 및 복호화----------------------------|
    private static string key = "HLS2402SecureKey";  //암호화용 키
    //암호화
    public static string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                Debug.Log(Convert.ToBase64String(ms.ToArray()));
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
    //복호화
    public static async Task<string> DecryptAsync(string cipherText)
    {
        byte[] fullCipher;
        try
        {
            // Base64 문자열을 바이트 배열로 변환
            fullCipher = Convert.FromBase64String(cipherText);
        }
        catch (FormatException ex)
        {
            Debug.Log("ERROR : " + ex.Message);
            return null;
        }

        byte[] iv = new byte[16];
        byte[] cipher = new byte[fullCipher.Length - iv.Length];

        Array.Copy(fullCipher, iv, iv.Length);
        Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // 비동기 복호화 처리
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        // 비동기로 텍스트를 읽음
                        return await sr.ReadToEndAsync();
                    }
                }
            }
        }
    }

    //|------------------------로그인 정보 저장----------------------------|
    async Task setDefaultData(string Id)
    {
        PlayerPrefs.SetString("UserID", Id);

        PlayerPrefs.SetString("UserName", await FireBase.DataLoad(Id, "Name"));

        PlayerPrefs.SetString("MF", await FireBase.DataLoad(Id, "MF"));

        PlayerPrefs.SetString("Birth: ", await FireBase.DataLoad(Id, "Birth"));
    }
}
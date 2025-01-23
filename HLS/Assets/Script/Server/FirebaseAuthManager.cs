using System;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;          // Firebase Authentication 인스턴스
    private FirebaseUser currentUser;   // 현재 로그인된 사용자
    public InputField signupEmail;
    public InputField signupPass;
    public InputField signupPass2;
    public InputField loginEmail;
    public InputField loginPass;
    public InputField nameText;   //유저명
    public InputField birthText;  //생년월일
    public Image Male;   //성별
    public Image Female; //성별
    public Image CheckAll; //약관체크전체
    bool checkAll = true;
    public Image Check1; //약관체크1
    bool check1 = true;
    public Image Check2; //약관체크2
    bool check2 = true;
    //자동로그인
    public Image ALCheck; //자동로그인체크
    bool ALCheckbool = false;
    //로그인오류문구
    [Header("LogInError")]
    public Text LoginError;
    [Header("SignInError")]
    //회원가입 오류 문구 및 UI
    public Text mailError;
    public Text pass1Error;
    public Text pass2Error;
    public Text nameError;
    public Text birthError;
    public Text mfError;
    public Text termError;
    public GameObject SignInUI;
    public GameObject SignInComplete;

    string mf = "";                 //회원가입시 사용할 성별값
    bool nameBool = false;          //회원가입시 이름 확인
    bool termBool = false;          //회원가입시 약관 확인

    //string 형식 확인용
    Regex regexPass = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9]).{8,16}$", RegexOptions.IgnorePatternWhitespace); //비밀번호 형식
    Regex regex = new Regex(@"[a-zA-Z0-9]{1,25}@[a-zA-Z0-9]{1,20}\.[a-zA-Z]{1,5}$");                         //e-mail 형식
    Regex regex2 = new Regex(@"^\d{4}\d{2}\d{2}$");                                                          //생년월일 형식

    void Start()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("파이어베이스 인식 성공");
            }
            else
            {
                Debug.LogError("파이어베이스 인식 실패 : " + task.Result);
            }
        });

        if (PlayerPrefs.GetInt("autoLogin") == 1)
        {
            SceneManager.LoadSceneAsync("Main"); //main화면으로
        }
    }

    // |-------------------------회원가입 함수---------------------------|
    async public void SignUp(string email, string password)
    {
        bool emailBool = false;
        bool pwBool = false;
        bool birthBool = false;
        bool mfBool = false;

        string Id = signupEmail.text;    //자료형변환
        string Pw = signupPass.text;
        string Pw2 = signupPass2.text;
        string name = nameText.text;
        string Birth = birthText.text;

        mailError.gameObject.SetActive(false);
        pass1Error.gameObject.SetActive(false);
        pass2Error.gameObject.SetActive(false);
        birthError.gameObject.SetActive(false);
        mfError.gameObject.SetActive(false);
        nameError.gameObject.SetActive(false);
        termError.gameObject.SetActive(false);

        if (regex.IsMatch(Id))    //이메일 형식 확인
        {
            emailBool = true;
        }
        else
        {
            mailError.gameObject.SetActive(true);
        }

        if (!regexPass.IsMatch(Pw))   //비밀번호 확인
        {
            pass1Error.gameObject.SetActive(true);
        }
        else if (Pw != Pw2)
        {
            pass2Error.gameObject.SetActive(true);
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
                birthError.gameObject.SetActive(true);
            }
        }
        else
        {
            birthError.gameObject.SetActive(true);
        }

        if (name != "")
        {
            nameBool = true;

        }
        else
        {
            nameError.gameObject.SetActive(true);
        }

        if (mf != "")
        {
            mfBool = true;
        }
        else
        {
            mfError.gameObject.SetActive(true);
        }

        if (!termBool)
        {
            termError.gameObject.SetActive(true);
        }

        Debug.Log(emailBool + "+" + pwBool + "+" + nameBool + "+" + birthBool + "+" + mfBool + "+" + termBool);

        if (emailBool && pwBool && nameBool && birthBool && mfBool && termBool)
        {
            try
            {
                // Firebase Authentication으로 사용자 생성
                var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
                var currentUser = authResult.User;
                string uid = currentUser.UserId;

                Debug.Log($"회원가입 성공: {currentUser.Email}, UID: {uid}");

                // Firestore에 UID로 데이터 저장
                await FireBase.DataSave(uid, "Name", name);
                await FireBase.DataSave(uid, "Birth", Birth);
                await FireBase.DataSave(uid, "MF", mf);

                Debug.Log("Firestore에 데이터 저장 성공");

                SignInComplete.SetActive(true);
                ResetStatus();
            }
            catch (Exception ex)
            {
                Debug.LogError($"회원가입 실패 또는 데이터 저장 실패: {ex.Message}");
            }
        }

        emailBool = false;
        pwBool = false;
        birthBool = false;
        nameBool = false;
    }

    // |-------------------------로그인 함수----------------------------|
    async public void LogIn(string email, string password)
    {
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("로그인 실패 : " + task.Exception?.Message);
                LoginError.gameObject.SetActive(true);
                return;
            }

            // 로그인 성공
            var authResult = task.Result;           // AuthResult 가져오기
            currentUser = authResult.User;          // FirebaseUser 가져오기
            Debug.Log($"로그인 성공 : {currentUser.Email}");

            if (ALCheckbool)   //자동로그인 확정
            {
                PlayerPrefs.SetInt("autoLogin", 1);
            }

            FireBase.SurveyDataLoad("AUDIT", "A1-1");

            SceneManager.LoadSceneAsync("Main"); //main화면으로
        });
    }

    // |---------------------------성별선택-----------------------------|
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

    // |----------------------------초기화------------------------------|
    public void ResetStatus()
    {

        mailError.gameObject.SetActive(false);
        pass1Error.gameObject.SetActive(false);
        pass2Error.gameObject.SetActive(false);
        birthError.gameObject.SetActive(false);
        mfError.gameObject.SetActive(false);
        nameError.gameObject.SetActive(false);
        termError.gameObject.SetActive(false);

        signupEmail.text = "";
        signupPass.text = "";
        signupPass2.text = "";
        nameText.text = "";
        birthText.text = "";
        Male.color = new Color(233 / 255f, 233 / 255f, 233 / 255f, 1f);
        Male.sprite = Resources.Load<Sprite>("UI/block_01");
        Male.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(137 / 255f, 137 / 255f, 137 / 255f, 1f);
        Female.color = new Color(233 / 255f, 233 / 255f, 233 / 255f, 1f);
        Female.sprite = Resources.Load<Sprite>("UI/block_01");
        Female.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(137 / 255f, 137 / 255f, 137 / 255f, 1f);
        mf = "";
        if (termBool)
            TermCheckAll();
    }

    // |---------------------------버튼 동작----------------------------|
    public void OnSignUpButtonClicked()
    {
        string email = signupEmail.text;        // InputField에서 이메일 가져오기
        string password = signupPass.text;      // InputField에서 비밀번호 가져오기

        SignUp(email, password);
    }

    public void OnLogInButtonClicked()
    {
        string email = loginEmail.text;        // InputField에서 이메일 가져오기
        string password = loginPass.text;      // InputField에서 비밀번호 가져오기

        LogIn(email, password);
    }

    // |------------------------자동로그인 옵션-------------------------|
    public void AutoLoginCheck()       //자동로그인
    {
        if (!ALCheckbool)
        {
            ALCheck.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            ALCheckbool = true;
        }
        else
        {
            ALCheck.sprite = Resources.Load<Sprite>("UI/Frame_ItemFrame02_d_1");
            ALCheckbool = false;
        }
        Debug.Log("자동로그인 활성화");
    }

    // |----------------------------약관-------------------------------|
    public void TermCheckAll()
    {
        if (checkAll)
        {
            CheckAll.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            checkAll = false;
            Check1.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            check1 = false;
            Check2.sprite = Resources.Load<Sprite>("UI/Toggle_Square_s_on");
            check2 = false;
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
            termBool = false;
        }
        Debug.Log("a" + checkAll + "1" + check1 + "2" + check2);
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

        if (!check1 && !check2)
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
        Debug.Log("a" + checkAll + "1" + check1 + "2" + check2 + "3");
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

        if (!check1 && !check2)
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
        Debug.Log("a" + checkAll + "1" + check1 + "2" + check2 + "3");
    }
    // |--------------------------계정삭제-----------------------------|
    public async void DeleteAccount()
    {
        // 현재 사용자 가져오기
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        string uid = user.UserId; // UID 가져오기

        // Firestore에서 데이터 삭제
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        await db.Collection("user").Document(uid).DeleteAsync();
        Debug.Log("Firestore에서 사용자 데이터 삭제 성공");

        // Firebase Authentication에서 삭제
        await user.DeleteAsync();
        Debug.Log("Firebase Authentication에서 사용자 삭제 성공");

        SceneManager.LoadScene("LoginScene");
    }
}
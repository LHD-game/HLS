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
    private FirebaseAuth auth;          // Firebase Authentication �ν��Ͻ�
    private FirebaseUser currentUser;   // ���� �α��ε� �����
    public InputField signupEmail;
    public InputField signupPass;
    public InputField signupPass2;
    public InputField loginEmail;
    public InputField loginPass;
    public InputField nameText;   //������
    public InputField birthText;  //�������
    public Image Male;   //����
    public Image Female; //����
    public Image CheckAll; //���üũ��ü
    bool checkAll = true;
    public Image Check1; //���üũ1
    bool check1 = true;
    public Image Check2; //���üũ2
    bool check2 = true;
    //�ڵ��α���
    public Image ALCheck; //�ڵ��α���üũ
    bool ALCheckbool = false;
    //�α��ο�������
    [Header("LogInError")]
    public Text LoginError;
    [Header("SignInError")]
    //ȸ������ ���� ���� �� UI
    public Text mailError;
    public Text pass1Error;
    public Text pass2Error;
    public Text nameError;
    public Text birthError;
    public Text mfError;
    public Text termError;
    public GameObject SignInUI;
    public GameObject SignInComplete;

    string mf = "";                 //ȸ�����Խ� ����� ������
    bool nameBool = false;          //ȸ�����Խ� �̸� Ȯ��
    bool termBool = false;          //ȸ�����Խ� ��� Ȯ��

    //string ���� Ȯ�ο�
    Regex regexPass = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9]).{8,16}$", RegexOptions.IgnorePatternWhitespace); //��й�ȣ ����
    Regex regex = new Regex(@"[a-zA-Z0-9]{1,25}@[a-zA-Z0-9]{1,20}\.[a-zA-Z]{1,5}$");                         //e-mail ����
    Regex regex2 = new Regex(@"^\d{4}\d{2}\d{2}$");                                                          //������� ����

    void Start()
    {
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("���̾�̽� �ν� ����");
            }
            else
            {
                Debug.LogError("���̾�̽� �ν� ���� : " + task.Result);
            }
        });

        if (PlayerPrefs.GetInt("autoLogin") == 1)
        {
            SceneManager.LoadSceneAsync("Main"); //mainȭ������
        }
    }

    // |-------------------------ȸ������ �Լ�---------------------------|
    async public void SignUp(string email, string password)
    {
        bool emailBool = false;
        bool pwBool = false;
        bool birthBool = false;
        bool mfBool = false;

        string Id = signupEmail.text;    //�ڷ�����ȯ
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

        if (regex.IsMatch(Id))    //�̸��� ���� Ȯ��
        {
            emailBool = true;
        }
        else
        {
            mailError.gameObject.SetActive(true);
        }

        if (!regexPass.IsMatch(Pw))   //��й�ȣ Ȯ��
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

        if (regex2.IsMatch(Birth))   //������� Ȯ��
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
                // Firebase Authentication���� ����� ����
                var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
                var currentUser = authResult.User;
                string uid = currentUser.UserId;

                Debug.Log($"ȸ������ ����: {currentUser.Email}, UID: {uid}");

                // Firestore�� UID�� ������ ����
                await FireBase.DataSave(uid, "Name", name);
                await FireBase.DataSave(uid, "Birth", Birth);
                await FireBase.DataSave(uid, "MF", mf);

                Debug.Log("Firestore�� ������ ���� ����");

                SignInComplete.SetActive(true);
                ResetStatus();
            }
            catch (Exception ex)
            {
                Debug.LogError($"ȸ������ ���� �Ǵ� ������ ���� ����: {ex.Message}");
            }
        }

        emailBool = false;
        pwBool = false;
        birthBool = false;
        nameBool = false;
    }

    // |-------------------------�α��� �Լ�----------------------------|
    async public void LogIn(string email, string password)
    {
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("�α��� ���� : " + task.Exception?.Message);
                LoginError.gameObject.SetActive(true);
                return;
            }

            // �α��� ����
            var authResult = task.Result;           // AuthResult ��������
            currentUser = authResult.User;          // FirebaseUser ��������
            Debug.Log($"�α��� ���� : {currentUser.Email}");

            if (ALCheckbool)   //�ڵ��α��� Ȯ��
            {
                PlayerPrefs.SetInt("autoLogin", 1);
            }

            FireBase.SurveyDataLoad("AUDIT", "A1-1");

            SceneManager.LoadSceneAsync("Main"); //mainȭ������
        });
    }

    // |---------------------------��������-----------------------------|
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

    // |----------------------------�ʱ�ȭ------------------------------|
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

    // |---------------------------��ư ����----------------------------|
    public void OnSignUpButtonClicked()
    {
        string email = signupEmail.text;        // InputField���� �̸��� ��������
        string password = signupPass.text;      // InputField���� ��й�ȣ ��������

        SignUp(email, password);
    }

    public void OnLogInButtonClicked()
    {
        string email = loginEmail.text;        // InputField���� �̸��� ��������
        string password = loginPass.text;      // InputField���� ��й�ȣ ��������

        LogIn(email, password);
    }

    // |------------------------�ڵ��α��� �ɼ�-------------------------|
    public void AutoLoginCheck()       //�ڵ��α���
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
        Debug.Log("�ڵ��α��� Ȱ��ȭ");
    }

    // |----------------------------���-------------------------------|
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
    // |--------------------------��������-----------------------------|
    public async void DeleteAccount()
    {
        // ���� ����� ��������
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        string uid = user.UserId; // UID ��������

        // Firestore���� ������ ����
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        await db.Collection("user").Document(uid).DeleteAsync();
        Debug.Log("Firestore���� ����� ������ ���� ����");

        // Firebase Authentication���� ����
        await user.DeleteAsync();
        Debug.Log("Firebase Authentication���� ����� ���� ����");

        SceneManager.LoadScene("LoginScene");
    }
}
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

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko �� �α��� �ؼ� DBȮ���ϸ� �����ϱ�.

public class FirebaseTest : MonoBehaviour
{
    // TODO: �Ʒ� �ʵ����� ������ ��Ű���� ��Ȯ�� �����Ǿ�� �� (JSON Ʈ�� ���� �ۼ��Ǹ� ���� ����)
    //     PascalCase ���� camelCase �� ������ �����ϸ� ���� ��
    public InputField IdText;     //ID�� 
    public InputField PwText;     //PW��
    public InputField IdTextSignIn;     //ID�� 
    public InputField PwTextSignIn;     //PW��
    public InputField PwTextSignIn2;    //PWȮ�ΰ�
    public InputField NameText;   //������
    public InputField BirthText;  //�������
    public Image Male;   //����
    public Image Female; //����
    public Image CheckAll; //���üũ��ü
    bool checkAll = true;
    public Image Check1; //���üũ1
    bool check1 = true;
    public Image Check2; //���üũ2
    bool check2 = true;
    public Image Check3; //���üũ3
    bool check3 = true;
    public Text MsgText;  //�ȳ�����
    public Text LoginError;     //�α��ο�������
    public GameObject MsgWin;   //�ȳ�����â
    public GameObject GotoLgiBtn;   //�ȳ�����
    string mf = "";             //ȸ�����Խ� ����� ������
    bool termBool = false;          //ȸ�����Խ� ��� Ȯ��

    Regex regexPass = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9]).{8,16}$", RegexOptions.IgnorePatternWhitespace); //��й�ȣ ����
    Regex regex = new Regex(@"[a-zA-Z0-9]{1,25}@[a-zA-Z0-9]{1,20}\.[a-zA-Z]{1,5}$");  //e-mail ����
    Regex regex2 = new Regex(@"^\d{4}\d{2}\d{2}$");  //������� ����
    // TODO: �Ʒ� �ʵ���� namespace ��� ���� ������� ���� �ʿ�
    //     namespace MyNamespace
    //     {
    //       �����μ� ���ӽ����̽� ���� ���ǹǷ�, ǥ����� �ٸ��ٰ� �ؼ� ������ ���̹��� �ƴ�
    //     }

    public ScoreData sd;
    //FireBase.DataSave([���� ID], [Key��], [Data��])  | ������ ����(�����)
    //FireBase.DataLoad([���� ID], [Key��])            | ������ �ҷ�����
    //FireBase.DataCheck([���� ID]])                   | ������ Ȯ��

    //|-----------------------�������� �����͸� �о���� ����----------------------|
    //async�� �񵿱� �۾��� �־��ٴ� �ǹ�, FireBase�� �񵿱�� ������ ���.
    async public void Login()
    {
        string Id = IdText.text;    //�ڷ�����ȯ
        string Pw = PwText.text;
        string DecryptPw;     //��ȣȭ��PW��

        //await�� �񵿱� ���� ����ȭ�Ҷ����� ��ٷ� �޶�� ��.
        //��ȣȭ ����
        DecryptPw = await DecryptAsync(await FireBase.DataLoad(Id, "Password"));

        //Data�� = FireBase.DataLoad([���� ID], [Key��]) | Dictionary�� �ڷ������� Key(string)�� ���� Data(string)�� ã��
        if (DecryptPw == Pw)
        {
            Debug.Log("�α��� ����");
            await setDefaultData(Id);
            sd = GameObject.FindGameObjectWithTag("ScoreData").GetComponent<ScoreData>();
            sd.Set();
        }
        else if (DecryptPw == null) //�α��� ���н� ���̵� ��� ���� �߻�
        {
            LoginError.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("�α��� ����");
        }

        SceneManager.LoadSceneAsync("Main"); //mainȭ������
    }

    //|-----------------------�������� �����͸� �����ϴ� ����----------------------|
    async public void Signin()
    {
        MsgText.text = "";
        bool emailBool = false;
        bool pwBool = false;
        bool birthBool = false;
        bool mfBool = false;

        string Id = IdTextSignIn.text;    //�ڷ�����ȯ
        string Pw = PwTextSignIn.text;
        string Pw2 = PwTextSignIn2.text;
        string Name = NameText.text;
        string Birth = BirthText.text;

        if (regex.IsMatch(Id))    //�̸��� ���� Ȯ��
        {
            Debug.Log("email�� ��İ� ��ġ�մϴ�.");
            //|-------------------�������� ������ ���� Ȯ�� ����----------------|
            //FireBase.DataLoad([���� ID]) | Bool��(true, false)���� ����
            if (await FireBase.DataCheck(Id))
            {
                MsgText.text = "�̹� ���Ե� ID�Դϴ�.";
                MsgWin.SetActive(true);
                Debug.Log("���� ���� - �̹� �ִ� ID");
            }
            else
            {
                emailBool = true;
            }
        }
        else
        {
            MsgText.text = MsgText.text + "email�� ��İ� ��ġ���� �ʽ��ϴ�.\n";
        }

        if (!regexPass.IsMatch(Pw))   //��й�ȣ Ȯ��
        {
            MsgText.text = MsgText.text + "��й�ȣ�� ��İ� ��ġ���� �ʽ��ϴ�.\n";
        }
        else if(Pw != Pw2)
        {
            MsgText.text = MsgText.text + "��й�ȣ�� ��ġ���� �ʽ��ϴ�.\n";
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
                MsgText.text = MsgText.text + "��������� ��İ� ��ġ���� �ʽ��ϴ�.\n";
            }
        }
        else
        {
            MsgText.text = MsgText.text + "��������� ��İ� ��ġ���� �ʽ��ϴ�.\n";
        }

        if (mf != "")
        {
            mfBool = true;
        }
        else
        {
            MsgText.text = MsgText.text + "������ ���õ��� �ʾҽ��ϴ�.\n";
        }

        if (!termBool)
        {
            MsgText.text = MsgText.text + "����� ���ǵ��� �ʾҽ��ϴ�.\n";
        }

        if (emailBool == true && pwBool == true && birthBool == true && mfBool == true && termBool == true)
        {

            //FireBase.DataSave([���� ID], [Key��], [Data��]) | Dictionary�� �ڷ������� Key(string)�� Data(string)�� ���ÿ� ����
            //ID�� Key�� ��ĥ�� �ڵ����� �����ǹǷ� ����
            await FireBase.DataSave(Id, "Name", Name);
            await FireBase.DataSave(Id, "Password", Encrypt(Pw));
            await FireBase.DataSave(Id, "Birth", Birth);
            await FireBase.DataSave(Id, "MF", mf);

            MsgText.text = "���Լ���";
            MsgWin.SetActive(true);
            GotoLgiBtn.SetActive(true);
            Debug.Log("���� ����");
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


    //|-------------------------��������----------------------------|
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

    //|-------------------------���----------------------------|
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

    //|-------------------------��ȣȭ �� ��ȣȭ----------------------------|
    private static string key = "HLS2402SecureKey";  //��ȣȭ�� Ű
    //��ȣȭ
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
    //��ȣȭ
    public static async Task<string> DecryptAsync(string cipherText)
    {
        byte[] fullCipher;
        try
        {
            // Base64 ���ڿ��� ����Ʈ �迭�� ��ȯ
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

            // �񵿱� ��ȣȭ ó��
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        // �񵿱�� �ؽ�Ʈ�� ����
                        return await sr.ReadToEndAsync();
                    }
                }
            }
        }
    }

    //|------------------------�α��� ���� ����----------------------------|
    async Task setDefaultData(string Id)
    {
        PlayerPrefs.SetString("UserID", Id);

        PlayerPrefs.SetString("UserName", await FireBase.DataLoad(Id, "Name"));

        PlayerPrefs.SetString("MF", await FireBase.DataLoad(Id, "MF"));

        PlayerPrefs.SetString("Birth: ", await FireBase.DataLoad(Id, "Birth"));
    }
}
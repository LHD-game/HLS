using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko �� �α��� �ؼ� DBȮ���ϸ� �����ϱ�.

public class FirebaseTest : MonoBehaviour
{
    // TODO: �Ʒ� �ʵ����� ������ ��Ű���� ��Ȯ�� �����Ǿ�� �� (JSON Ʈ�� ���� �ۼ��Ǹ� ���� ����)
    //     PascalCase ���� camelCase �� ������ �����ϸ� ���� ��
    public Text IdText;     //ID�� 
    public Text PwText;     //PW��
    public Text IdTextSignIn;     //ID�� 
    public Text PwTextSignIn;     //PW��
    public Text NameText;   //������
    Regex regex = new Regex(@"[a-zA-Z0-9]{1,25}@[a-zA-Z0-9]{1,20}\.[a-zA-Z]{1,5}$");  //e-mail����
    // TODO: �Ʒ� �ʵ���� namespace ��� ���� ������� ���� �ʿ�
    //     namespace MyNamespace
    //     {
    //       �����μ� ���ӽ����̽� ���� ���ǹǷ�, ǥ����� �ٸ��ٰ� �ؼ� ������ ���̹��� �ƴ�
    //     }
    public Text UserNameSpace;  //������ ���â

    public ScoreData scuns;
    //FireBase.DataSave([���� ID], [Key��], [Data��])  | ������ ����(�����)
    //FireBase.DataLoad([���� ID], [Key��])            | ������ �ҷ�����
    //FireBase.DataCheck([���� ID]])                   | ������ Ȯ��

    //|-----------------------�������� �����͸� �о���� ����----------------------|
    //async�� �񵿱� �۾��� �־��ٴ� �ǹ�, FireBase�� �񵿱�� ������ ���.
    async public void Login()
    {
        string Id = IdText.text;    //�ڷ�����ȯ
        string Pw = PwText.text;

        //await�� �񵿱� ���� ����ȭ�Ҷ����� ��ٷ� �޶�� ��.
        //Data�� = FireBase.DataLoad([���� ID], [Key��]) | Dictionary�� �ڷ������� Key(string)�� ���� Data(string)�� ã��
        if (Pw == await FireBase.DataLoad(Id, "Password"))
        {
            Debug.Log("�α��� ����");
            UserNameSpace.text = await FireBase.DataLoad(Id, "Name");
            scuns.id = Id;
            scuns.LoginSet();
            WinCtl.Instance.GotoMain(); //mainȭ������
        }
        else
        {
            Debug.Log("�α��� ����");
        }
    }

    //|-----------------------�������� �����͸� �����ϴ� ����----------------------|
    async public void Signin()
    {
        string Id = IdTextSignIn.text;    //�ڷ�����ȯ
        string Pw = PwTextSignIn.text;
        string Name = NameText.text;

        if ((regex.IsMatch(Id)))    //�̸��� ���� Ȯ��
        {
            Debug.Log("email�� ��İ� ��ġ�մϴ�.");
            //|-------------------�������� ������ ���� Ȯ�� ����----------------|
            //FireBase.DataLoad([���� ID]) | Bool��(true, false)���� ����
            if (await FireBase.DataCheck(Id))
            {
                Debug.Log("���� ���� - �̹� �ִ� ID");
            }
            else
            {
                //----------------------------------------------------------------|

                //FireBase.DataSave([���� ID], [Key��], [Data��]) | Dictionary�� �ڷ������� Key(string)�� Data(string)�� ���ÿ� ����
                //ID�� Key�� ��ĥ�� �ڵ����� �����ǹǷ� ����
                await FireBase.DataSave(Id, "Password", Pw);
                await FireBase.DataSave(Id, "Name", Name);

                Debug.Log("���� ����");
            }
        }
        else   //�̸��� ������ �ٸ� ���
        {
            Debug.Log("email�� ��İ� ��ġ���� �ʽ��ϴ�.");
        }

    }
    //|-------------------------------------------------------------------------|
}

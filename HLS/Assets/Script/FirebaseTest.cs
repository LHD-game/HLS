using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

//https://console.firebase.google.com/u/1/project/hlsapp-2d873/firestore/data/~2Fuser~2F01062341524?hl=ko �� �α��� �ؼ� DBȮ���ϸ� �����ϱ�.

public class FirebaseTest : MonoBehaviour
{
    public Text IdText;     //ID�� 
    public Text PwText;     //PW��
    public Text NameText;   //������
    public Text NameSpace;  //������ ���â

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
            NameSpace.text = await FireBase.DataLoad(Id, "Name");
        }
        else
        {
            Debug.Log("�α��� ����");
        }
    }

    //|-----------------------�������� �����͸� �����ϴ� ����----------------------|
    async public void Signin()
    {
        string Id = IdText.text;    //�ڷ�����ȯ
        string Pw = PwText.text;
        string Name = NameText.text;

        //|-------------------�������� ������ ���� Ȯ�� ����----------------|
        //FireBase.DataLoad([���� ID]) | Bool��(true, false)���� ����
        if (await FireBase.DataCheck(Id))
        {
            Debug.Log("���� ���� - �̹� �ִ� ID");
            return;
        }
        else
        {
            //----------------------------------------------------------------|

            //FireBase.DataSave([���� ID], [Key��], [Data��]) | Dictionary�� �ڷ������� Key(string)�� Data(string)�� ���ÿ� ����
            //ID�� Key�� ��ĥ�� �ڵ����� �����ǹǷ� ����
            await FireBase.DataSave(Id, "Name", Name);
            await FireBase.DataSave(Id, "Password", Pw);

            Debug.Log("���� ����");
        }
    }
    //|-------------------------------------------------------------------------|
}

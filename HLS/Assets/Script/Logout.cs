using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public void LogOut()       //�ڵ��α���
    {
        PlayerPrefs.SetInt("autoLogin", 0);

        SceneManager.LoadSceneAsync("LoginScene"); //mainȭ������

        Debug.Log("�ڵ��α���  ��Ȱ��ȭ");
        Debug.Log("�α׾ƿ�");
    }
}

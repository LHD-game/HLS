using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    public void LogOut()       //자동로그인
    {
        PlayerPrefs.SetInt("autoLogin", 0);

        SceneManager.LoadSceneAsync("LoginScene"); //main화면으로

        Debug.Log("자동로그인  비활성화");
        Debug.Log("로그아웃");
    }
}

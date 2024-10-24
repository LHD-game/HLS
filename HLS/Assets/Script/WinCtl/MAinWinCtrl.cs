using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MAinWinCtl : MonoBehaviour
{
    public Text UserName;
    public Text HelloUser;
    public Text UserRecentScore;

    public ScoreData Sd;

    private void Start()
    {
        Sd = GameObject.FindGameObjectWithTag("ScoreData").GetComponent<ScoreData>();
        MainSetting();
    }

    public void MainSetting()
    {
        UserName.text = PlayerPrefs.GetString("UserName");
        HelloUser.text = $"<color=#32438B>{UserName.text}�� �ȳ��ϼ���?</color>\n\nHLS���� �������� �Բ� �ǰ��� �������� ����������.";
        //Debug.Log("Main"+Sd.ScoreData_.Count);
        StartCoroutine(UpdateRecentScore());
    }

    IEnumerator UpdateRecentScore()
    {
        int noRscore = 0;
        while (true)
        {
            //Debug.Log("While");
            if (Sd.ScoreData_.Count < 1)
            {
                UserRecentScore.text = "0��";
                noRscore++;
                if (noRscore < 10)
                    yield return new WaitForFixedUpdate();
                else
                    break;
            }
            else
            {
                UserRecentScore.text = Sd.ScoreData_[Sd.ScoreData_.Count - 1]["total"] + "��";
                break;
            }
        }
        PlayerPrefs.SetString("RecentScore", UserRecentScore.text);
    }
}

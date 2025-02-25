using ChartAndGraph;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using Firebase.Firestore;

public class RaderDraw : MonoBehaviour
{
    [Header("script")]
    private const string Pattern = @"\\n";
    public ScoreData scuns;
    public RadarChart chart;
    public RadarChart chartBg;
    public ScoreManager scoreManager;
    public ResolutWinCtrl ResolutWinCtrl;
    //public int segments; // �׷����� ���׸�Ʈ �� data.Length
    public RectTransform WheelPrent;

    [Header("Bar")]
    public float radius; // ���� ������
    //public float animationSpeed = 1f; // �ִϸ��̼� �ӵ�
    public Slider slider; // �����̴� UI
    public Slider[] bars;
    public RectTransform BarPos;

    [Space(10f)]
    public GameObject Mysign;
    public Sprite[] MysignImgs;
    string mysigncolor;
    public Text scoreTxt;

    [Header("Data")]
    public string[] TitleTxts;
    [Space(5f)]
    public int[] data;
    [Space(5f)]
    public Text Date;
    public Text adviceTxt;
    [Space(5f)]
    public int Score;
    [Space(5f)]
    public GameObject ConnetWarningWin;


    private void Start()
    {
        StartCoroutine(start());
    }

    IEnumerator start()
    {
        yield return new WaitForSeconds(0.2f);
        startSet();
    }

    private void startSet()
    {
        scuns = GameObject.FindGameObjectWithTag("ScoreData").GetComponent<ScoreData>();
        if (WheelPrent.rect.width > WheelPrent.rect.height)
            chart.Radius = WheelPrent.rect.height / 3f;
        else
            chart.Radius = WheelPrent.rect.width / 3f;

        chartBg.Radius = chart.Radius;
        //Debug.Log(chart.Radius);

        //Debug.Log(WheelPrent.rect.width + " , " + WheelPrent.rect.height);

        chart.Angle = 70;
        chartBg.Angle = chart.Angle;
        for (int i = 0; i < data.Length; i++)
        {
            chart.DataSource.AddGroup(TitleTxts[i]);
        }
    }

    public void PrintDetail(int index)
    {
        buttonC(index);
        //WinCtl.Instance.GotoDatailWin();
    }

    public void buttonC(int index)
    {
        GetData(index); 
        DetailPrint(index);
        SetDate(index);
        CreateBars(index);
    }

    void DetailPrint(int index)
    {
        Score = Convert.ToInt32(scuns.ScoreData_[index]["total"]);
        scoreTxt.text = Score.ToString();
    }

    void GetData(int index)
    {
        for (int i = 0; i < data.Length; i++) //segment -> header.Length-1
        {
            chart.DataSource.SetValue("MyScore", TitleTxts[i], Int32.Parse(scuns.ScoreData_[index][scuns.header[i + 1]].ToString()));
        }

    }

    //���� �� ����
    async public void addData(Dictionary<string, object> Data, string surveyType)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db.Collection("user").Document(scuns.id);
        int cnnetCount = 0;
        while (true)
        {
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists) //���� ���� ����
            {
                //HLS���� ����
                bool isHLS = false;
                if (surveyType.Equals("HLS"))
                    isHLS = true;

                //����Ʈ�� ��¥ ���� ����
                Data.Add("date", System.DateTime.Now.ToString("yy MM dd"));

                //����Ʈ ��� ����
                string[] header;
                if (isHLS)
                    header = scuns.header;
                else
                    header = scuns.otherheader;

                await scuns.addData(surveyType, Data, header);

                if (isHLS)
                {
                    Debug.Log("HLS���â");
                    buttonC(scuns.ScoreData_.Count - 1);
                    ResolutWinCtrl.setResolutWin(mysigncolor);
                }
                else
                {
                    WinCtl.Instance.GotoOtherTestResoult(); Debug.Log("other���â");
                }
                return;
            }
            else //�������� ����
            {
                Debug.Log("���� ����");
                if (cnnetCount > 10)
                {
                    StartCoroutine(WinCtl.Instance.warningWinCtl(ConnetWarningWin));
                    break;
                }
                else
                    cnnetCount++;
            }
        }
    }

    void CreateBars(int index)
    {
        bars = new Slider[data.Length];

        float angleStep = 360f / data.Length;

        for (int i = 0; i < data.Length; i++)
        {
            float angle = (i * angleStep) -110;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle + 90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, BarPos.transform.position + barPosition, Quaternion.identity, BarPos.transform);
            bars[i] = bar;

            //���� ������ ����
            RectTransform thisRect = bar.GetComponent<RectTransform>();

            //if (WheelPrent.rect.width > WheelPrent.parent.GetComponent<RectTransform>().rect.width)
                thisRect.sizeDelta = new Vector2(chart.Radius+2, chart.Radius+2);
            /*else
                thisRect.sizeDelta = new Vector2((WheelPrent.parent.GetComponent<RectTransform>().rect.width / 3), thisRect.rect.height);
*/
            // ������ ȸ�� ����
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            //������ �� ����
            string LabelTxt = Regex.Replace(chart.DataSource.GetGroupName(i), Pattern, " ");
            bar.GetComponentInChildren<Text>().text = LabelTxt;//scuns.header[i + 1].ToString();

        }

        UpdateGraph();
    }

    void UpdateGraph()
    {
        for (int i = 0; i < data.Length; i++)
            if (Score < 72)
            { Mysign.GetComponent<Image>().sprite = MysignImgs[0];
                mysigncolor = "������";
            }
            else if (92 < Score && Score < 114)
            {
                Mysign.GetComponent<Image>().sprite = MysignImgs[1];
                mysigncolor = "�����";
            }
            else if (114 < Score)
            {
                Mysign.GetComponent<Image>().sprite = MysignImgs[2];
                mysigncolor = "�ʷϺ�";
            }
    }

    void SetDate(int index)
    {
        string date = scuns.ScoreData_[index]["date"].ToString();
        string[] dates = date.Split(" ");
        Date.text = $"{dates[1]}�� {dates[2]}��\n����Ʈ ���";
        int textSize = adviceTxt.fontSize;
        adviceTxt.text = $"{PlayerPrefs.GetString("UserName")} ���� \n������ ��Ÿ�� ������\n<color=#32438B><size={textSize+2}>{scuns.ScoreData_[index]["total"].ToString()}���̿���!</size></color>";
    }
}

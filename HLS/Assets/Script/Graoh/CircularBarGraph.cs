using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularBarGraph : MonoBehaviour
{
    [SerializeField]
    [Header("Script")]
    LineRen Ln;
    [Space(20f)]

    public int segments = 10; // 그래프의 세그먼트 수
    public float radius; // 원의 반지름
    public float animationSpeed = 1f; // 애니메이션 속도
    public Slider slider; // 슬라이더 UI
    public Text Title; // 항목title UI
    public RectTransform WheelPrent;

    public Slider[] bars;
    public Vector3[] handles;
    public int[] data;
    [Space (10f)]
    public GameObject Mysign;
    public Text scoreTxt;

    private RectTransform rectTransform;

    [Header("Data")]
    public Text[] Titles;
    public string[] TitleTxts;
    [Space(5f)]
    public Text Date;
    public string DateTxt;
    [Space(5f)]
    public int Score;



    void Start()
    {
    }

    public void buttonClick()
    {
        segments = data.Length;
        Score = plusScore();
        scoreTxt.text = Score.ToString();
        CreateBars();
        Ln.Points();
    }

    public void BackSpa()
    {
        Transform targetTransform = WheelPrent.transform;
        Ln.LineEnable();
        foreach (Transform child in targetTransform)
        {
            Destroy(child.gameObject);
        }
    }

    int plusScore()
    {
        int P =0;
        for(int i = 0; i<data.Length;i++)
        {
            P += data[i];
        }
        return P;
    }

    void CreateBars()
    {
        bars = new Slider[segments];
        handles = new Vector3[segments];

        float angleStep = 360f / segments;

        //날짜설정
        SetDate();

        for (int i = 0; i < segments; i++)
        {
            float angle = (i * angleStep)-90;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle+90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, transform.position + barPosition, Quaternion.identity, transform);
            bars[i] = bar;
            bar.value = data[i];
            //막대 사이즈 조절
            RectTransform thisRect = bar.GetComponent<RectTransform>();
            if (WheelPrent.rect.width > WheelPrent.rect.height)
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.height / 3)-10, thisRect.rect.height);
            else
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.width / 3)-10, thisRect.rect.height);
            // 막대의 회전 조절
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            handles[i] = bar.transform.Find("Handle Slide Area/Handle").position;

            //각 점수 별 title 생성
            Transform RT = bar.transform.Find("TitlePos").GetComponent<Transform>();
            SetTitles();

            void SetTitles()
            {
                /*Debug.Log(i);
                Debug.Log("앵글값" + angle);
                Debug.Log("떨어지는 값" + thisRect.rect.width / 10);*/
                bar.gameObject.name = i.ToString();
                Text Tt = Instantiate(Title, RT);
                Tt.text = TitleTxts[i];
                if (TitleTxts[i].Length > 6)
                {
                    //좌우 타이틀 위치 설정
                    if (angle < 35 && angle > -61)
                    {
                        Tt.transform.position = new Vector2(RT.position.x - 1.5f, RT.position.y);
                    }
                    else if (angle < 240 && angle > 140)
                    {
                        Tt.transform.position = new Vector2(RT.position.x + 2.5f, RT.position.y + 1.5f);
                    }
                }
                Tt.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

        }


        UpdateGraph();
    }

    void SetDate()
    {
        Date.text = DateTxt;
    }

    


    void Update()
    {
        //UpdateGraph();
    }

    void UpdateGraph()
    {
        int sign = 0;
        for (int i = 0; i < segments; i++)
            sign += data[i];
        if (sign < 92)
            Mysign.GetComponent<Image>().color = Color.red;
        else if (92 < sign && sign < 114)
            Mysign.GetComponent<Image>().color = Color.yellow;
        else if (114 < sign)
            Mysign.GetComponent<Image>().color = Color.green;
        //Debug.Log(sign);
    }
}

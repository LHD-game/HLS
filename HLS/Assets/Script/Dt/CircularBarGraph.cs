using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularBarGraph : MonoBehaviour
{
    [SerializeField]
    [Header("Script")]
    RaderDraw RD;
    //LineRen Ln;
    [Space(20f)]

    public int segments = 10; // 그래프의 세그먼트 수
    public float radius; // 원의 반지름
    public float animationSpeed = 1f; // 애니메이션 속도
    public Slider slider; // 슬라이더 UI

    public RectTransform WheelPrent;

    public Slider[] bars;
    [Space (10f)]
    public GameObject Mysign;
    //public Text scoreTxt;

    //private RectTransform rectTransform;

    //[Header("Data")]
    //public Text[] Titles;
   // public string[] TitleTxts;
    //[Space(5f)]
    //public int Score;



    void Start()
    {
    }

    public void buttonClick()
    {
        segments = RD.data.Length;
        CreateBars();
    }

    public void BackSpa()
    {
        Transform targetTransform = WheelPrent.transform;
        //Ln.LineEnable();
        foreach (Transform child in targetTransform)
        {
            Destroy(child.gameObject);
        }
    }

    void CreateBars()
    {
        //radius = RD.chart.Radius;
        bars = new Slider[segments];
        //handles = new Vector3[segments];

        float angleStep = 360f / segments;

        //날짜설정
        //SetDate();

        for (int i = 0; i < segments; i++)
        {
            float angle = (i * angleStep)-90;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle+90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, transform.position + barPosition, Quaternion.identity, transform);
            bars[i] = bar;
            //bar.value = data[i];
            //막대 사이즈 조절
            RectTransform thisRect = bar.GetComponent<RectTransform>();
            Debug.Log("H: " + WheelPrent.rect.height + " W: " + WheelPrent.rect.width);
            if (WheelPrent.rect.width > WheelPrent.rect.height)
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.height / 3), thisRect.rect.height);
            else
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.width / 3), thisRect.rect.height);
            // 막대의 회전 조절
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        }


        UpdateGraph();
    }

    void UpdateGraph()
    {
        for (int i = 0; i < segments; i++)
        if (RD.Score < 92)
            Mysign.GetComponent<Image>().color = Color.red;
        else if (92 < RD.Score && RD.Score < 114)
            Mysign.GetComponent<Image>().color = Color.yellow;
        else if (114 < RD.Score)
            Mysign.GetComponent<Image>().color = Color.green;
        //Debug.Log(sign);
    }
}

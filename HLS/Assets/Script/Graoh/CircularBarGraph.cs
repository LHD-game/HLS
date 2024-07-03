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

    public int segments = 10; // �׷����� ���׸�Ʈ ��
    public float radius; // ���� ������
    public float animationSpeed = 1f; // �ִϸ��̼� �ӵ�
    public Slider slider; // �����̴� UI
    public Text Title; // �׸�title UI
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

        //��¥����
        SetDate();

        for (int i = 0; i < segments; i++)
        {
            float angle = (i * angleStep)-90;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle+90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, transform.position + barPosition, Quaternion.identity, transform);
            bars[i] = bar;
            bar.value = data[i];
            //���� ������ ����
            RectTransform thisRect = bar.GetComponent<RectTransform>();
            if (WheelPrent.rect.width > WheelPrent.rect.height)
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.height / 3)-10, thisRect.rect.height);
            else
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.width / 3)-10, thisRect.rect.height);
            // ������ ȸ�� ����
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            handles[i] = bar.transform.Find("Handle Slide Area/Handle").position;

            //�� ���� �� title ����
            Transform RT = bar.transform.Find("TitlePos").GetComponent<Transform>();
            SetTitles();

            void SetTitles()
            {
                /*Debug.Log(i);
                Debug.Log("�ޱ۰�" + angle);
                Debug.Log("�������� ��" + thisRect.rect.width / 10);*/
                bar.gameObject.name = i.ToString();
                Text Tt = Instantiate(Title, RT);
                Tt.text = TitleTxts[i];
                if (TitleTxts[i].Length > 6)
                {
                    //�¿� Ÿ��Ʋ ��ġ ����
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

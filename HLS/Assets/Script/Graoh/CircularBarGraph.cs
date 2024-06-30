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
    public string[] Titles;
    public int Score;
    public string Date;



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

        for (int i = 0; i < segments; i++)
        {
            float angle = (i * angleStep)-90;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle+90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, transform.position + barPosition, Quaternion.identity, transform);
            bars[i] = bar;
            //���� ������ ����
            RectTransform thisRect = bar.GetComponent<RectTransform>();
            if (WheelPrent.rect.width > WheelPrent.rect.height)
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.height / 3)-10, thisRect.rect.height);
            else
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.width / 3)-10, thisRect.rect.height);
            // ������ ȸ�� ����
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            UpdateGraph(i);

            handles[i] = bar.transform.Find("Handle Slide Area/Handle").position;

            //�� ���� �� title ����
            Transform RT = bar.transform.Find("TitlePos").GetComponent<Transform>();
            Transform OutPos = RT;
            Debug.Log(i);
            Debug.Log("�ޱ۰�"+angle);
            Debug.Log("�������� ��"+ thisRect.rect.width / 10);
            bar.gameObject.name = i.ToString();
            
            //�¿� Ÿ��Ʋ ��ġ ����
            if(angle < 35 && angle > -61)
            {
                Debug.Log("��ȷ");
                OutPos.position = new Vector2(OutPos.position.x - 1.5f, OutPos.position.y);
            }
            else if(angle < 240 && angle > 140)
            {
                Debug.Log("��ȷ");
                OutPos.position = new Vector2(OutPos.position.x + 2.5f, OutPos.position.y);
            }

            Instantiate(Title, OutPos.position, Quaternion.identity, transform);
        }
    }

    void Update()
    {
        //UpdateGraph();
    }

    void UpdateGraph(int i)
    {
        int sign = 0;
        sign += data[i];
        bars[i].value = data[i];
        if (sign < 92)
            Mysign.GetComponent<Image>().color = Color.red;
        else if (92 < sign && sign < 114)
            Mysign.GetComponent<Image>().color = Color.yellow;
        else if (114 < sign)
            Mysign.GetComponent<Image>().color = Color.green;
        //Debug.Log(sign);
    }
}

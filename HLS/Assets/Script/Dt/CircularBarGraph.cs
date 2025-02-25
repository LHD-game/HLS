using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularBarGraph : MonoBehaviour
{
    [SerializeField]
    [Header("Script")]
    RaderDraw RD;
    [Space(20f)]

    public int segments = 10; // 그래프의 세그먼트 수
    public float radius; // 원의 반지름
    public float animationSpeed = 1f; // 애니메이션 속도
    public Slider slider; // 슬라이더 UI

    public RectTransform WheelPrent;

    public Slider[] bars;
    [Space (10f)]
    public GameObject Mysign;
    public void buttonClick()
    {
        segments = RD.data.Length;
        CreateBars();
    }

    public void BackSpa()
    {
        Transform targetTransform = WheelPrent.transform;
        foreach (Transform child in targetTransform)
        {
            Destroy(child.gameObject);
        }
    }

    void CreateBars()
    {
        bars = new Slider[segments];

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = (i * angleStep)-90;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle+90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, transform.position + barPosition, Quaternion.identity, transform);
            bars[i] = bar;

            //막대 사이즈 조절
            RectTransform thisRect = bar.GetComponent<RectTransform>();
            
            if (WheelPrent.rect.width > WheelPrent.parent.GetComponent<RectTransform>().rect.width)
                thisRect.sizeDelta = new Vector2((WheelPrent.rect.height / 3), thisRect.rect.height);
            else
                thisRect.sizeDelta = new Vector2((WheelPrent.parent.GetComponent<RectTransform>().rect.width / 3), thisRect.rect.height);
            
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
    }
}

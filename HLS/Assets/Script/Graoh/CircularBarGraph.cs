using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularBarGraph : MonoBehaviour
{
    public int segments = 10; // 그래프의 세그먼트 수
    public float radius; // 원의 반지름
    public float animationSpeed = 1f; // 애니메이션 속도
    public Slider slider; // 슬라이더 UI
    public RectTransform WheelPrent;

    private Slider[] bars;
    public int[] data;
    public GameObject Mysign;

    private RectTransform rectTransform;

    void Start()
    {
        segments = data.Length;
        CreateBars();
        UpdateGraph();
    }

    void CreateBars()
    {
        bars = new Slider[segments];

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            Vector3 barPosition = Quaternion.Euler(0, 0, angle+90) * Vector3.up * radius;

            Slider bar = Instantiate(slider, transform.position + barPosition, Quaternion.identity, transform);
            bars[i] = bar;
            //막대 사이즈 조절
            RectTransform thisRect = bar.GetComponent<RectTransform>();
            if (WheelPrent.rect.width > WheelPrent.rect.height)
                thisRect.sizeDelta = new Vector2(WheelPrent.rect.height / 2, thisRect.rect.height);
            else
                thisRect.sizeDelta = new Vector2(WheelPrent.rect.width / 2, thisRect.rect.height);
            // 막대의 회전 조절
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        }
    }

    void Update()
    {
        //UpdateGraph();
    }

    void UpdateGraph()
    {
        int sign = 0;
        for (int i = 0; i < segments; i++)
        {
            //float height = data[i];
            Slider bar = bars[i];
            sign += data[i];
            bar.value = data[i]/4;

        }
        if (sign < 92)
            Mysign.GetComponent<Image>().color = Color.red;
        else if (92<sign && sign < 114)
            Mysign.GetComponent<Image>().color = Color.yellow;
        else if (114<sign)
            Mysign.GetComponent<Image>().color = Color.green;
        Debug.Log(sign);
    }
}

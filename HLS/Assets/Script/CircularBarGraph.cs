using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularBarGraph : MonoBehaviour
{
    public int segments = 10; // 그래프의 세그먼트 수
    public float radius = 2f; // 원의 반지름
    public float animationSpeed = 1f; // 애니메이션 속도
    public Slider slider; // 슬라이더 UI

    private Slider[] bars;
    public int[] data;

    private RectTransform rectTransform;
    private Vector2 barSize = new Vector2(500f, 40.0f);

    void Start()
    {
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

            // 막대의 회전 조절
            bar.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            bar.GetComponent<RectTransform>().sizeDelta = barSize;

        }
    }

    void Update()
    {
        //UpdateGraph();
    }

    void UpdateGraph()
    {
        for (int i = 0; i < segments; i++)
        {
            //float height = data[i];
            Slider bar = bars[i];
            bar.value = data[i];

        }
    }
}

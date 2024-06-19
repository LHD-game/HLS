using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public class PieChartComponet : MonoBehaviour
{
    PieChartdraw m_PieChart;

    void Start()
    {
        m_PieChart = new PieChartdraw();
        GetComponent<UIDocument>().rootVisualElement.Add(m_PieChart);
    }
}

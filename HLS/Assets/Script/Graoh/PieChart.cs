using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PieChart : VisualElement
{
    float m_Radius = 100.0f;
    float m_Value = 70.0f;

    public float radius
    {
        get => m_Radius;
        set
        {
            m_Radius = value;
        }
    }

    public float diameter => m_Radius * 2.0f;

    public float value
    {
        get { return m_Value; }
        set { m_Value = value; MarkDirtyRepaint(); }
    }

    public PieChart()
    {
        generateVisualContent += DrawCanvas;
    }

    void DrawCanvas(MeshGenerationContext ctx)
    {
        var painter = ctx.painter2D;
        painter.strokeColor = Color.white;
        painter.fillColor = Color.white;
        Vector2 ScreenCenter;

        //painter.MoveTo(new Vector2(100, 100));
        
        ScreenCenter = new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2);
        Debug.Log(ScreenCenter.x+", "+ ScreenCenter.y);


        var percentages = new float[] {
            11.1f,11.1f,11.1f,11.1f,11.1f,11.1f,11.1f,11.1f,11.1f
        };
        var colors = new Color32[] {
            Color.black,
            new Color32(52,235,122,255),
            new Color32(182,75,160,255),
            new Color32(102,25,122,255),
            new Color32(72,106,122,255),
            new Color32(102,235,122,255),
            new Color32(202,205,22,255),
            new Color32(82,45,122,255),
            Color.white
        };
        var valus = new int[]
        {
            1,2,3,1,2,3,1,2,3,1
        };

        float angle = 0.0f;
        float anglePct = 0.0f;
        int k = 0;
        int move = 50;

        foreach (var pct in percentages)
        {
            anglePct += 360.0f * (pct / 100);
            painter.fillColor = colors[k];
            painter.lineWidth = 2.0f;
            painter.strokeColor = Color.red;
            painter.BeginPath();
            painter.MoveTo(new Vector2(m_Radius+move, m_Radius + move));
            painter.Arc(new Vector2(m_Radius + move, m_Radius + move), (m_Radius) / valus[k], angle, anglePct);
            painter.Fill();
            painter.Stroke();

            angle = anglePct;
            k++;
        }
    }
}

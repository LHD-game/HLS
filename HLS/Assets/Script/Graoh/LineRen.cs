using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRen : MonoBehaviour
{
    private float lineWidth = 0.3f;
    private LineRenderer lr;
    private Vector3[] linePoints;

    public CircularBarGraph CB;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        lr.widthMultiplier = lineWidth;
    }

    public void Points()
    {
        linePoints = new Vector3[CB.segments];
        for (int i = 0; i < CB.segments; i++)
        {

            linePoints[i] = CB.handles[i];
        }
        lr.positionCount = linePoints.Length;

        drawLine();
    }

    public void LineEnable()
    {
        lr.enabled = false;
    }

    void drawLine()
    {
        lr.enabled = true;
        lr.SetPositions(linePoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

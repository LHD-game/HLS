using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DrewArc : VisualElement
{
    public Vector2 p0, p1, p2, p3;

    Color wireColor = Color.black;

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var mesh = mgc.Allocate(4, 6);

        mesh.SetNextVertex(new Vertex() { position = new Vector3(p0.x, p0.y, Vertex.nearZ), tint = wireColor });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(p1.x, p1.y, Vertex.nearZ), tint = wireColor });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(p2.x, p2.y, Vertex.nearZ), tint = wireColor });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(p3.x, p3.y, Vertex.nearZ), tint = wireColor });

        mesh.SetNextIndex(0);
        mesh.SetNextIndex(1);
        mesh.SetNextIndex(2);
        mesh.SetNextIndex(0);
        mesh.SetNextIndex(2);
        mesh.SetNextIndex(3);
    }

    void DrawCanvas(MeshGenerationContext mgc)
    {
        var painter2D = mgc.painter2D;

        /*painter2D.fillColor = wireColor;

        painter2D.BeginPath();
        painter2D.MoveTo(p0);
        painter2D.LineTo(p1);
        painter2D.LineTo(p2);
        painter2D.LineTo(p3);
        painter2D.ClosePath();
        painter2D.Fill();

        painter2D.lineWidth = 2.0f;
        painter2D.strokeColor = Color.red;
        painter2D.fillColor = Color.blue;*/

        painter2D.lineWidth = 2.0f;
        painter2D.strokeColor = Color.red;
        painter2D.fillColor = Color.blue;

        painter2D.BeginPath();
        // Move to the arc center
        painter2D.MoveTo(new Vector2(100, 100));

        // Draw the arc, and close the path
        painter2D.Arc(new Vector2(100, 100), 50.0f, 10.0f, 95.0f);
        painter2D.ClosePath();

        // Fill and stroke the path
        painter2D.Fill();
        painter2D.Stroke();

        painter2D.BeginPath();
        painter2D.MoveTo(new Vector2(100, 100));
        painter2D.ArcTo(new Vector2(150, 150), new Vector2(200, 100), 20.0f);
        painter2D.LineTo(new Vector2(200, 100));
        painter2D.Stroke();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}

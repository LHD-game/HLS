using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class PieChartWindow : EditorWindow
{
    [MenuItem("Tools/PieChart Window")]
    public static void ShowExample()
    {
        PieChartWindow wnd = GetWindow<PieChartWindow>();
        wnd.titleContent = new GUIContent("PieChartWindow");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        root.Add(new PieChartdraw());
    }
}

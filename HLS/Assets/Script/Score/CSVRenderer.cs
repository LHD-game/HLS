using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class CSVRenderer : MonoBehaviour
{
    public List<Dictionary<string, object>> SolutionData = new List<Dictionary<string, object>>();

    void Start()
    {
        string FileAdress = "CsvFile/bestSolution";
        SolutionData = CSVreader.Read(FileAdress);
    }

}

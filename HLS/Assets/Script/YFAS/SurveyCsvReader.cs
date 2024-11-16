using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
//using UnityEngine.iOS;

public class SurveyCsvReader : MonoBehaviour
{
    public string fileName;
    public string filePath;
    public string csvFile;
    public int dataCount;


    public List<string[]> csvData = new List<string[]>(); // CSV ������ ���� ����

    public void SetFiles()
    {
        //Debug.Log("SetFiles() �޼��� ȣ��");
        csvData.Clear();
        filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        filePath += ".csv";
        csvFile = fileName + ".csv";
        Debug.Log(csvFile);
        StartCoroutine(ReadCSV(filePath));


        // CSV �����Ͱ� ����� �ε�Ǿ����� �α� ���
        dataCount = csvData.Count;
        Debug.Log("CSV Data Loaded:");
        for (int i = 0; i < dataCount; i++)
        {
            //Debug.Log("Row " + i + ": " + string.Join(",", csvData[i]));
            //Debug.Log(csvData[i]);
        }
        Debug.Log($"CSV ���Ͽ��� {dataCount}���� ������ �ε�Ǿ����ϴ�.");
    }



    IEnumerator ReadCSV(string filePath)
    {
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            StringReader stringReader = new StringReader(www.text);
            while (stringReader.Peek() != -1)
            {
                string line = stringReader.ReadLine();
                csvData.Add(line.Split(','));
            }
        }
        else
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    csvData.Add(line.Split(','));
                }
            }
        } 
    }
}

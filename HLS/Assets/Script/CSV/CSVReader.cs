using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public string fileName = "questions.csv"; // CSV 파일 이름
    public List<string[]> csvData = new List<string[]>(); // CSV 데이터 저장 변수

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        StartCoroutine(ReadCSV(filePath));
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

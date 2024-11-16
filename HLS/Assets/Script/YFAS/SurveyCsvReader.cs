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


    public List<string[]> csvData = new List<string[]>(); // CSV 데이터 저장 변수

    public void SetFiles()
    {
        //Debug.Log("SetFiles() 메서드 호출");
        csvData.Clear();
        filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        filePath += ".csv";
        csvFile = fileName + ".csv";
        Debug.Log(csvFile);
        StartCoroutine(ReadCSV(filePath));


        // CSV 데이터가 제대로 로드되었는지 로그 출력
        dataCount = csvData.Count;
        Debug.Log("CSV Data Loaded:");
        for (int i = 0; i < dataCount; i++)
        {
            //Debug.Log("Row " + i + ": " + string.Join(",", csvData[i]));
            //Debug.Log(csvData[i]);
        }
        Debug.Log($"CSV 파일에서 {dataCount}개의 질문이 로드되었습니다.");
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

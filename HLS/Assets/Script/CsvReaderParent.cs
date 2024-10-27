using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;

public abstract class CsvReaderParent : MonoBehaviour
{
    public string fileName;// CSV 파일 이름
    public List<string[]> csvData = new List<string[]>(); // CSV 데이터를 저장할 리스트

    public void StartLoadCsvData(string buttonName)
    {
        csvData.Clear();
        StartCoroutine(DoStartLoadCsvData($"{buttonName}.csv"));
    }

    protected abstract IEnumerator DoStartLoadCsvData(string fileName);

    public IEnumerator ReadCSV(string filePath)
    {
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string text = www.downloadHandler.text;
                StringReader stringReader = new StringReader(text);
                while (stringReader.Peek() != -1)
                {
                    string line = stringReader.ReadLine();
                    csvData.Add(line.Split(','));
                }
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

        Debug.Log($"CSV 파일에서 {csvData.Count}개의 질문이 로드되었습니다.");
    }
}

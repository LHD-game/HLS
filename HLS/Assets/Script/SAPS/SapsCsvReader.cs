using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SapsCsvReader : MonoBehaviour
{
    public string fileName = "SAPS.csv"; // SAPS용 CSV 파일 이름
    public List<string[]> csvData = new List<string[]>(); // CSV 데이터를 저장할 리스트
    private int maxSapsQuestions = 15; // SAPS 검사에서 읽어들일 최대 질문 수

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        StartCoroutine(ReadCSV(filePath)); // CSV 파일 읽기
    }

    IEnumerator ReadCSV(string filePath)
    {
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            StringReader stringReader = new StringReader(www.text);
            int lineCount = 0;
            while (stringReader.Peek() != -1 && lineCount <= maxSapsQuestions)
            {
                string line = stringReader.ReadLine();
                csvData.Add(line.Split(',')); // 콤마로 구분된 데이터를 배열로 저장
                lineCount++;
            }
        }
        else
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                int lineCount = 0;
                while (!sr.EndOfStream && lineCount <= maxSapsQuestions)
                {
                    string line = sr.ReadLine();
                    csvData.Add(line.Split(',')); // 콤마로 구분된 데이터를 배열로 저장
                    lineCount++;
                }
            }
        }

        Debug.Log($"CSV 파일에서 {csvData.Count}개의 질문이 로드되었습니다.");
    }
}

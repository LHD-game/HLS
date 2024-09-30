using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RcbCsvReader : MonoBehaviour
{
    public string fileName = "RCBS_Questions.csv"; // RCBS용 CSV 파일 이름
    public List<string[]> csvData = new List<string[]>(); // CSV 데이터를 저장할 리스트
    private int maxRCBSQuestions = 6; // RCBS 검사에서 읽어들일 최대 질문 수 (6개의 질문)

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
            while (stringReader.Peek() != -1 && lineCount <= maxRCBSQuestions)
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
                while (!sr.EndOfStream && lineCount <= maxRCBSQuestions)
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

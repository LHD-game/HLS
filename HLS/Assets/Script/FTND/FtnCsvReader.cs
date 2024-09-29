using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FtnCsvReader : MonoBehaviour
{
    public string fileName = "FTND.csv"; // FTND�� CSV ���� �̸�
    public List<string[]> csvData = new List<string[]>(); // CSV �����͸� ������ ����Ʈ
    private int maxFTNDQuestions = 6; // FTND �˻翡�� �о���� �ִ� ���� �� (6�����)

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        StartCoroutine(ReadCSV(filePath)); // CSV ���� �б�
    }

    IEnumerator ReadCSV(string filePath)
    {
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            StringReader stringReader = new StringReader(www.text);
            int lineCount = 0;
            while (stringReader.Peek() != -1 && lineCount <= maxFTNDQuestions)
            {
                string line = stringReader.ReadLine();
                csvData.Add(line.Split(',')); // �޸��� ���е� �����͸� �迭�� ����
                lineCount++;
            }
        }
        else
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                int lineCount = 0;
                while (!sr.EndOfStream && lineCount <= maxFTNDQuestions)
                {
                    string line = sr.ReadLine();
                    csvData.Add(line.Split(',')); // �޸��� ���е� �����͸� �迭�� ����
                    lineCount++;
                }
            }
        }

        Debug.Log($"CSV ���Ͽ��� {csvData.Count}���� ������ �ε�Ǿ����ϴ�.");
    }
}

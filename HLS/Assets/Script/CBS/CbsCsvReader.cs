using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

public class CbsCsvReader : CsvReaderParent
{
    private CbsQuestionRenderer questionRenderer;

    private void Awake()
    {
        questionRenderer = GetComponent<CbsQuestionRenderer>();
    }

    protected override IEnumerator DoStartLoadCsvData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        yield return StartCoroutine(ReadCSV(filePath)); // CSV 파일 읽기

        if (csvData.Count > 0)
        {
            questionRenderer.StartQuestion();
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RcbCsvReader : CsvReaderParent
{
    private RcbQuestionRenderer questionRenderer;

    private void Awake()
    {
        questionRenderer = GetComponent<RcbQuestionRenderer>();
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
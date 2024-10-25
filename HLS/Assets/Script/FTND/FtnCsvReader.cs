using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FtnCsvReader : CsvReaderParent
{
    private FtnQuestionRenderer questionRenderer;
    private void Awake()
    {
        questionRenderer = GetComponent<FtnQuestionRenderer>();
    }

    protected override IEnumerator DoStartLoadCsvData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        yield return StartCoroutine(ReadCSV(filePath)); // CSV ���� �б�

        if (csvData.Count > 0)
        {
            questionRenderer.StartQuestion();
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class YfasCsvReader : CsvReaderParent
{
    private YfasQuestionRenderer questionRenderer;

    private void Awake()
    {
        questionRenderer = GetComponent<YfasQuestionRenderer>();
    }

    protected override IEnumerator DoStartLoadCsvData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        yield return StartCoroutine(ReadCSV(filePath));

        if (csvData.Count > 0)
        {
            questionRenderer.StartQuestion();
        }
    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVreader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        //Debug.Log(lines[0]); //scriptNumber,scriptType,name,dialog,select1,select2,select3,select4,select5,image,비고,SoundEffect
        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE); //string
        //Debug.Log(header[0]); //scriptNumber
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", ""); //1,nomal,Himchan
                //Debug.Log(value);
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                //Debug.Log(finalvalue); //1,nomal,Himchan
                entry[header[j]] = finalvalue;
                //Debug.Log(entry[header[j]]); //1,nomal,Himchan
            }
            list.Add(entry);
            //Debug.Log(list[0]);
        }
        return list;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Localize : MonoBehaviour
{
    protected static SystemLanguage sl;
    protected static string lang;

    public static List<Dictionary<string, object>> data;


    public string GetValueFromCSV(int index)    // index´Â dictionary ±âÁØ
    {
        return data[index - 2]["value"].ToString();
    }
}

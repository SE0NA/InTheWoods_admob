using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localize : MonoBehaviour
{
    static protected SystemLanguage sl;
    static protected string lang;

    static public List<Dictionary<string, object>> data;


    public string GetValueFromCSV(int index)    // index�� dictionary ����
    {
        return data[index - 2]["value"].ToString();
    }
}

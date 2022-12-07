using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localize : MonoBehaviour
{
    static protected SystemLanguage sl;
    static protected string lang;

    static protected List<Dictionary<string, object>> data;

    private void Start()
    {
        Debug.Log(data[3]["value"].ToString());
    }

    public string GetValueFromCSV(int index)    // index�� dictionary ����
    {
        return data[index]["value"].ToString();
    }
}

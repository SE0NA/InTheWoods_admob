using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedText : Localize
{
    [Tooltip("csv파일의 순번")]
    [SerializeField] public int index;

    private TextMeshProUGUI mytext;

    private void Start()
    {
        mytext = GetComponent<TextMeshProUGUI>();
        switch (sl)
        {
            case SystemLanguage.English:
                mytext.fontSize += 10;  break;
            default:    break;
        }
        mytext.text = GetValueFromCSV(index);   // 0부터 시작하므로
    }

}

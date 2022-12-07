using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedText : Localize
{
    [Tooltip("csv������ ����")]
    [SerializeField] public int index;

    private TextMeshProUGUI mytext;

    private void Start()
    {
        mytext = GetComponent<TextMeshProUGUI>();
        mytext.text = GetValueFromCSV(index - 2);
    }

}

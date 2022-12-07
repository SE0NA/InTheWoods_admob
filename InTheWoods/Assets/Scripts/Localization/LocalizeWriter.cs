using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeWriter : Localize
{
    // Start is called before the first frame update
    void Awake()
    {
        sl = Application.systemLanguage;

        Debug.Log("writer " + (int)sl);

        switch (sl)
        {
            case SystemLanguage.Korean:
                lang = "kr"; break;
            case SystemLanguage.English:
                lang = "en"; break;
            default:
                lang = "en"; break;
        }
        data = CSVReader.Read("script_" + lang);
        Destroy(GetComponent<LocalizeWriter>());
    }
}

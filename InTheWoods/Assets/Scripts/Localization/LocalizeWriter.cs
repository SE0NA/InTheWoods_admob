using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeWriter : Localize
{
    void Awake()
    {
        sl = Application.systemLanguage;
        switch (sl)
        {
            case SystemLanguage.Korean:
                lang = "kr";    break;
            case SystemLanguage.Japanese:
                lang = "jp"; break;
            case SystemLanguage.English:
                lang = "en";    break;
            default:
                lang = "en";    break;
        }
        data = CSVReader.Read("script_" + lang);
        Destroy(GetComponent<LocalizeWriter>());
    }
}

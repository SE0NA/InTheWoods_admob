using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeWriter : Localize
{
    void Awake()
    {
        sl = Application.systemLanguage;
        // sl = SystemLanguage.Japanese;
        switch (sl)
        {
            case SystemLanguage.Korean:
                lang = "kr";    break;
            case SystemLanguage.English:
                lang = "en";    break;
            case SystemLanguage.Japanese:
                lang = "jp";    break;
            default:
                lang = "en";    break;
        }
        data = CSVReader.Read("script_" + lang);
        Destroy(GetComponent<LocalizeWriter>());
    }
}

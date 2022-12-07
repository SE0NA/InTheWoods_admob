using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localize : MonoBehaviour
{
    static Localize instance;
    static SystemLanguage sl;
    static string lang;

    List<Dictionary<string, object>> data;

    void Awake()
    {
        if (instance == null)
        {
            // 시스템 언어 설정 알기
            sl = Application.systemLanguage;
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

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

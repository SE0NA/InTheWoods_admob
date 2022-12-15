using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    List<string> missions;
    int count = 0;
    List<string> history;

    void Start()
    {
        missions = new List<string>();
        history = new List<string>();

        SystemLanguage sl = Application.systemLanguage;
        //SystemLanguage sl = SystemLanguage.Japanese;
        string file_lang = "";
        switch (sl)
        {
            case SystemLanguage.Korean:
                file_lang = "kr";   break;
            case SystemLanguage.Japanese:
                file_lang = "jp";   break;
            case SystemLanguage.English:
                file_lang = "en";   break;
            default:
                file_lang = "en";   break;
        }

        TextAsset file = Resources.Load("mission_" + file_lang) as TextAsset;
        StringReader reader = new StringReader(file.text);
        //   string[] file = File.ReadAllLines(path);
        if(file == null)
        {
            // 파일 존재X
            missions.Add("<color=white>[단어]</color>바나나");
            missions.Add("<color=white>[행위]</color>다리 떨기");
            missions.Add("<color=white>[단어]</color>주사위");

            Debug.Log("mission 파일 존재X");
        }

        string str = reader.ReadLine();
        while (str != null)
        {
            missions.Add(str);
            str = reader.ReadLine();
        }
        /*
        if (file.Length > 0)
            for (int i = 0; i < file.Length; i++)
                missions.Add(file[i]);
        else
        {
            missions.Add("<color=white>[단어]</color>바나나");
            missions.Add("<color=white>[행위]</color>다리 떨기");
            missions.Add("<color=white>[단어]</color>주사위");
        }
        */
        count = missions.Count;
    }
    
    public string NewMission()
    {
        int n = Random.Range(0, count);
        history.Add(missions[n]);

        return missions[n];
    }
    public string GetPreviousMission()
    {
        return history[history.Count - 1];
    }

    public List<string> GetMissionHistory() => history;
}

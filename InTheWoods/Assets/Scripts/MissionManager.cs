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
            // ���� ����X
            missions.Add("<color=white>[�ܾ�]</color>�ٳ���");
            missions.Add("<color=white>[����]</color>�ٸ� ����");
            missions.Add("<color=white>[�ܾ�]</color>�ֻ���");

            Debug.Log("mission ���� ����X");
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
            missions.Add("<color=white>[�ܾ�]</color>�ٳ���");
            missions.Add("<color=white>[����]</color>�ٸ� ����");
            missions.Add("<color=white>[�ܾ�]</color>�ֻ���");
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

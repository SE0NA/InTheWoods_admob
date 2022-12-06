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

        string path = "Assets/Resources/mission.txt";
        string[] file = File.ReadAllLines(path);
        if (file.Length > 0)
            for (int i = 0; i < file.Length; i++)
                missions.Add(file[i]);
        else
        {
            missions.Add("<color=white>[단어]</color>바나나");
            missions.Add("<color=white>[행위]</color>다리 떨기");
            missions.Add("<color=white>[단어]</color>주사위");
        }
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

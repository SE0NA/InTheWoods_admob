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
            missions.Add("<color=white>[�ܾ�]</color>�ٳ���");
            missions.Add("<color=white>[����]</color>�ٸ� ����");
            missions.Add("<color=white>[�ܾ�]</color>�ֻ���");
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

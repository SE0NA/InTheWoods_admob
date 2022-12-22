using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SetManager : MonoBehaviour
{
    // ���� ���� ������
    [SerializeField] List<GameObject> SetPrefabsList;
    GameObject thisSet;

    public PlayerData previousData;

    // ���� ��
    int _playerCount = 0;
    List<Player> _playerlist;
    List<Player> _wolflist;
    List<Player> _catlist;
    Player _whoswan;

    int _wolfCount;
    int _catCount;
    int _swanCount;

    string str_mission;

    GameManager _gm;
    MissionManager _mission;

    void Start()
    {
        _playerlist = new List<Player>();
        _wolflist = new List<Player>();
        _catlist = new List<Player>();

        Invoke("SetStart", 1f);

        // ���� ������ �б�
        LoadData();
    }
    void LoadData()
    {
        string filename = "previousData";
        string path = Application.persistentDataPath + "/" + filename + ".json";

        try
        {
            FileStream filestream = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] filedata = new byte[filestream.Length];
            filestream.Read(filedata, 0, filedata.Length);
            filestream.Close();
            string json = Encoding.UTF8.GetString(filedata);

            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            previousData = data;
        }
        catch
        {
            Debug.Log("load previous data file error");
        }
    }

    void SetStart()
    {
        thisSet = GameObject.Instantiate(SetPrefabsList[0], transform);
    }


    public void HideSetContent() => thisSet.SetActive(false);
    public void ShowSetContent() => thisSet.SetActive(true);

    public int Get_PlayerCount() => _playerCount;

    // Set 1: ���� �ο�
    public void Set_PlayerCount(int value) => _playerCount = value;
    
    // Set 2: 
    public void Active_Set2()
    {
        thisSet = GameObject.Instantiate(SetPrefabsList[1], transform);
    }
    
    public void Set_PlayerList(List<Player> list) => _playerlist = list;

    //Set 3:
    public void Active_Set3()
    {
        thisSet = GameObject.Instantiate(SetPrefabsList[2], transform);
    }
    public void SetRoleCount(int w, int c, int s)
    {
        _wolfCount = w;
        _catCount = c;
        _swanCount = s;
    }

    // Set 4:
    public void Active_Set4()
    {
        SetRoleOfAll();

        _mission = FindObjectOfType<MissionManager>();
        str_mission = _mission.NewMission();

        thisSet = GameObject.Instantiate(SetPrefabsList[3], transform);
    }
    public Player GetPlayerOn(int index)
    {
        return _playerlist[index];
    }
    public string GetFirstMission()
    {
        return str_mission;
    }
    private void SetRoleOfAll()
    {
        int selectCount = 0;
        int n = 0;

        // ���� ����
        while(selectCount < _wolfCount)
        {
            n = Random.Range(0, _playerCount);
            if (_playerlist[n].role == Role.empty)
            {
                _playerlist[n].role = Role.wolf;
                _wolflist.Add(_playerlist[n]);
                selectCount++;
            }
        }
        // ����� ����
        selectCount = 0;
        while(selectCount < _catCount)
        {
            n = Random.Range(0, _playerCount);
            if (_playerlist[n].role == Role.empty)
            {
                _playerlist[n].role = Role.cat;
                _catlist.Add(_playerlist[n]);
                selectCount++;
            }
        }
        // ���� ����
        selectCount = 0;
        while(selectCount < _swanCount)
        {
            n = Random.Range(0, _playerCount);
            if (_playerlist[n].role == Role.empty)
            {
                _playerlist[n].role = Role.swan;
                _whoswan = _playerlist[n];
                selectCount++;
            }
        }
        // ������ ����
        for(int i = 0; i < _playerCount; i++)
        {
            if (_playerlist[i].role == Role.empty)
            {
                n = Random.Range(0, 2);
                _playerlist[i].role = (Role)n;
            }
        }
    }
    public List<Player> GetRoleList(Role r) // ���� or �����
    {
        if (r == Role.wolf)
            return _wolflist;
        else
            return _catlist;

    }

    // Set �Ϸ�
    public void Active_Finish()
    {
        // ���� �÷��̾� ������ ���� -> JSON
        SaveData();

        _gm = FindObjectOfType<GameManager>();
        _gm.SetGameManager(_playerlist, _wolflist, _catlist, _whoswan);
    }
    private void SaveData()
    {
        PlayerData data = new PlayerData();
        
        data.player_count = _playerCount;
        
        List<string> names = new List<string>();
        for (int i = 0; i < _playerlist.Count; i++)
            names.Add(_playerlist[i].name);
        data.player_names = names.ToArray();

        data.count_wolf = _wolfCount;
        data.count_cat = _catCount;
        data.count_swan = _swanCount;

        // JSON ������ ���ڿ��� �Է� �Ϸ�
        string json = JsonUtility.ToJson(data);

        // ���� �����
        string filename = "previousData";
        string path = Application.persistentDataPath + "/" + filename + ".json";
        FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.Write);
        byte[] bytedata = Encoding.UTF8.GetBytes(json);
        filestream.Write(bytedata, 0, bytedata.Length);
        filestream.Close();
        Debug.Log("Save: " + json);
    }

    public void EndThis()
    {
        Destroy(gameObject);
    }
}

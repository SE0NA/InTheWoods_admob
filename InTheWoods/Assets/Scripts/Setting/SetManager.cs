using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetManager : MonoBehaviour
{
    // 게임 설정 프리펩
    [SerializeField] List<GameObject> SetPrefabsList;
    GameObject thisSet;

    // 설정 값
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
    }
    void SetStart()
    {
        thisSet = GameObject.Instantiate(SetPrefabsList[0], transform);
    }


    public void HideSetContent() => thisSet.SetActive(false);
    public void ShowSetContent() => thisSet.SetActive(true);

    public int Get_PlayerCount() => _playerCount;

    // Set 1: 참가 인원
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

        // 늑대 결정
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
        // 고양이 결정
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
        // 백조 결정
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
        // 나머지 결정
        for(int i = 0; i < _playerCount; i++)
        {
            if (_playerlist[i].role == Role.empty)
            {
                n = Random.Range(0, 2);
                _playerlist[i].role = (Role)n;
            }
        }
    }
    public List<Player> GetRoleList(Role r) // 늑대 or 고양이
    {
        if (r == Role.wolf)
            return _wolflist;
        else
            return _catlist;

    }

    // Set 완료
    public void Active_Finish()
    { 
        _gm = FindObjectOfType<GameManager>();
        _gm.SetGameManager(_playerlist, _wolflist, _catlist, _whoswan);
    }

    public void EndThis()
    {
        Destroy(gameObject);
    }
}

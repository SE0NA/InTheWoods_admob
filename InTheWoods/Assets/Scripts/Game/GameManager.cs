using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ���� �÷��̾� ����
    List<Player> _playerlist;
    List<Player> _wolflist;
    List<Player> _catlist;
    Player _whoswan;
    List<Player> _alivelist;

    // ���� ���� ����
    int day = 1;
    Player _whoDied;
    Player _catSelect;
    Player _wolfSelect;
    string _thisMission = "";
    string _lastMission = "";
    Player _whoVoted;
    bool _isWin;

    [SerializeField] List<GameObject> gamePrefabsList;

    GameObject thisStep;
    SetManager _setmanager;
    UIManager _uimanager;
    MissionManager _mission;
    AudioManager _audiomanager;

    void Start()
    {
        _setmanager = FindObjectOfType<SetManager>();
        _uimanager = FindObjectOfType<UIManager>();
        _mission = FindObjectOfType<MissionManager>();
        _audiomanager = FindObjectOfType<AudioManager>();
    }

    public void SetGameManager(List<Player> pl, List<Player> wl, List<Player> cl, Player sw)
    {
        _playerlist = pl;
        _wolflist = wl;
        _catlist = cl;
        _whoswan = sw;

        _alivelist = _playerlist.ToList<Player>();

        FindObjectOfType<SetManager>().EndThis();
        Debug.Log("Set 4 �Ϸ� > ���� ����");

        // ���̵� �ƿ�, ��
        _uimanager.UI_Fade_Out(true);
        Invoke("GameStartAnim", 3.5f);
    }
    void GameStartAnim()
    {
        _uimanager.UI_ActiveDay();
        _uimanager.UI_BackGround(0);
        Active_Step3();
    }

    public void HideGameContent()
    {
        _audiomanager.PlayAudioClip(0);
        if (_setmanager != null)
            _setmanager.HideSetContent();
        else
            thisStep.SetActive(false);
    }
    public void ShowGameContent()
    {
        _audiomanager.PlayAudioClip(1);
        if (_setmanager != null)
            _setmanager.ShowSetContent();
        else
            thisStep.SetActive(true);
    }

    // ���� ���� Steps
    // Step 1: ���� �� ���� ��� �ȳ�
    public void Active_Step1()
    {
        if (_whoDied == null)   // ���� ��� ����
            _audiomanager.PlayAudioClip(3);
        else
            _audiomanager.PlayAudioClip(4);

        thisStep = GameObject.Instantiate(gamePrefabsList[0], transform);
    }
    public Player GetWhoDiedLastNight()
    {
        return _whoDied;
    }


    // Step 2: ���� ���� ��� �ȳ� �� ��Ʈ ����
    public void Active_Step2()
    {
        if (EndCheck())
            GameEnd();
        else
        {
            SetMission();
            thisStep = GameObject.Instantiate(gamePrefabsList[1], transform);
        }
    }
    public Player GetCatSelect() => _catSelect;
    public int GetCatCount() => _catlist.Count;
    public Player GetWolfSelect() => _wolfSelect;
    public Player GetSwanSelect() => _whoswan.selected;
    public Player GetPlayerOn(int index) => _alivelist[index];
    public string GetLastMission() => _lastMission;
    public string GetThisMission() => _thisMission;


    // Step 3: ��ȭ
    public void Active_Step3()
    {
        _audiomanager.PlayAudioClip(2);
        thisStep = GameObject.Instantiate(gamePrefabsList[2], transform);
    }


    // Step 4: ��ǥ
    public void Active_Step4()
    {
        _uimanager.UI_BackGround(1);
        thisStep = GameObject.Instantiate(gamePrefabsList[3], transform);
    }
    public void SetWhoVoted(Player p) => _whoVoted = p;


    // Step 5: ���
    public void Active_Step5()
    {

        thisStep = GameObject.Instantiate(gamePrefabsList[4], transform);
    }
    public Player GetWhoVoted() => _whoVoted;
    public void ExecuteWho(Player p)
    {
        if (p == null)
            return;

        p.isAlive = false;
        _alivelist.Remove(p);
        if (p.role == Role.wolf)
            _wolflist.Remove(p);
        else if (p.role == Role.cat)
            _catlist.Remove(p);

        _whoVoted = null;
        _whoDied = null;
        _catSelect = null;
        _whoswan.selected = null;
        _wolfSelect = null;

        for (int i = 0; i < _alivelist.Count; i++)
            _alivelist[i].selected = null;
    }
    public void Step5_Audio_VotedWasWolf(bool wolf)
    {
        if (wolf)
            _audiomanager.PlayAudioClip(3);
        else
            _audiomanager.PlayAudioClip(4);
    }

    
    // Step 6: ���� ��� ����
    public void Active_Step6()
    {
        _uimanager.UI_BackGround(2);
        if (EndCheck())
            GameEnd();
        else
            thisStep = GameObject.Instantiate(gamePrefabsList[5], transform);
    }
    public void PlayerSelect(int i, Player p) => _alivelist[i].selected = p;


    // �Ϸ� ��
    public void Active_GoodNight()
    {
        _audiomanager.PlayAudioClip(1);
        _uimanager.UI_Sleep_Fade();
        SelectOneOfLastNight();
        Invoke("SetUIForNextDay", 2f);
    }
    void SelectOneOfLastNight()
    {
        int n;

        // ���밡 ������ ������ ���(����)
        n = Random.Range(0, _wolflist.Count);
        _wolfSelect = _wolflist[n].selected;
        _whoDied = _wolfSelect;

        // ����̰� ������ ������ ���(����)
        if (_catlist.Count > 0)
        {
            n = Random.Range(0, _catlist.Count);
            _catSelect = _catlist[n].selected;
        }

        // ������ ����κ��� ��ȣ�Ͽ�����
        if (_whoswan.isAlive && _whoswan.selected.id == _whoDied.id)
            _whoDied = null;
        else
        {
            _whoDied.isAlive = false;
            _alivelist.Remove(_whoDied);
        }
    }
    void SetUIForNextDay()
    {
        _uimanager.UI_SetDay(++day);
        _uimanager.UI_BackGround(0);
    }

    void SetMission()
    {
        _lastMission = _mission.GetPreviousMission();
        _thisMission = _mission.NewMission();
    }
    public int GetPlayerCount() => _alivelist.Count;


    // Game Check & End
    bool EndCheck()
    {
        // ���밡 ���� ó���� > �¸�
        if (_wolflist.Count == 0)
        {
            _isWin = true;
            return true;
        }
        // ���� �� == ���� ���� �� > �й�
        else if (_wolflist.Count == _alivelist.Count - _wolflist.Count)
        {
            _isWin = false;
            return true;
        }
        return false;
    }
    void GameEnd()
    {
        _uimanager.UI_End_Fade();
        Invoke("GameEndActive", 3.5f);
    }
    void GameEndActive()
    {
        thisStep = GameObject.Instantiate(gamePrefabsList[6], transform);
        if (_isWin)
            _audiomanager.PlayAudioClip(3);
        else
            _audiomanager.PlayAudioClip(4);
    }
    public int GetWholePlayerCount() => _playerlist.Count;
    public Player GetWholePlayerOn(int index) => _playerlist[index];
    public string GetMissionHistory()
    {
        string str = "";
        List<string> history = _mission.GetMissionHistory();
        for (int i = 0; i < history.Count; i++)
            str += history[i] + "\n";
        return str;
    }
    public int GetDay() => day;
    public int GetWolfListCount() => _wolflist.Count;

    public bool GetResult() => _isWin;
    
}

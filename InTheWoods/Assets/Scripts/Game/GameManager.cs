using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 게임 플레이어 정보
    List<Player> _playerlist;
    List<Player> _wolflist;
    List<Player> _catlist;
    Player _whoswan;
    List<Player> _alivelist;

    // 게임 진행 정보
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
        Debug.Log("Set 4 완료 > 게임 시작");

        // 페이드 아웃, 인
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

    // 게임 진행 Steps
    // Step 1: 전날 밤 죽은 사람 안내
    public void Active_Step1()
    {
        if (_whoDied == null)   // 죽은 사람 없음
            _audiomanager.PlayAudioClip(3);
        else
            _audiomanager.PlayAudioClip(4);

        thisStep = GameObject.Instantiate(gamePrefabsList[0], transform);
    }
    public Player GetWhoDiedLastNight()
    {
        return _whoDied;
    }


    // Step 2: 전날 선택 결과 안내 및 힌트 제공
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


    // Step 3: 대화
    public void Active_Step3()
    {
        _audiomanager.PlayAudioClip(2);
        thisStep = GameObject.Instantiate(gamePrefabsList[2], transform);
    }


    // Step 4: 투표
    public void Active_Step4()
    {
        _uimanager.UI_BackGround(1);
        thisStep = GameObject.Instantiate(gamePrefabsList[3], transform);
    }
    public void SetWhoVoted(Player p) => _whoVoted = p;


    // Step 5: 결과
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

    
    // Step 6: 동작 대상 선택
    public void Active_Step6()
    {
        _uimanager.UI_BackGround(2);
        if (EndCheck())
            GameEnd();
        else
            thisStep = GameObject.Instantiate(gamePrefabsList[5], transform);
    }
    public void PlayerSelect(int i, Player p) => _alivelist[i].selected = p;


    // 하루 끝
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

        // 늑대가 선택한 습격할 대상(랜덤)
        n = Random.Range(0, _wolflist.Count);
        _wolfSelect = _wolflist[n].selected;
        _whoDied = _wolfSelect;

        // 고양이가 선택한 감시할 대상(랜덤)
        if (_catlist.Count > 0)
        {
            n = Random.Range(0, _catlist.Count);
            _catSelect = _catlist[n].selected;
        }

        // 백조가 늑대로부터 보호하였는지
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
        // 늑대가 전부 처형됨 > 승리
        if (_wolflist.Count == 0)
        {
            _isWin = true;
            return true;
        }
        // 늑대 수 == 남은 동물 수 > 패배
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_end;
    [SerializeField] TextMeshProUGUI txt_wolf;
    [SerializeField] TextMeshProUGUI txt_cat;
    [SerializeField] TextMeshProUGUI txt_swan;
    [SerializeField] TextMeshProUGUI txt_mission;
    [SerializeField] TextMeshProUGUI txt_end_info;
    [SerializeField] Button btn;

    GameManager _gm;
    UIManager _uimanager;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _uimanager = FindObjectOfType<UIManager>();
        btn.interactable = false;

        if (_gm.GetResult())
            txt_end.text = "°ÔÀÓ ½Â¸®!";
        else
            txt_end.text = "<color=red>´Á´ë ½Â¸®!</color>";

        txt_wolf.text = "";
        txt_cat.text = "";
        Player p;
        for(int i = 0; i < _gm.GetWholePlayerCount(); i++)
        {
            p = _gm.GetWholePlayerOn(i);
            if (p.role == Role.wolf)
                txt_wolf.text += p.name + "\n";
            else if (p.role == Role.cat)
                txt_cat.text += p.name + "\n";
            else if (p.role == Role.swan)
                txt_swan.text = p.name;
        }

        txt_mission.text = "<¹Ì¼Ç>\n" + _gm.GetMissionHistory();

        txt_end_info.text = _gm.GetDay() + "ÀÏÂ÷\n\n"
            + "ÃÑ " + _gm.GetWholePlayerCount().ToString() + "¸¶¸®\n"
            + "»ýÁ¸ " + _gm.GetPlayerCount().ToString() + "¸¶¸®\n"
            + "´Á´ë " + _gm.GetWolfListCount().ToString() + "¸¶¸®";

        Invoke("Active_Btn", 1f);
    }
    void Active_Btn() => btn.interactable = true;

    public void OnClick_Btn()
    {
        btn.interactable = false;
        _uimanager.OnClick_PNHome_EndGame();
    }

}

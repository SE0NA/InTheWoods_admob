using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Set4_Manager : MonoBehaviour
{
    [Header ("아이콘")]
    [SerializeField] List<Sprite> list_icon;
    [SerializeField] List<Color> list_color;

    [Header ("오브젝트")]
    [SerializeField] GameObject pn_before;
    [SerializeField] GameObject pn_after;
    [SerializeField] TextMeshProUGUI txt_whosturn_before;
    [SerializeField] TextMeshProUGUI txt_whosturn_after;
    [SerializeField] TextMeshProUGUI txt_info;
    [SerializeField] Image img_icon;
    [SerializeField] Image img_bg;
    [SerializeField] Button btn;

    SetManager _setmanager;
    Animator anim;

    int turn = 0;

    void Start()
    {
        _setmanager = FindObjectOfType<SetManager>();
        anim = GetComponent<Animator>();

        SetPanel();
    }

    void SetPanel()
    {
        Player p = _setmanager.GetPlayerOn(turn);
        txt_whosturn_before.text = p.name;
        txt_whosturn_after.text = p.name;

        img_bg.color = list_color[(int)p.role];
        img_icon.sprite = list_icon[(int)p.role];

        if (p.role == Role.wolf)
        {
            txt_info.text = "밤에 누군가를 습격합니다.\n\n"
                + TeamList(_setmanager.GetRoleList(Role.wolf), p) + "\n\n"
                + "<color=yellow>미션</color>\n"
                + _setmanager.GetFirstMission();
        }

        else if (p.role == Role.cat)
            txt_info.text = "밤에 누군가를 감시합니다.\n\n"
                + TeamList(_setmanager.GetRoleList(Role.cat), p);

        else if (p.role == Role.swan)
            txt_info.text = "밤에 누군가를 보호합니다.";

        else if (p.role == Role.deer)
            txt_info.text = "모두를 의심하세요!";

        else
            txt_info.text = "모두를 의심하세요!";

    }
    string TeamList(List<Player> pl, Player me)
    {
        string msg = "<color=red>동료</color>  ";
        int n;

        if (pl.Count == 1)  // 동료 없음
            return msg + "-";

        for (n = 0; n < pl.Count; n++)
        {
            if (pl[n].id != me.id)
                msg += pl[n].name + " ";
        }

        return msg;
    }

    public void Touch_Panel()
    {
        pn_before.SetActive(false);
        pn_after.SetActive(true);
    }
    public void UnTouch_Panel()
    {
        pn_before.SetActive(true);
        pn_after.SetActive(false);
    }

    public void OnClick_Btn()
    {
        btn.interactable = false;

        turn++;

        if(turn >= _setmanager.Get_PlayerCount())
        {
            pn_after.SetActive(false);
            pn_before.SetActive(false);

            anim.Play("set4_fade_out");
            Invoke("EndThis", 1f);
        }
        else
        {
            pn_after.SetActive(false);
            pn_before.SetActive(true);

            Invoke("BtnUnLock", 1f);
            SetPanel();
        }
    }
    void EndThis()
    {
        _setmanager.Active_Finish();
        Destroy(gameObject);
    }

    void BtnUnLock()
    {
        btn.interactable = true;
    }

}

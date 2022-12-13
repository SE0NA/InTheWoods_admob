using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step6_Manager : MonoBehaviour
{
    [SerializeField] GameObject ob_before;
    [SerializeField] GameObject ob_after;
    [SerializeField] TextMeshProUGUI txt_name;
    [SerializeField] TextMeshProUGUI txt_info;
    [SerializeField] GameObject ob_content;
    [SerializeField] Button btn_1;
    [SerializeField] Button btn_2;
    [SerializeField] GameObject ob_prefab_tab;

    GameManager _gm;
    Animator anim;
    List<Step6_Tab> list_tab;

    int turn = 0;
    Player nowP;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        list_tab = new List<Step6_Tab>();

        ob_after.SetActive(false);

        GameObject tab;
        for(int i = 0; i < _gm.GetPlayerCount(); i++)
        {
            tab = GameObject.Instantiate(ob_prefab_tab, ob_content.transform);
            list_tab.Add(tab.GetComponent<Step6_Tab>());
            tab.GetComponent<Step6_Tab>().Initialize(_gm.GetPlayerOn(i));
            tab.GetComponent<Step6_Tab>().UnSelectThis();
        }

        SetPanel();
    }

    void SetPanel()
    {
        nowP = _gm.GetPlayerOn(turn);

        txt_name.text = nowP.name;

        if (nowP.role == Role.wolf)
            txt_info.text = _gm.GetLocalText(69);
        else if (nowP.role == Role.cat)
            txt_info.text = _gm.GetLocalText(70);
        else if (nowP.role == Role.swan)
            txt_info.text = _gm.GetLocalText(71);
        else
            txt_info.text = _gm.GetLocalText(68);

        btn_1.interactable = false;

        Invoke("Btn1UnLock", 1f);
    }
    void Btn1UnLock() => btn_1.interactable = true;
    

    public void OnClick_Btn_1()
    {
        ob_before.SetActive(false);
        ob_after.SetActive(true);
        btn_2.interactable = false;
    }

    public void SelectOne(Player p)
    {
        // ����: ���� ���� ����
        if(nowP.role == Role.swan)
        {
            _gm.PlayerSelect(turn, p);
            btn_2.interactable = true;
        }
        // �׿ܰ� �ٸ���� ����
        else if(nowP.id != p.id)
        {
            // ��Ʈ ���� && Ư�� ���� && ���� �� ���� -> ���� �Ұ�
            if (PlayerPrefs.GetInt("KnowEachOther") == 1 && (nowP.role == Role.cat || nowP.role == Role.wolf) && nowP.role == p.role)
            {
                btn_2.interactable = false;
            }
            else
            {
                _gm.PlayerSelect(turn, p);
                btn_2.interactable = true;
            }
        }
        else
        {
            // �� ���� ���: ���� ���� -> ���� �Ұ�
            btn_2.interactable = false;
        }

        for (int i = 0; i < list_tab.Count; i++)
            if (list_tab[i].me.id != p.id)
                list_tab[i].UnSelectThis();
    }

    public void OnClick_Btn_2()
    {
        btn_1.interactable = false;
        btn_2.interactable = false;

        ob_after.SetActive(false);
        ob_before.SetActive(true);
        
        turn++;
        if (turn >= _gm.GetPlayerCount())
        {
            ob_after.SetActive(false);

            anim.Play("step6_fade_out");
            Invoke("EndThis", 1f);
        }
        else
        {
            ob_after.SetActive(false);
            SetPanel();
            ob_before.SetActive(true);
            Invoke("Btn2UnLock", 1f);

        }

        for (int i = 0; i < list_tab.Count; i++)
            list_tab[i].UnSelectThis();
    }
    void EndThis()
    {
        _gm.Active_GoodNight();
        Destroy(gameObject);
    }
    void Btn2UnLock() => btn_2.interactable = true;
}

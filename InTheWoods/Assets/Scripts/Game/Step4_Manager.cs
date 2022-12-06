using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Step4_Manager : MonoBehaviour
{
    [SerializeField] GameObject ob_before;
    [SerializeField] GameObject ob_vote;
    [SerializeField] Button btn_pass;
    [SerializeField] Button btn_select_1;

    [SerializeField] GameObject ob_content;
    [SerializeField] Button btn_back;
    [SerializeField] Button btn_select_2;

    [SerializeField] GameObject ob_prefab_list;
    List<Step4_VoteTab> list_tab;

    Player selected;

    Animator anim;
    GameManager _gm;
    
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();

        ob_vote.SetActive(false);

        list_tab = new List<Step4_VoteTab>();
        GameObject tab;
        for(int i = 0; i < _gm.GetPlayerCount(); i++)
        {
            tab = GameObject.Instantiate(ob_prefab_list, ob_content.transform);
            list_tab.Add(tab.GetComponent<Step4_VoteTab>());
            tab.GetComponent<Step4_VoteTab>().Initialize(_gm.GetPlayerOn(i));
            tab.GetComponent<Step4_VoteTab>().UnSelectThis();
        }

        btn_pass.interactable = false;
        btn_select_1.interactable = false;
        btn_back.interactable = false;
        btn_select_2.interactable = false;

        Invoke("Active_Btn_1", 1f);
    }
    void Active_Btn_1()
    {
        btn_pass.interactable = true;
        btn_select_1.interactable = true;
    }

    public void OnClick_Btn_Pass()
    {
        selected = null;
        _gm.SetWhoVoted(selected);

        anim.Play("step4_fade_out");
        Invoke("EndThis", 1f);
    }

    public void OnClick_Btn_Select_1()
    {
        ob_before.SetActive(false);
        ob_vote.SetActive(true);
        btn_back.interactable = true;
        btn_select_2.interactable = false;
    }
    public void OnClick_Btn_Back()
    {
        ob_before.SetActive(true);
        ob_vote.SetActive(false);
        for (int i = 0; i < list_tab.Count; i++)
            list_tab[i].UnSelectThis();
        btn_select_2.interactable = false;

        selected = null;
    }
    public void OnClick_Btn_Select_2()
    {
        if(selected != null)
        {
            ob_vote.SetActive(false);
            btn_back.interactable = false;
            btn_select_2.interactable = false;
            btn_pass.interactable = false;
            btn_select_1.interactable = false;

            ob_before.SetActive(true);
            _gm.SetWhoVoted(selected);
            anim.Play("step4_fade_out");
            Invoke("EndThis", 1f);
        }
    }
    void EndThis()
    {
        _gm.Active_Step5();
        Destroy(gameObject);
    }

    public void WhoSelected(Player p)
    {
        selected = p;
        btn_select_2.interactable = true;
        for (int i = 0; i < list_tab.Count; i++)
            if (list_tab[i].me.id != selected.id)
                list_tab[i].UnSelectThis();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Set2_Manager : MonoBehaviour
{
    [SerializeField] GameObject ob_namefield;
    [SerializeField] GameObject ob_Content;
    [SerializeField] Button btn;

    List<Set2_NameField> list_namefield;
    Animator anim;

    SetManager _setmanager;

    void Start()
    {
        _setmanager = FindObjectOfType<SetManager>();
        anim = GetComponent<Animator>();

        list_namefield = new List<Set2_NameField>();

        GameObject thisNF;
        for(int i=0; i<_setmanager.Get_PlayerCount(); i++)
        {
            thisNF = GameObject.Instantiate(ob_namefield, ob_Content.transform);
            thisNF.GetComponent<Set2_NameField>().SetNameField(i + 1);
            list_namefield.Add(thisNF.GetComponent<Set2_NameField>());
        }
    }

    public void OnClick_BtnOK()
    {
        btn.interactable = false;

        List<Player> playerlist = new List<Player>();
        Player thisP;
        
        for(int i = 0; i < _setmanager.Get_PlayerCount(); i++)
        {
            thisP = list_namefield[i].GetPlayerInfo();
            playerlist.Add(thisP);
        }
        _setmanager.Set_PlayerList(playerlist);

        anim.Play("set2_fade_out");
        Invoke("EndThis", 1f);
    }

    void EndThis()
    {
        _setmanager.Active_Set3();
        Destroy(gameObject);
    }
}

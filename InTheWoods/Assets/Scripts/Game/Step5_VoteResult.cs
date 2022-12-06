using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step5_VoteResult : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_info;
    [SerializeField] GameObject ob_btn_1;
    [SerializeField] Button btn_1;
    [SerializeField] GameObject ob_btn_2;
    [SerializeField] Button btn_2;

    GameManager _gm;
    Player voted;
    Animator anim;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        voted = _gm.GetWhoVoted();
        anim = GetComponent<Animator>();

        if (voted == null)
        {
            txt_info.text = "<color=yellow>�ƹ���</color>\nó������\n�ʾҽ��ϴ�.";
            ob_btn_1.SetActive(false);
            ob_btn_2.SetActive(true);
            btn_2.interactable = false;
            Invoke("Active_Btn_2", 1f);
        }
        else
        {
            txt_info.text = "��ǥ ���,\n<color=yellow>" + voted.name + "</color> ��(��)\n"
            + "<color=red>ó��</color>�Ǿ����ϴ�.";

            btn_1.interactable = false;
            ob_btn_2.SetActive(false);
        }
        Invoke("Active_Btn_1", 1f);
    }
    void Active_Btn_1() => btn_1.interactable = true;

    public void OnClick_Btn_1()
    {
        bool wasWolf = (voted.role == Role.wolf) ? true : false;
        _gm.Step5_Audio_VotedWasWolf(wasWolf);

        ob_btn_1.SetActive(false);
        ob_btn_2.SetActive(true);
        btn_2.interactable = false;

        txt_info.text = "���� ���,\n" +
                "ó���� <color=yellow>" + voted.name + "</color> ��(��)\n";

        if (voted.role == Role.wolf)
            txt_info.text += "<color=red>����</color>�����ϴ�!";
        else
            txt_info.text += "���밡 �ƴմϴ�!";

        Invoke("Active_Btn_2", 1f);
    }
    void Active_Btn_2() => btn_2.interactable = true;

    public void OnClick_Btn_2()
    {
        anim.Play("step5_fade_out");
        _gm.ExecuteWho(voted);
        Invoke("EndThis", 1f);
    }
    void EndThis()
    {
        _gm.Active_Step6();
        Destroy(gameObject);
    }
}

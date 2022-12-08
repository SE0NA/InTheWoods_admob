using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step1_Manager : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] TextMeshProUGUI txt_info;

    GameManager _gm;
    Animator anim;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        btn.interactable = false;

        Player p = _gm.GetWhoDiedLastNight();
        if (p != null)
        {
            txt_info.text = _gm.GetLocalText(35) + "\n"
                + "<color=red>" + _gm.GetWhoDiedLastNight().name + "</color> " +
                _gm.GetLocalText(36);
        }
        else
        {
            txt_info.text = _gm.GetLocalText(37);
        }

        Invoke("Active_Btn", 2f);
    }
    void Active_Btn() => btn.interactable = true;

    public void OnClick_Btn()
    {
        btn.interactable = false;
        anim.Play("step1_fade_out");
        Invoke("EndThis", 1f);
    }
    void EndThis()
    {
        _gm.Active_Step2();
        Destroy(gameObject);
    }
    
}

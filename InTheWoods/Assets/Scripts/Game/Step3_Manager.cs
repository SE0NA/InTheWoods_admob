using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Step3_Manager : MonoBehaviour
{

    [SerializeField] Button btn;

    GameManager _gm;
    Animator anim;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        btn.interactable = false;
    }

    public void Active_Btn() => btn.interactable = true;

    public void OnClick_Btn()
    {
        anim.Play("step3_fade_out");
        Invoke("EndThis", 1f);
    }
    void EndThis()
    {
        _gm.Active_Step4();
        Destroy(gameObject);
    }
}

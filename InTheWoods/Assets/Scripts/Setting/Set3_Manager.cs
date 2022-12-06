using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Set3_Manager : MonoBehaviour
{
    [SerializeField] Slider wolf_slider;
    [SerializeField] Slider cat_slider;
    [SerializeField] Slider swan_slider;
    [SerializeField] Button btn;

    Animator anim;
    SetManager _setmanager;

    int playerCount;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _setmanager = FindObjectOfType<SetManager>();

        playerCount = _setmanager.Get_PlayerCount();

        wolf_slider.maxValue = playerCount / 3;
        wolf_slider.value = wolf_slider.maxValue;
        cat_slider.maxValue = (wolf_slider.maxValue > 1) ? 2 : 1;
        cat_slider.value = cat_slider.maxValue;
        swan_slider.maxValue = 1;
        swan_slider.value = swan_slider.maxValue;
    }

    public void OnClick_BtnOK()
    {
        wolf_slider.interactable = false;
        cat_slider.interactable = false;
        swan_slider.interactable = false;
        btn.interactable = false;

        _setmanager.SetRoleCount((int)wolf_slider.value, (int)cat_slider.value, (int)swan_slider.value);
        anim.Play("set3_fade_out");
        Invoke("EndThis", 1f);
    }

    void EndThis()
    {
        _setmanager.Active_Set4();
        Destroy(gameObject);
    }

}

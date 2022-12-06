using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Set1_Manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ui_text;
    [SerializeField] Slider ui_slider;
    [SerializeField] Button ui_btn;

    Animator anim;
    SetManager _setmanager;

    void Start()
    {
        _setmanager = FindObjectOfType<SetManager>();
        anim = GetComponent<Animator>();
    }

    public void SetText()
    {
        ui_text.text = ui_slider.value.ToString();
    }

    public void OnClick_BtnOK()
    {
        ui_slider.interactable = false;
        ui_btn.interactable = false;
        _setmanager.Set_PlayerCount((int)ui_slider.value);

        Invoke("EndThis", 1f);
        anim.Play("set1_fade_out");
    }

    void EndThis()
    {
        _setmanager.Active_Set2();
        Destroy(gameObject);
    }
}
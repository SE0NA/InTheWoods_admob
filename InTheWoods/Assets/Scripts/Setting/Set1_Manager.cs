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
    [SerializeField] Button loadDataBtn;

    Animator anim;
    SetManager _setmanager;

    void Start()
    {
        _setmanager = FindObjectOfType<SetManager>();
        anim = GetComponent<Animator>();

        if (_setmanager.previousData == null)
            loadDataBtn.interactable = false;
        else
            loadDataBtn.interactable = true;
    }

    public void SetText()
    {
        ui_text.text = ui_slider.value.ToString();
    }

    public void OnClick_BtnOK()
    {
        loadDataBtn.interactable = false;
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

    public void LoadBtn()
    {
        ui_slider.value = _setmanager.previousData.player_count;
    }
}
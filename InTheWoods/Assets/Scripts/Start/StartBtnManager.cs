using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartBtnManager : MonoBehaviour
{
    // Start 씬의 버튼 동작
    [SerializeField] Button startbtn;
    [SerializeField] GameObject ob_btns;
    [SerializeField] GameObject pn_option;
    [SerializeField] Toggle tg_option_1;
    [SerializeField] Toggle tg_option_2;
    [SerializeField] Slider sl_option_3;
    [SerializeField] GameObject prefab_fade;
    [SerializeField] List<Button> btnsOnMain;
    
    public Animator anim;
    AudioManager _audioManager;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _audioManager = FindObjectOfType<AudioManager>();

        // 옵션 창 UI 설정
        if (PlayerPrefs.HasKey("KnowEachOther"))    
        {
            if (PlayerPrefs.GetInt("KnowEachOther") == 1)
                tg_option_1.isOn = true;
            else
                tg_option_1.isOn = false;
        }
        else
        {
            tg_option_1.isOn = true;
            PlayerPrefs.SetInt("KnowEachOther", 1);
        }

        if (PlayerPrefs.HasKey("GetHint"))
        {
            if (PlayerPrefs.GetInt("GetHint") == 1)
                tg_option_2.isOn = true;
            else
                tg_option_2.isOn = false;
        }
        else
        {
            tg_option_2.isOn = true;
            PlayerPrefs.SetInt("GetHint", 1);
        }

        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            sl_option_3.value = PlayerPrefs.GetFloat("SoundVolume");
            _audioManager.SetVolume(sl_option_3.value);
        }
        else
        {
            sl_option_3.value = 1f;
            PlayerPrefs.SetFloat("SoundVolume", 1f);
        }
    }

    public void OnClick_StartBtn()
    {
        for (int i = 0; i < btnsOnMain.Count; i++)
            btnsOnMain[i].interactable = false;

        _audioManager.PlayAudioClip(1);
        GameStart();
    }
    void GameStart()
    {
        // 광고 시작
        GoogleMobileAdsScript ads = FindObjectOfType<GoogleMobileAdsScript>();
        ads.GameStart();    // 페이드 아웃 -> 광고 -> 게임 시작
    }
    public void FadeOutBeforeAds()
    {
        GameObject fade = GameObject.Instantiate(prefab_fade, transform.parent.transform);
        fade.GetComponent<Animator>().Play("fade_out");
    }
    public void FailedGameStart()
    {
        for (int i = 0; i < btnsOnMain.Count; i++)
            btnsOnMain[i].interactable = true;
    }
    public void SetActive_StartBtn(bool active)
    {
        startbtn.interactable = active;
    }

    public void OnClick_Option()
    {
        ob_btns.SetActive(false);
        pn_option.SetActive(true);
    }
    public void OnClick_Option_OK()
    {
        _audioManager.PlayAudioClip(0);
        pn_option.SetActive(false);
        ob_btns.SetActive(true);
    }
    
    public void Toggle_KnowEachOther(bool check)
    {
        if (check)
            PlayerPrefs.SetInt("KnowEachOther", 1);
        else
            PlayerPrefs.SetInt("KnowEachOther", 0);
    }
    public void Toggle_GetHint(bool check)
    {
        if (check)
            PlayerPrefs.SetInt("GetHint", 1);
        else
            PlayerPrefs.SetInt("GetHint", 0);
    }
    public void Slider_SoundVolume(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        _audioManager.SetVolume(sl_option_3.value);
    }

    public void OnClick_Quit()
    {
        _audioManager.PlayAudioClip(1);
        anim.Play("btn_fade_out");
        Invoke("Quit", 1f);
    }
    void Quit()
    {
        Application.Quit();
    }
}

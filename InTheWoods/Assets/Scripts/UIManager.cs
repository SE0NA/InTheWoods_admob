using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image img_bg;
    [SerializeField] List<Sprite> list_bg;
    [SerializeField] GameObject ob_day;
    [SerializeField] TextMeshProUGUI txt_day;
    [SerializeField] Button btn_gohome;
    [SerializeField] GameObject pn_gohome;
    [SerializeField] GameObject ob_fade;
    [SerializeField] GameObject ob_fade_end;
    [SerializeField] GameObject ob_fade_sleep;

    GameObject fade;
    [SerializeField] GameObject startfade;

    GameManager _gm;
    Localize _localizemanager;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _localizemanager = FindObjectOfType<Localize>();

        startfade.GetComponent<Animator>().Play("startfade_in");
        Invoke("Destroy_StartFade", 1f);
    }
    void Destroy_StartFade()
    {
        Destroy(startfade);
    }

    // 배경 이미지 변경
    public void UI_BackGround(int n)
    {
        img_bg.sprite = list_bg[n]; // 0: 아침, 1: 저녁, 2:밤
    }

    // 일차 관련 UI
    public void UI_ActiveDay()
    {
        ob_day.SetActive(true);
        UI_SetDay(1);
    }
    public void UI_SetDay(int n) => txt_day.text = _localizemanager.GetValueFromCSV(14) + " "
                                                  + n.ToString() + " " + _localizemanager.GetValueFromCSV(15);

    // 홈버튼 관련 UI
    public void OnClick_HomeBtn()
    {
        btn_gohome.interactable = false;
        _gm.HideGameContent();
        pn_gohome.SetActive(true);
    }
    public void OnClick_PNHome_EndGame()
    {
        UI_Fade_Out(false);
        Invoke("EndThis", 1f);
    }
    void EndThis()
    {
        SceneManager.LoadScene("Start");
    }
    public void OnClick_PNHome_Continue()
    {
        btn_gohome.interactable = true;
        pn_gohome.SetActive(false);
        _gm.ShowGameContent();
    }

    // 페이드 관련(생성 ~ 제거)
    public void UI_Fade_In()
    {
        fade = GameObject.Instantiate(ob_fade, transform);
        fade.GetComponent<Animator>().Play("fade_in");
        Invoke("Destroy_Fade", 1f);
    }
    public void UI_Fade_Out(bool game)
    {
        fade = GameObject.Instantiate(ob_fade, transform);
        if (game)
        {
            fade.GetComponent<Animator>().Play("fade_out_in");
            Invoke("Destroy_Fade", 5f);
        }
        else
        {
            fade.GetComponent<Animator>().Play("fade_out");
        }
    }

    public void UI_End_Fade()
    {
        fade = GameObject.Instantiate(ob_fade_end, transform);
        fade.GetComponent<Animator>().Play("fade_out_in");
        Invoke("Destroy_Fade", 5f);
    }

    public void UI_Sleep_Fade()
    {
        fade = GameObject.Instantiate(ob_fade_sleep, transform);
        fade.GetComponent<Animator>().Play("fade_out_in");
        Invoke("Destroy_Fade_And_NextDay", 5f);
    }

    void Destroy_Fade() => Destroy(fade);
    void Destroy_Fade_And_NextDay()
    {
        Destroy(fade);
        _gm.Active_Step1();
    }
}

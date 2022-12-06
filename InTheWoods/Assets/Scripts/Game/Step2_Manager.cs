using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step2_Manager : MonoBehaviour
{
    [SerializeField] GameObject pn_before;
    [SerializeField] TextMeshProUGUI txt_before_name;
    [SerializeField] GameObject pn_after;
    [SerializeField] TextMeshProUGUI txt_after_name;
    [SerializeField] TextMeshProUGUI txt_after_info;
    [SerializeField] Button btn;

    GameManager _gm;
    Animator anim;

    int hintRatio = 30;

    int turn = 0;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();

        SetPanel();
    }

    public void SetPanel()
    {
        Player p = _gm.GetPlayerOn(turn);
        txt_before_name.text = p.name;
        txt_after_name.text = p.name;

        // 늑대
        if(p.role == Role.wolf)
        {
            txt_after_info.text = "이번 미션 단어 입니다.\n\n"
                + "<color=yellow>" + _gm.GetThisMission() + "</color>";
        }
        // 고양이
        else if(p.role == Role.cat)
        {
            txt_after_info.text = "<color=yellow>" + _gm.GetCatSelect().name + "</color>은\n\n";
            if (_gm.GetCatSelect().role == Role.wolf)
                txt_after_info.text += "<color=red>늑대</color>입니다!";
            else
                txt_after_info.text += "늑대가 아닙니다!";
        }
        // 그 외(백조, 사슴, 토끼)
        else
        {
            int n = Random.Range(0, 100);
            if (n < hintRatio && PlayerPrefs.GetInt("GetHint") == 1)   // 힌트 발생
            {
                n = Random.Range(0, 50);
                // 1) 죽을 뻔한 대상 2) 늑대의 이전 미션
                if (n < 10)
                {
                    if (_gm.GetWhoDiedLastNight() == null) // 죽은 사람 없음
                        txt_after_info.text = "어젯밤, 늑대가\n"
                            + "<color=yellow>" + _gm.GetWolfSelect().name + "<color> 에게\n"
                            + "습격을 시도했습니다!";
                    else
                    {
                        txt_after_info.text = "어제의 늑대의 미션 단어입니다!\n\n"
                            + "<color=yellow>" + _gm.GetLastMission() + "</color>";
                    }
                }
                // 3) 백조가 선택했던 사람(백조 제외)
                else if (n < 25 && p.role != Role.swan)
                {
                    txt_after_info.text = "어젯밤, 백조는\n"
                        + "<color=yellow>" + _gm.GetSwanSelect().name + "</color> 을(를)\n"
                        + "보호하였습니다!";
                }
                // 4) 남은 고양이의 수
                else
                {
                    txt_after_info.text = "남은 고양이는\n"
                        + "<color=yellow>" + _gm.GetCatCount().ToString() + "</color>마리입니다.";
                }
            }
            // 힌트 없음
            else
            {
                txt_after_info.text = "모두를 의심하세요!";
            }
        }

        Invoke("BtnActive", 5f);
    }
    void BtnActive() => btn.interactable = true;

    public void Touch_Panel()
    {
        pn_before.SetActive(false);
        pn_after.SetActive(true);
    }
    public void UnTouch_Panel()
    {
        pn_before.SetActive(true);
        pn_after.SetActive(false);
    }

    public void OnClick_Btn()
    {
        pn_after.SetActive(false);
        pn_before.SetActive(true);
        btn.interactable = false;

        turn++;

        if(turn >= _gm.GetPlayerCount())
        {
            pn_after.SetActive(false);
            pn_before.SetActive(false);

            anim.Play("step2_fade_out");
            Invoke("EndThis", 1f);
        }
        else
        {
            pn_after.SetActive(false);
            pn_before.SetActive(true);

            Invoke("BtnUnLock", 3f);
            SetPanel();
        }
    }
    void EndThis()
    {
        _gm.Active_Step3();
        Destroy(gameObject);
    }
    void BtnUnLock()
    {
        btn.interactable = true;
    }
}

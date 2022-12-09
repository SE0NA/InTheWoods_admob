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

        btn.interactable = false;
        SetPanel();
    }

    public void SetPanel()
    {
        Player p = _gm.GetPlayerOn(turn);
        txt_before_name.text = p.name;
        txt_after_name.text = p.name;

        // ����
        if(p.role == Role.wolf)
        {
            txt_after_info.text = _gm.GetLocalText(40) + "\n\n"
                + "<color=yellow>" + _gm.GetThisMission() + "</color>";
        }
        // �����
        else if(p.role == Role.cat)
        {
            txt_after_info.text = "<color=yellow>" + _gm.GetCatSelect().name + "</color> ";
            if (_gm.GetCatSelect().role == Role.wolf)
                txt_after_info.text += _gm.GetLocalText(41);
            else
                txt_after_info.text += _gm.GetLocalText(42);
        }
        // �� ��(����, �罿, �䳢)
        else
        {
            int n = Random.Range(0, 100);
            if (n < hintRatio && PlayerPrefs.GetInt("GetHint") == 1)   // ��Ʈ �߻�
            {
                n = Random.Range(0, 50);
                // 1) ���� ���� ��� 2) ������ ���� �̼�
                if (n < 10)
                {
                    if (_gm.GetWhoDiedLastNight() == null) // ���� ��� ����
                        txt_after_info.text = _gm.GetLocalText(43) + "\n"
                            + "<color=yellow>" + _gm.GetWolfSelect().name + "</color> "
                            + _gm.GetLocalText(44);
                    else
                    {
                        txt_after_info.text = _gm.GetLocalText(45) + "\n\n"
                            + "<color=yellow>" + _gm.GetLastMission() + "</color>";
                    }
                }
                // 3) ������ �����ߴ� ���(���� ����), ���������� ������ �־����.
                else if (n < 35 && p.role != Role.swan && _gm._whoswan != null)
                {
                    if (_gm.IsSwanAlive())  // ������ ����ִ� ���
                    {
                        txt_after_info.text = _gm.GetLocalText(46) + "\n"
                            + "<color=yellow>" + _gm.GetSwanSelect().name + "</color> "
                            + _gm.GetLocalText(47);
                    }
                    else
                    {
                        txt_after_info.text = _gm.GetLocalText(80);
                    }
                }
                // 4) ���� ������� ��
                else
                {
                    txt_after_info.text = _gm.GetLocalText(48) + "\n"
                        + "<color=yellow>" + _gm.GetCatCount().ToString() + "</color> "
                        + _gm.GetLocalText(49);
                }
            }
            // ��Ʈ ����
            else
            {
                txt_after_info.text = _gm.GetLocalText(50);
            }
        }

        Invoke("BtnActive", 3f);
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

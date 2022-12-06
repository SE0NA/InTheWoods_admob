using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Step6_Tab : MonoBehaviour
{
    [SerializeField] Image img_bg;
    [SerializeField] List<Color> list_color_bg;
    [SerializeField] TextMeshProUGUI txt_name;
    [SerializeField] List<Color> list_color_txt;

    public Player me;
    Step6_Manager _manager;
    

    public void Initialize(Player p)
    {
        _manager = FindObjectOfType<Step6_Manager>();
        me = p;
        txt_name.text = me.name;
    }

    public void SelectThis()
    {
        _manager.SelectOne(me);
        img_bg.color = list_color_bg[1];
        txt_name.color = list_color_txt[1];
    }
    public void UnSelectThis()
    {
        img_bg.color = list_color_bg[0];
        txt_name.color = list_color_txt[0];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Set2_NameField : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_id;
    [SerializeField] TMP_InputField if_name;

    int id;

    public void SetNameField(int num)
    {
        id = num;
        txt_id.text = id.ToString();
        if_name.placeholder.GetComponent<TextMeshProUGUI>().text = id.ToString() + "��...";
    }

    public Player GetPlayerInfo()
    {
        Player p = new Player();

        p.id = id;
        p.name = "";

        if (if_name.text == "")
            p.name = id.ToString() + "��";
        else
            p.name = if_name.text;

        return p;
    }
}

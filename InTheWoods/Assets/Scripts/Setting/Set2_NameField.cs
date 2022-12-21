using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Set2_NameField : Localize
{
    [SerializeField] TextMeshProUGUI txt_id;
    [SerializeField] public TMP_InputField if_name;

    int id;


    public void SetNameField(int num)
    {
        id = num;
        txt_id.text = id.ToString();

        // 한국어 사용자: 기본 키보드. 숫자, 영어, 한글 입력 가능
        if (Application.systemLanguage == SystemLanguage.Korean)
        {
            if_name.keyboardType = TouchScreenKeyboardType.Default;
            if_name.onValueChanged.AddListener(
                (str) => if_name.text = Regex.Replace(str, @"[^0-9a-zA-Z가-힣]", ""));
        }
        else if(Application.systemLanguage == SystemLanguage.Japanese)
        {
            if_name.keyboardType = TouchScreenKeyboardType.Default;
            if_name.onValueChanged.AddListener(
                (str) => if_name.text = Regex.Replace(str, @"[^0-9a-zA-Zぁ-んァ-ー０-９]", ""));
        }
        else
        {
            if_name.keyboardType = TouchScreenKeyboardType.ASCIICapable;
            if_name.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
        }



        if_name.placeholder.GetComponent<TextMeshProUGUI>().text = GetValueFromCSV(21)
            + id.ToString() + GetValueFromCSV(22) + " ...";
    }

    public Player GetPlayerInfo()
    {
        Player p = new Player();

        p.id = id;
        p.name = "";
        if (if_name.text == "")
            p.name = GetValueFromCSV(21) + id.ToString() + GetValueFromCSV(22);
        else
            p.name = if_name.text;

        return p;
    }
}

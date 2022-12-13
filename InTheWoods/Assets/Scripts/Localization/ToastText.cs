using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastText : LocalizedText
{
    void Update()
    {
        // 인터넷 연결
        if (Application.internetReachability != NetworkReachability.NotReachable)
            mytext.text = GetValueFromCSV(81);
        else
            mytext.text = GetValueFromCSV(10);
    }
}

using System;
using TMPro;
using UnityEngine;

public class Step3_Timer : MonoBehaviour
{
    Step3_Manager manager;
    [SerializeField] TextMeshProUGUI txt_timer;

    int min = 3;
    int sec = 0;
    [SerializeField] float time = 0;

    bool timer = false;

    void Start()
    {
        manager = FindObjectOfType<Step3_Manager>();
        time = min * 60 + sec;
        SetTimer(min, sec);
        Invoke("StartTimer", 2f);
    }
    void StartTimer() => timer = true;

    void Update()
    {
        if (timer)
        {
            time -= Time.deltaTime;
            min = (int)time / 60;
            sec = ((int)time - min * 60) % 60;

            if(min <=0 && sec <= 0)
            {
                SetTimer(0, 0);
                Handheld.Vibrate();
                EndThis();
            }
            else
            {
                SetTimer(min, sec);
            }
        }
    }

    void SetTimer(int m, int s)
    {
        string str = string.Format("{0:D2}:{1:D2}", m, s);
        txt_timer.text = str;
    }

    void EndThis()
    {
        manager.Active_Btn();
        Destroy(GetComponent<Step3_Timer>());
    }
}

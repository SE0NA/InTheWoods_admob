using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    // 모바일 화면 비율 초기 설정

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scalewidth = 1f / scaleheight;

        if(scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        cam.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}

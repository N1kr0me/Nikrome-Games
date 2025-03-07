using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orienter : MonoBehaviour
{
    void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer && IsMobile())
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    bool IsMobile()
    {
        return SystemInfo.deviceType == DeviceType.Handheld;
    }
}

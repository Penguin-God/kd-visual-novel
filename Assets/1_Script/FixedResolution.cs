using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedResolution : MonoBehaviour
{
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1080, 720, true);
    }
}
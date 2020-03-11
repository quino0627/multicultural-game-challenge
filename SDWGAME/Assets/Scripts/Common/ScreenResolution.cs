using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1280, 720, false); 
    }
}

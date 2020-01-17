using System;
using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;

public class SelectResponse : MonoBehaviour
{
    // game 4개마다 장착된 스크립트
    private SpriteGlowEffect glow;
    public bool isHovering;
    void Start()
    {
        

    }


    private void OnMouseEnter()
    {
        //반짝반짝

        isHovering = true;

    }

    private void OnMouseExit()
    {
        isHovering = false;
    }
}

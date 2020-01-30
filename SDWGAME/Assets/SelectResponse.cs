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
        
        // 뾰로롱 소리
        SoundManager.Instance.Play_SoundGameHover();

    }

    private void OnMouseExit()
    {
        isHovering = false;
    }
}

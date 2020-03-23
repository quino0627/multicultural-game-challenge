using System;
using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectResponse : MonoBehaviour
{
    // game 4개마다 장착된 스크립트
    private SpriteGlowEffect glow;
    public bool isHovering;
    private SettingsHandler setting;
    void Start()
    {

        setting = FindObjectOfType<SettingsHandler>();
    }


    private void OnMouseEnter()
    {
        //반짝반짝

        // setting창이 보여지고 있으면 이 변수값은 true입니다.
//        Debug.Log(setting.isShowed);
        if (setting.isShowed)
        {
            return;
        }
        isHovering = true;
        
        // 뾰로롱 소리
        SoundManager.Instance.Play_SoundGameHover();

//        ScreenMouseRay();
    }

    private void OnMouseExit()
    {
        isHovering = false;
    }
    
}

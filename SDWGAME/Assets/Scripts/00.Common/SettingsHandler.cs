using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsHandler : UIPT_PRO_Demo_GUIPanel
{

    public Slider BGMSlider = null;
    public Slider EffectSlider = null;

    public Toggle UltraToggle = null;
    public Toggle MediumToggle = null;
    public Toggle LowToggle = null;

    void Awake()
    {
//        setQualityToggleButton();
//        if (UltraMark.activeSelf == true)
//        {
//            UltraMark.SetActive(false);    
//        }
//
//        if (MediumMark.activeSelf == true)
//        {
//            MediumMark.SetActive(false);
//        }
//
//        if (LowMark.activeSelf == true)
//        {
//            LowMark.SetActive(false);
//        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpBGMVolume()
    {
        if (SoundManager.Instance.m_MusicVolume < 1.0f)
        {
            SoundManager.Instance.m_MusicVolume += 0.1f;
            BGMSlider.value = SoundManager.Instance.m_MusicVolume;
        }
    }

    public void UpEffectVolume()
    {
        if (SoundManager.Instance.m_SoundVolume < 1.0f)
        {
            SoundManager.Instance.m_SoundVolume += 0.1f;
            EffectSlider.value = SoundManager.Instance.m_SoundVolume;
        }
    }

    public void DownBGMVolume()
    {
        if (SoundManager.Instance.m_MusicVolume > 0f)
        {
            SoundManager.Instance.m_MusicVolume -= 0.1f;
            BGMSlider.value = SoundManager.Instance.m_MusicVolume;
        }
    }

    public void DownEffectVolume()
    {
        if (SoundManager.Instance.m_SoundVolume > 0f)
        {
            SoundManager.Instance.m_SoundVolume -= 0.1f;
            EffectSlider.value = SoundManager.Instance.m_SoundVolume;
        }
    }


    public void SetBGMVolume(Slider bgmSlider)
    {
        SoundManager.Instance.SetMusicVolume(bgmSlider.value);
    }

    public void SetEffectVolume(Slider effectSlider)
    {
        SoundManager.Instance.SetSoundVolume(effectSlider.value);
    }
    
    
    // Quality 설정
    public void setQuality(int qualityIndex)
    {
        Debug.Log("Hello SetQuality!");
        Debug.Log($"quality level is {QualitySettings.GetQualityLevel()}");
//        Mark.SetActive(true);
        // 0: Low, 1: High, 2:Ultra
        QualitySettings.SetQualityLevel(qualityIndex);
//        setQualityToggleButton();
    }

    public void setQualityToggleButton()
    {
        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                Debug.Log("LowMark ison true");
                UltraToggle.isOn = false;
                MediumToggle.isOn = false;
                LowToggle.isOn = true;
                break;
            case 1:
                Debug.Log("MediumMark ison true");
                UltraToggle.isOn = false;
                MediumToggle.isOn = true;
                LowToggle.isOn = false;
                break;
            case 2:
                Debug.Log("UltraMark ison true");
                UltraToggle.isOn = true;
                MediumToggle.isOn = false;
                LowToggle.isOn = false;
                break;
            default:
                Debug.Log("Do nothing?");
                break;
                
        }
    }
    
    

    public void CloseSettings()
    {
        Debug.Log("CLOSED BYBYE");
        Hide();
    }


}

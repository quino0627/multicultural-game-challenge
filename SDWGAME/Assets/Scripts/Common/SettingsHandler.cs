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

    public void CloseSettings()
    {
        Debug.Log("CLOSED BYBYE");
        Hide();
    }
    
   
}

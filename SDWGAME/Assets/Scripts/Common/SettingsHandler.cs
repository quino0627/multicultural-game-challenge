using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsHandler : UIPT_PRO_Demo_GUIPanel
{


    public AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("bgmVolume", volume);
    }

    public void CloseSettings()
    {
        Debug.Log("CLOSED BYBYE");
        Hide();
    }
}

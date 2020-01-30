using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : UIPT_PRO_Demo_GUIPanel
{
    public GameObject SettingPanel;
    public SettingsHandler m_Settings = null;
    public GameObject ClosePanel;

    public AudioClip MainMenuBgm;
    
    void Awake()
    {
        // Set GSui.Instance.m_AutoAnimation to false, 
        // this will let you control all GUI Animator elements in the scene via scripts.
        if (enabled)
        {
            GSui.Instance.m_GUISpeed = 4.0f;
            GSui.Instance.m_AutoAnimation = false;
        }

        if (this.transform.gameObject.activeSelf == false)
        {
            this.transform.gameObject.SetActive(true);
        }
        
        if (ClosePanel.activeSelf == false)
        {
            ClosePanel.SetActive(true);
        }
        
    }
    
    public void Start()
    {
        StartCoroutine(Show());
        SoundManager.Instance.Play_Music(MainMenuBgm);
    }

    public void StartGame(GameObject startButton)
    {
        // Play Play button sound
        SoundManager.Instance.Play_SoundPlay();
        GSui.Instance.MoveOut(this.transform, true);
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);
        GSui.Instance.LoadLevel("LevelMenu", 1.0f);
    }

    public void GoToResult()
    {
        //SceneManager.LoadScene("ResultScene");
    }

    public void ToggleSettingPanel()
    {
        //환경설정 창 띄우기
        m_Settings.Show();
        
    }
    
    IEnumerator Show()
    {
        //Debug.Log("ASDF");
        // Play MoveIn animation
        GSui.Instance.MoveIn(this.transform, true);

        // Creates a yield instruction to wait for a given number of seconds
        // http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
        yield return new WaitForSeconds(1.0f);

        // Play particles in the hierarchy of given transfrom
        GSui.Instance.PlayParticle(this.transform);
    }
} 

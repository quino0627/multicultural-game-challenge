using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject SettingPanel;
    public SettingsHandler m_Settings = null;

    public void Start()
    {
        StartCoroutine(Show());
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SelectMenu");
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
        Debug.Log("ASDF");
        // Play MoveIn animation
        GSui.Instance.MoveIn(this.transform, true);

        // Creates a yield instruction to wait for a given number of seconds
        // http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
        yield return new WaitForSeconds(1.0f);

        // Play particles in the hierarchy of given transfrom
        GSui.Instance.PlayParticle(this.transform);
    }
} 

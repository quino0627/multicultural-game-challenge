using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    public GraphicRaycaster GR;
    public SettingsHandler m_Settings = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickTest()
    {
        Debug.Log("Clicked");
    }

    public void Detection()
    {
        GR.enabled = false;
        SceneManager.LoadScene("DetectionGame");
        
    }

    public void Synthesis()
    { 
        GR.enabled = false;
        SceneManager.LoadScene("CrabLevel1");
        
    }

    public void Elimination()
    {
        GR.enabled = false;
        SceneManager.LoadScene("EliminationEscape");
        

    }

    public void Alternative()
    {
        GR.enabled = false;
        SceneManager.LoadScene("AlternativeGame");
        
    }

    public void ToggleSettingPanel()
    {
        //환경설정 창 띄우기
        m_Settings.Show();
        
    }

    public void GoBack()
    {
        SceneManager.LoadScene("StartMenu");
        //SceneManager.LoadScene("LevelMenu");
    }
    
}

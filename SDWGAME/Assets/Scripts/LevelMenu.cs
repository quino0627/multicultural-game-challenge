using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button Easy;

    public Button Normal;

    public Button Hard;
    
    public SettingsHandler m_Settings = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Normal.interactable = false;
        Hard.interactable = false;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ToggleSettingPanel()
    {
        //환경설정 창 띄우기
        m_Settings.Show();
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseController : UIPT_PRO_Demo_GUIPanel
{

    
    // Start is called before the first frame update
    
    void Awake()
    {
        // Set GSui.Instance.m_AutoAnimation to false, 
        // this will let you control all GUI Animator elements in the scene via scripts.
        if (enabled)
        {
            GSui.Instance.m_GUISpeed = 4.0f;
            GSui.Instance.m_AutoAnimation = false;
        }

        
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowClosePanel()
    {
        Show();
    }

    public void ExitGame()
    {
        SoundManager.Instance.Play_SoundExitGame();
        GSui.Instance.MoveOut(this.transform, true);
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);
        Debug.Log("Close Game");
        Application.Quit();

    }
}

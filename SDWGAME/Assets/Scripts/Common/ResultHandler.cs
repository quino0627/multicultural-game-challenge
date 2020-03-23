using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultHandler : UIPT_PRO_Demo_GUIPanel
{
    
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
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenResult()
    {
        Show();
    }

    public void Button_Main()
    {
        GSui.Instance.MoveOut(this.transform, true);
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);
        GSui.Instance.LoadLevel("StartMenu", 3.0f);
    }

    public void Button_Restart()
    {
        GSui.Instance.MoveOut(this.transform, true);
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);
        Scene currentScene = SceneManager.GetActiveScene();
        GSui.Instance.LoadLevel(currentScene.name, 3.0f);
        
    }

    public void Button_Restart_HJ()
    {
        // 애니메이션을 실행시키고 현재 씬 정보를 얻음.
        GSui.Instance.MoveOut(this.transform, true);
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);
        Scene currentScene = SceneManager.GetActiveScene();
        // 씬 이름으로 씬을 불러오는 함수
        GSui.Instance.LoadLevel(currentScene.name, 3.0f);
        Invoke("Restart_HJ", 3.0f);
    }

    private void Restart_HJ()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void Button_Select()
    {
        GSui.Instance.MoveOut(this.transform, true);
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);
        GSui.Instance.LoadLevel("SelectMenu", 1.0f);
    }
    
}

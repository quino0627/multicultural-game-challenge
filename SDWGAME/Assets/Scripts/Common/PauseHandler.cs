using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PauseHandler : UIPT_PRO_Demo_GUIPanel
{
    public SettingsHandler m_Settings = null;
    public static bool GameIsPaused = false;
    public Stopwatch timer;
    
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
            this.transform.gameObject.SetActive(true);
        
            // Activate all UI Canves GameObjects.
        if (m_Settings.gameObject.activeSelf == false)
            m_Settings.gameObject.SetActive(true);
        
        
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        Debug.Log("asdf");
        Time.timeScale = 0f;
//        GameIsPaused = true;
        ChangeGameIsPaused(true);
    }


    public void Button_Resume()
    {
        // Play Click sound
        // UIPT_PRO_SoundController.Instance.Play_SoundClick();

        // Hide this panel
        Hide();
        Time.timeScale = 1f;
        ChangeGameIsPaused(false);
    }

    private static void ChangeGameIsPaused(bool paused)
    {
        GameIsPaused = paused;
    }

    public void Button_Settings()
    {
        // Play Click sound
        // UIPT_PRO_SoundController.Instance.Play_SoundClick();

        // Show this panel
        m_Settings.Show();
    }

    public void Button_Levels()
    {
        // Play Click sound
        // UIPT_PRO_SoundController.Instance.Play_SoundClick();

        // Resume everything
        Time.timeScale = 1.0f;

        // Play Move Out animation
        GSui.Instance.MoveOut(this.transform, true);

        // Keep particles stay alive until it finished playing.
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

        // Load next scene according to orientation of current scene.
        GSui.Instance.LoadLevel("SelectMenu", 1.0f);
    }
}

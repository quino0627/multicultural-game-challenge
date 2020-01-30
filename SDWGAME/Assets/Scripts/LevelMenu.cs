using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    //KeepTrackController ConclusionData
    public GameObject totalStorageObject;
    private KeepTrackController totalStorageScript;
    
    public Button Easy;

    public Button Normal;

    public Button Hard;
    
    public SettingsHandler m_Settings = null;

  
    
    // Start is called before the first frame update
    void Start()
    {
        totalStorageObject = GameObject.Find("TotalStorage");
        totalStorageScript = totalStorageObject.GetComponent<KeepTrackController>();
        
        if (totalStorageScript.isLevelOpen[1])
        {
            Normal.interactable = true;
        }
        else
        {
            Normal.interactable = false;
        }

        if (totalStorageScript.isLevelOpen[2])
        {
            Hard.interactable = true;
        }
        else
        {
            Hard.interactable = false;
        }
        
    }

    public void EasySelectMenu()
    {
        totalStorageScript.chosenLevel = 0;
        SceneManager.LoadScene("SelectMenu");
    }
    public void NormalSelectMenu()
    {
        totalStorageScript.chosenLevel = 1;
        SceneManager.LoadScene("SelectMenu");
    }
    public void HardSelectMenu()
    {
        totalStorageScript.chosenLevel = 2;
        SceneManager.LoadScene("SelectMenu");
    }
    
    public void ToggleSettingPanel()
    {
        //환경설정 창 띄우기
        m_Settings.Show();
        
    }
}

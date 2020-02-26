using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    //KeepTrackController ConclusionData
    private GameObject totalStorageObject;
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

        if (totalStorageScript.bLogin)
        {
            if (totalStorageScript.tmpStars[0, 0] < 3 ||
                totalStorageScript.tmpStars[1, 0] < 3 ||
                totalStorageScript.tmpStars[2, 0] < 3 ||
                totalStorageScript.tmpStars[3, 0] < 3)
            {
                //초급
                totalStorageScript.isLevelOpen[0] = true;
                totalStorageScript.isLevelOpen[1] = false;
                totalStorageScript.isLevelOpen[2] = false;
            }
            else if (totalStorageScript.tmpStars[0, 1] < 3 ||
                     totalStorageScript.tmpStars[1, 1] < 3 ||
                     totalStorageScript.tmpStars[2, 1] < 3 ||
                     totalStorageScript.tmpStars[3, 1] < 3)
            {
                //중급

                if (!totalStorageScript.isLevelOpen[1])
                {
                    totalStorageScript.tmpTriedCnt = new Dictionary<string, int[,]>()
                    {
                        {"Detection", new int[4, 30]},
                        {"Synthesis", new int[4, 30]},
                        {"Elimination", new int[4, 30]},
                        {"Alternative", new int[4, 30]}
                    };
                }

                totalStorageScript.isLevelOpen[0] = totalStorageScript.isLevelOpen[1] = true;
                totalStorageScript.isLevelOpen[2] = false;
            }
            else
            {
                //고급
                if (!totalStorageScript.isLevelOpen[2])
                {
                    totalStorageScript.tmpTriedCnt = new Dictionary<string, int[,]>()
                    {
                        {"Detection", new int[4, 30]},
                        {"Synthesis", new int[4, 30]},
                        {"Elimination", new int[4, 30]},
                        {"Alternative", new int[4, 30]}
                    };
                }
                
                
                totalStorageScript.isLevelOpen[0]
                    = totalStorageScript.isLevelOpen[1]
                        = totalStorageScript.isLevelOpen[2] = true;
            }
        }

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
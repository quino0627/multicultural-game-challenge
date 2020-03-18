using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class LevelMenu : MonoBehaviour
{
    //KeepTrackController ConclusionData
    private GameObject totalStorageObject;
    private TotalDataManager totalStorageScript;

    private GameObject levelStorage;
    private LevelDataManager levelStorageScript;

    public Button EasyStage1;
    public Button EasyStage2;
    public Button EasyStage3;

    public Button NormalStage1;
    public Button NormalStage2;
    public Button NormalStage3;

    public Button HardStage1;
    public Button HardStage2;
    public Button HardStage3;

    public SettingsHandler m_Settings = null;

    public EGameName chosenGame;

    public int[,] starData;

    public GameObject[] Stars;

    // Start is called before the first frame update
    void Start()
    {
        starData = new int[3, 3];
        Debug.Log("stardata전: "+starData[0,0]);
        if (!SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.Play_MenuMusic();
        }

        totalStorageObject = GameObject.Find("TotalStorage");
        totalStorageScript = totalStorageObject.GetComponent<TotalDataManager>();

        levelStorage = GameObject.Find("LevelStorage");
        levelStorageScript = levelStorage.GetComponent<LevelDataManager>();

        chosenGame = (EGameName) totalStorageScript.chosenGame;
        Debug.Log("STart Level Menu, chosenGame: " + chosenGame);
        starData = levelStorageScript.LoadLevelSceneStar(chosenGame, totalStorageScript.currId);
        
        Debug.Log("Stardata[0,0]: "+starData[0,0]);
        // 3 -> 12 수정필요
        if (totalStorageScript.bLogin)
        {
            if (starData[0, 0] + starData[0, 1] + starData[0, 2] == 4)
            {
                totalStorageScript.isLevelOpen[(int) chosenGame, 1] = true;
            }

            if (starData[1, 0] + starData[1, 1] + starData[1, 2] == 4)
            {
                totalStorageScript.isLevelOpen[(int) chosenGame, 2] = true;
            }
        }

        if (totalStorageScript.isLevelOpen[(int) chosenGame, 1])
        {
            NormalStage1.interactable = true;
            NormalStage2.interactable = true;
            NormalStage3.interactable = true;
        }
        else
        {
            NormalStage1.interactable = false;
            NormalStage2.interactable = false;
            NormalStage3.interactable = false;
        }

        if (totalStorageScript.isLevelOpen[(int) chosenGame, 2])
        {
            HardStage1.interactable = true;
            HardStage2.interactable = true;
            HardStage3.interactable = true;
        }
        else
        {
            HardStage1.interactable = false;
            HardStage2.interactable = false;
            HardStage3.interactable = false;
        }

        if (starData[0, 0] == 4)
        {
            Stars[0].SetActive(true);
        }

        if (starData[0, 1] == 4)
        {
            Stars[1].SetActive(true);
        }

        if (starData[0, 2] == 4)
        {
            Stars[2].SetActive(true);
        }

        if (starData[1, 0] == 4)
        {
            Stars[3].SetActive(true);
        }

        if (starData[1, 1] == 4)
        {
            Stars[4].SetActive(true);
        }

        if (starData[1, 2] == 4)
        {
            Stars[5].SetActive(true);
        }

        if (starData[2, 0] == 4)
        {
            Stars[6].SetActive(true);
        }

        if (starData[2, 1] == 4)
        {
            Stars[7].SetActive(true);
        }

        if (starData[2, 2] == 4)
        {
            Stars[8].SetActive(true);
        }
    }


    public void LoadGame()
    {
        switch (chosenGame)
        {
            case EGameName.Detection:
                if (totalStorageScript.chosenLevel == 0)
                {
                    SceneManager.LoadScene("TutorialDetectionGameV2");
                }
                else
                {
                    SceneManager.LoadScene("DetectionGame");
                }

                break;
            case EGameName.Synthesis:
                if (totalStorageScript.chosenLevel == 0)
                {
                    SceneManager.LoadScene("TutorialSynthesisGameV2");
                }

                if (totalStorageScript.chosenLevel == 1)
                {
                    SceneManager.LoadScene("CrabLevel3");
                }

                if (totalStorageScript.chosenLevel == 2)
                {
                    SceneManager.LoadScene("CrabLevel4");
                }

                break;

            case EGameName.Elimination:
                if (totalStorageScript.chosenLevel == 0)
                {
                    SceneManager.LoadScene("TutorialEliminationGameV2");
                }
                else
                {
                    SceneManager.LoadScene("EliminationEscape");
                }

                break;

            case EGameName.Alternative:
                if (totalStorageScript.chosenLevel == 0)
                {
                    SceneManager.LoadScene("TutorialAlternativeGameV2");
                }
                else
                {
                    SceneManager.LoadScene("AlternativeGame");
                }

                break;
        }
    }

    public void EasyStage1SelectMenu()
    {
        totalStorageScript.chosenLevel = 0;
        totalStorageScript.chosenStage = 0;
        LoadGame();
    }

    public void EasyStage2SelectMenu()
    {
        totalStorageScript.chosenLevel = 0;
        totalStorageScript.chosenStage = 1;
        LoadGame();
    }

    public void EasyStage3SelectMenu()
    {
        totalStorageScript.chosenLevel = 0;
        totalStorageScript.chosenStage = 2;
        LoadGame();
    }

    public void NormalStage1SelectMenu()
    {
        totalStorageScript.chosenLevel = 1;
        totalStorageScript.chosenStage = 0;
        LoadGame();
    }

    public void NormalStage2SelectMenu()
    {
        totalStorageScript.chosenLevel = 1;
        totalStorageScript.chosenStage = 1;
        LoadGame();
    }

    public void NormalStage3SelectMenu()
    {
        totalStorageScript.chosenLevel = 1;
        totalStorageScript.chosenStage = 2;
        LoadGame();
    }

    public void HardStage1SelectMenu()
    {
        totalStorageScript.chosenLevel = 2;
        totalStorageScript.chosenStage = 0;
        LoadGame();
    }

    public void HardStage2SelectMenu()
    {
        totalStorageScript.chosenLevel = 2;
        totalStorageScript.chosenStage = 1;
        LoadGame();
    }

    public void HardStage3SelectMenu()
    {
        totalStorageScript.chosenLevel = 2;
        totalStorageScript.chosenStage = 2;
        LoadGame();
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
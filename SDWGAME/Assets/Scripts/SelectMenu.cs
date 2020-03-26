using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    public GraphicRaycaster GR;
    public SettingsHandler m_Settings = null;
    public GameObject TotalStorage;
    public TotalDataManager totalStorageScript;
    public int[] stages;
    public Sprite emptyStar;
    public Sprite filledStar;

    public int currentLevel;

    //public GameObject[] gameNames;
    public GameObject[] stars;
    private SpriteRenderer[] starSprite;

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
        TotalStorage = GameObject.Find("TotalStorage");
        totalStorageScript = TotalStorage.GetComponent<TotalDataManager>();
        currentLevel = totalStorageScript.chosenLevel;
        if (SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.StopMusic();
        }
        Invoke(nameof(MenuMusicStart), 1f);
        
        /*for (int i = 0; i < 4; i++)
        {
            starSprite = stars[i].GetComponentsInChildren<SpriteRenderer>();
            int cntStar = totalStorageScript.tmpStars[i, currentLevel];
            Debug.Log("tmpstars[ " + i + ", " + currentLevel + " ] = " +
                      totalStorageScript.tmpStars[i, currentLevel]);
            if (cntStar == 0)
            {
                starSprite[0].sprite = emptyStar;
                starSprite[1].sprite = emptyStar;
                starSprite[2].sprite = emptyStar;
            }
            else if (cntStar == 1)
            {
                starSprite[0].sprite = filledStar;
                starSprite[1].sprite = emptyStar;
                starSprite[2].sprite = emptyStar;
            }
            else if (cntStar == 2)
            {
                starSprite[0].sprite = filledStar;
                starSprite[1].sprite = filledStar;
                starSprite[2].sprite = emptyStar;
            }
            else if (cntStar == 3)
            {
                starSprite[0].sprite = filledStar;
                starSprite[1].sprite = filledStar;
                starSprite[2].sprite = filledStar;
            }
        }*/
    }

    
    
    // function invoke를 위해 따로 함수를 선업합니다.
    private void MenuMusicStart()
    {
        SoundManager.Instance.Play_MenuMusic();
    }

    public void Detection()
    {
        GR.enabled = false;
        SoundManager.Instance.Play_SoundClick();

        totalStorageScript.chosenGame = 0;
        SceneManager.LoadScene("LevelMenu");
       
        
        /*if (totalStorageScript.chosenLevel == 0)
        {
            SceneManager.LoadScene("TutorialDetectionGameV2");    
        }
        else
        {
            SceneManager.LoadScene("DetectionGame");    
        }*/
        
    }

    public void Synthesis()
    {
        GR.enabled = false;
        SoundManager.Instance.Play_SoundClick();
        
        totalStorageScript.chosenGame = 1;
        SceneManager.LoadScene("LevelMenu");
        
        /*if (totalStorageScript.chosenLevel == 0)
        {
            SceneManager.LoadScene("TutorialSynthesisGame");
            
        }

        if (totalStorageScript.chosenLevel == 1)
        {
            SceneManager.LoadScene("CrabLevel3");
        }

        if (totalStorageScript.chosenLevel == 2)
        {
            SceneManager.LoadScene("CrabLevel4");
        }*/
    }

    public void Elimination()
    {
        GR.enabled = false;
        SoundManager.Instance.Play_SoundClick();
        
        totalStorageScript.chosenGame = 2;
        SceneManager.LoadScene("LevelMenu");
        
        
        //SceneManager.LoadScene("TutorialEliminationGame");
//        SceneManager.LoadScene("EliminationEscape");
    }

    public void Alternative()
    {
        GR.enabled = false;
        SoundManager.Instance.Play_SoundClick();
        
        totalStorageScript.chosenGame = 3;
        SceneManager.LoadScene("LevelMenu");
        
        //SceneManager.LoadScene("TutorialAlternativeGame");
//        SceneManager.LoadScene("AlternativeGame");
    }

    public void ToggleSettingPanel()
    {
        //환경설정 창 띄우기
        m_Settings.Show();
    }

    public void GoBack()
    {
        SoundManager.Instance.Play_SoundBack();
        GSui.Instance.MoveOut(this.transform, true);
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);
        GSui.Instance.LoadLevel("StartMenu", 1.0f);
        //SceneManager.LoadScene("LevelMenu");
    }
}
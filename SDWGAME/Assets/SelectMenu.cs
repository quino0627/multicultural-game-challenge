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
    public KeepTrackController TotalStorageScript;
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
        TotalStorageScript = TotalStorage.GetComponent<KeepTrackController>();
        currentLevel = TotalStorageScript.chosenLevel;


        //Debug.Log(emptyStar);
        //Debug.Log(filledStar);

        for (int i = 0; i < 4; i++)
        {
            starSprite = stars[i].GetComponentsInChildren<SpriteRenderer>();
            int cntStar = TotalStorageScript.tmpStars[i, currentLevel];
            Debug.Log("tmpstars[ " + i + ", " + currentLevel + " ] = " +
                      TotalStorageScript.tmpStars[i, currentLevel]);
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
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Detection()
    {
        GR.enabled = false;
        SoundManager.Instance.Play_SoundClick();
        SceneManager.LoadScene("DetectionGame");
    }

    public void Synthesis()
    {
        GR.enabled = false;
        SoundManager.Instance.Play_SoundClick();
        if (TotalStorageScript.chosenLevel == 0)
        {
            //매직넘버
            if (TotalStorageScript.tmpStage[1] < 14)
            {
                SceneManager.LoadScene("CrabLevel1");
            }
            else
            {
                SceneManager.LoadScene("CrabLevel2");
            }
        }

        if (TotalStorageScript.chosenLevel == 1)
        {
            SceneManager.LoadScene("CrabLevel3");
        }

        if (TotalStorageScript.chosenLevel == 2)
        {
            SceneManager.LoadScene("CrabLevel4");
        }
    }

    public void Elimination()
    {
        GR.enabled = false;
        SoundManager.Instance.Play_SoundClick();
        SceneManager.LoadScene("EliminationEscape");
    }

    public void Alternative()
    {
        GR.enabled = false;
        SoundManager.Instance.Play_SoundClick();
        SceneManager.LoadScene("AlternativeGame");
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
        GSui.Instance.LoadLevel("LevelMenu", 1.0f);
        //SceneManager.LoadScene("LevelMenu");
    }
}
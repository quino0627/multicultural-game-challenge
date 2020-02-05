using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class StartMenu : UIPT_PRO_Demo_GUIPanel
{
    public GameObject SettingPanel;
    public SettingsHandler m_Settings = null;
    public GameObject ClosePanel;
    public AudioClip MainMenuBgm;
    
    private GameObject TotalStorage;
    private KeepTrackController TotalStorageScript;

    private GameObject StageStorage;
    private DataController StageStorageScript;
    
    public GameObject idInputFieldGameObject;
    private TMP_InputField idInputField;
    public GameObject idEnterButtonGameObject;

    public GameObject newIdInputFieldGameObject;
    private TMP_InputField newIdInputField;
    public GameObject newIdEnterButtonGameObject;
    

    public GameObject startButtonGameObject;
    public GameObject statisticButtonGameObject;
    
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
        
        if (ClosePanel.activeSelf == false)
        {
            ClosePanel.SetActive(true);
        }
        
    }
    
    public void Start()
    {
        TotalStorage = GameObject.Find("TotalStorage");
        TotalStorageScript = TotalStorage.GetComponent<KeepTrackController>();
        StageStorage = GameObject.Find("StageStorage");
        StageStorageScript = StageStorage.GetComponent<DataController>();
        
        idInputField = idInputFieldGameObject.GetComponent<TMP_InputField>();
        newIdInputField = newIdInputFieldGameObject.GetComponent<TMP_InputField>();
        
        StartCoroutine(Show());
        SoundManager.Instance.Play_Music(MainMenuBgm);
    }

    public void StartGame(GameObject startButton)
    {
        // Play Play button sound
        SoundManager.Instance.Play_SoundPlay();
        GSui.Instance.MoveOut(this.transform, true);
        GSui.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);
        GSui.Instance.LoadLevel("LevelMenu", 1.0f);
    }

    public void GoToResult()
    {
        //SceneManager.LoadScene("ResultScene");
    }

    public void ToggleSettingPanel()
    {
        //환경설정 창 띄우기
        m_Settings.Show();
        
    }
    
    IEnumerator Show()
    {
        //Debug.Log("ASDF");
        // Play MoveIn animation
        GSui.Instance.MoveIn(this.transform, true);

        // Creates a yield instruction to wait for a given number of seconds
        // http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
        yield return new WaitForSeconds(1.0f);

        // Play particles in the hierarchy of given transfrom
        GSui.Instance.PlayParticle(this.transform);
    }

    public void CheckEnterGameStart()
    {
        
        var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("enter 누름");
            ExecuteEvents.Execute(idEnterButtonGameObject, pointer, ExecuteEvents.submitHandler);
        }
    }
    public void CheckEnterNewUser()
    {
        
        var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("enter 누름");
            ExecuteEvents.Execute(newIdEnterButtonGameObject, pointer, ExecuteEvents.submitHandler);
        }
    }
    public void Login()
    {
        //Debug.Log("Login 함수 호출됨");
        //button 눌러질때 불러짐
        //Debug.Log(idInputField.text);
        TotalStorageScript.GetDataWithID(idInputField.text);
        TotalStorageScript.LoadTmpData();
        /*StageStorageScript.LoadStageData();*/
        
        idInputFieldGameObject.SetActive(false);
        idEnterButtonGameObject.SetActive(false);
        newIdInputFieldGameObject.SetActive(false);
        newIdEnterButtonGameObject.SetActive(false);
        startButtonGameObject.SetActive(true);
        statisticButtonGameObject.SetActive(true);
        
    }

    public void makeNewID()
    {
        TotalStorageScript.makeNewId(newIdInputField.text);
        StageStorageScript.makeNewId(newIdInputField.text);
    }
    
} 

﻿using System;
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
    public GameObject idInputPlaceholder;
    private TextMeshProUGUI idInputPlaceholderTextMeshProUgui;
    public GameObject idEnterButtonGameObject;

    public GameObject newIdInputFieldGameObject;
    private TMP_InputField newIdInputField;
    public GameObject newIdInputPlaceholder;
    private TextMeshProUGUI newIdInputPlaceholderTextMeshProUgui;
    public GameObject newIdEnterButtonGameObject;
    

    public GameObject startButtonGameObject;
    public GameObject statisticButtonGameObject;

    public TMP_InputField tmpInputfield;
   
    
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
        
        idInputPlaceholderTextMeshProUgui = idInputPlaceholder.GetComponent<TextMeshProUGUI>();
        idInputPlaceholderTextMeshProUgui.text = "ID를\n입력하세요...";

        newIdInputPlaceholderTextMeshProUgui = newIdInputPlaceholder.GetComponent<TextMeshProUGUI>();
        
        StartCoroutine(Show());
        SoundManager.Instance.Play_Music(MainMenuBgm);
        
        if (TotalStorageScript.bLogin)
        {
            idInputFieldGameObject.SetActive(false);
            idEnterButtonGameObject.SetActive(false);
            newIdInputFieldGameObject.SetActive(false);
            newIdEnterButtonGameObject.SetActive(false);
            
            startButtonGameObject.SetActive(true);
            statisticButtonGameObject.SetActive(true);
        }
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

        if (!TotalStorageScript.GetDataWithID(idInputField.text))
        {
            idInputField.text = null;
            idInputPlaceholderTextMeshProUgui.text = "없는 ID\n다시입력하시오";
            
        }
        
        if (TotalStorageScript.bLogin)
        {
            TotalStorageScript.LoadTmpData();
            StageStorageScript.LoadUserStageData();

            idInputFieldGameObject.SetActive(false);
            idEnterButtonGameObject.SetActive(false);
            newIdInputFieldGameObject.SetActive(false);
            newIdEnterButtonGameObject.SetActive(false);
            startButtonGameObject.SetActive(true);
            statisticButtonGameObject.SetActive(true);

        }
    }

    public void makeNewID()
    {
        string newId = newIdInputField.text;
        if (TotalStorageScript.makeNewId(newId))
        {
            TotalStorageScript.makeNewId(newIdInputField.text);
            StageStorageScript.makeNewId(newIdInputField.text);
            newIdInputField.text = null;
        }
        else
        {
         //id 중복 다시입력
         newIdInputField.text = null;
         newIdInputPlaceholderTextMeshProUgui.text = "중복된 ID\n다른 ID 입력하시오";
        }
    }

    

    public void deleteTotalInfo()
    {
        TotalStorageScript.deleteTotalInfoOfCurrentId(tmpInputfield.text);
        
    }

    public void deleteStageInfo()
    {
        StageStorageScript.deleteStageInfoOfCurrentId(tmpInputfield.text);
    }
} 

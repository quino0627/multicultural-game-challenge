using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerController : MonoBehaviour
{
    private GameObject QuizManager;
    private AudioSource _audioSource;
    private FishShowAnswer script;
    // Start is called before the first frame update
    void Start()
    {
        //QuizManager = GameObject.Find("QuizManager");
        _audioSource = gameObject.GetComponent<AudioSource>();
        //script = QuizManager.GetComponent<FishShowAnswer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        Debug.Log("OnMouseDown");
        SpeakerClicked();
    }

    public void SpeakerClicked()
    {
        //string s = $"Sounds/Detection/{script.data.sheets[script.level].list[script.stageIndex].오답음성1}"
        _audioSource.Play();
        
    }
}

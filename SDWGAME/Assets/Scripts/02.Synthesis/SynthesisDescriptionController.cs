using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SynthesisDescriptionController : MonoBehaviour
{
    private GameObject DescriptionText;
    private GameObject ParentCrab;
    private string description_text;

    private Vector3 OriginCrabScale;
    private Vector3 AfterCrabScale;
    private Vector3 OriginMsgScale;
    private float AfterMsgScaleXValue;

    private bool isFirstUpdate;

    private Boolean allowRepeatFlag;
    private Boolean isPlaying;
    private AudioSource _audioSource;
    
    private void Awake()
    {
        isFirstUpdate = true;
        ParentCrab = this.transform.parent.gameObject;
        Debug.Log(ParentCrab.transform.localScale);
        OriginCrabScale = ParentCrab.transform.localScale;
        OriginMsgScale = this.transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
            allowRepeatFlag = false;
        isPlaying = false;
        // Get Parent Object which is Crab\
        DescriptionText = transform.Find("DescriptionText").gameObject;
        DefaultDescription();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (isFirstUpdate)
        {
            AfterCrabScale = ParentCrab.transform.localScale;
            AfterMsgScaleXValue = (OriginCrabScale.x * OriginMsgScale.x / AfterCrabScale.x);
            //Debug.Log($"OriginCrabScale.x: {OriginCrabScale.x}");
            //Debug.Log($"AfterCrabScale.x: {AfterCrabScale.x}");
            //Debug.Log($"OriginMsgScale.x: {OriginMsgScale.x}");
            //Debug.Log($"AfterMsgScaleXValue: {AfterMsgScaleXValue}");
            this.transform.localScale = new Vector3(AfterMsgScaleXValue, AfterMsgScaleXValue, 1);
            
            isFirstUpdate = false;
            
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MESSAGE CLICLED");
            if (Time.timeScale == 0) return;
            if (isPlaying) return;
            if (!allowRepeatFlag) return;
        
            StartCoroutine(StopMusicPlayWord());
        }
        
      
        
        var tmp = 0;
        this.DescriptionText.GetComponent<TextMeshPro>().text = description_text;
    }

    public void CorrectOneWord()
    {
        description_text = "맞아!";
        Invoke("DefaultDescription", 2.0f);
    }

    public string CorrectAnswer()
    {
        description_text = "정답이야. \n 고마워!";
        return "CorrectAnswer";
    }

    public void WrongAnswer()
    {
        description_text = "틀렸어. \n 다시 골라볼까?";
        Invoke("DefaultDescription", 2.0f);
    }

    public void DefaultDescription()
    {
        description_text = "해파리를 옮겨 \n 소리나는 단어를\n 만들어줘!";
        allowRepeatFlag = false;
    }

    public void RepeatSound()
    {
        Debug.Log("REPEATSOUND");
        description_text = "다시 듣기";
        allowRepeatFlag = true;

    }


//    private void OnMouseUp()
//    {
//        Debug.Log("AFETR");
//        if (Time.timeScale == 0) return;
//        if (isPlaying) return;
//        if (!allowRepeatFlag) return;
//        
//        StartCoroutine(StopMusicPlayWord());
//    }

    IEnumerator StopMusicPlayWord()
    {
        isPlaying = true;
        if (SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.PauseMusic();
        }
        yield return new WaitForSeconds(1.0f);

        if (_audioSource.clip != null)
        {
            _audioSource.Play();  
            yield return new WaitForSeconds(_audioSource.clip.length+1.5f);
        }
        else
        {
            Debug.Log("Clip Not Exists");
        }
        
        
        SoundManager.Instance.UnpauseMusic();
        isPlaying = false;



    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AlternativeQuizManager에서 사용됨
public class WordSpeakerController : MonoBehaviour
{

    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 마우스로 해당 오브젝트 - WordBoxOrigin 위에 있는 하트 - 를 클릭하면
    // SpeakerClicked 함수를 호
    private void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        SpeakerClicked();
    }

    public void SpeakerClicked()
    {
        _audioSource.Play();
    }
}

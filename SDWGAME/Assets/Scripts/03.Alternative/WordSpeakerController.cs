using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AlternativeQuizManager에서 사용됨
// 원 제시어를 말함
public class WordSpeakerController : MonoBehaviour
{

    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WordSpeakerController Started");
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 마우스로 해당 오브젝트 - WordBoxOrigin 위에 있는 하트 - 를 클릭하면
    // SpeakerClicked 함수를 호출한다.
    private void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        Debug.Log("HeartIcon Clicked");
        StartCoroutine(StopMusicPlayWord());
    }

    IEnumerator StopMusicPlayWord()
    {
        if (SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.StopMusic();
        }
        yield return new WaitForSeconds(1.0f);

        if (_audioSource.clip != null)
        {
            Debug.Log("StopMusicAnd Play Word");
            _audioSource.Play();  
        }
        
        yield return new WaitForSeconds(_audioSource.clip.length+1.5f);
        
        SoundManager.Instance.Play_AlternativeMusic();
        
        
        
    }
}

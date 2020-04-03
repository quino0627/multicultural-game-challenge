using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Alternative과제에서 사용됨
// 클릭했을 때 오디오를 재생할 수 있는 가장 보편적인 클래스.
// 다른 곳에서 사용될 수 있음
public class WordSpeakerController : MonoBehaviour
{

    private AudioSource _audioSource;
    //음원 재생 중 여러번 클릭되는 것을 방지하기 위한 플래그
    private Boolean isPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 마우스로 해당 오브젝트 - WordBoxOrigin을 클릭하면
    // SpeakerClicked 함수를 호출한다.
    private void OnMouseUp()
    {
        if (Time.timeScale == 0) return;
        if (isPlaying) return;
        StartCoroutine(StopMusicPlayWord());
    }

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

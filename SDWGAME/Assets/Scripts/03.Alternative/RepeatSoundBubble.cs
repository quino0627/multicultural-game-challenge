using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AlternativeQuizManager에서 
public class RepeatSoundBubble : MonoBehaviour
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

    private void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        Debug.Log("RepeatSoundBubble Clicked");
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
            
            Debug.Log($"StopMusicAnd Play Word, {_audioSource.clip.name}");
            _audioSource.Play();  
            yield return new WaitForSeconds(_audioSource.clip.length+1.5f);
        }
        else
        {
            Debug.Log("Clip Not Exists");
        }
        
       
        
        SoundManager.Instance.Play_AlternativeMusic();
        
        
        
    }
}

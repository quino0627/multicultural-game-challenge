using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @author: 송동욱
// 여기서는 FishShowAnswer Component에 접근하여
// 동시에 여러개의 물고기가 눌려도 하나만 재생하게끔 처리해 주기 위하여
// WordSpeakerController를 사용하지 않고 독자적인 클래스를 만들어 주었음
public class FishSpeakerController : MonoBehaviour
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        // 유니티에서 포인터를 쓰기가 애매. 그냥 매번 접근해서 변수를 가져와야 할 것 같다.
//        isPlaying = GameObject.FindObjectOfType<FishShowAnswer>().isSpeakingWord;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        if (Time.timeScale == 0) return;
        if (GameObject.FindObjectOfType<FishShowAnswer>().isSpeakingWord) return;
        StartCoroutine(StopMusicPlayWord());
    }
    
    IEnumerator StopMusicPlayWord()
    {
        GameObject.FindObjectOfType<FishShowAnswer>().isSpeakingWord = true;
        if (SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.PauseMusic();
        }

        if (_audioSource.clip != null)
        {
            _audioSource.Play();  
            yield return new WaitForSeconds(_audioSource.clip.length+0.5f);
        }
        else
        {
            Debug.Log("Clip Not Exists");
        }
        
        
        SoundManager.Instance.UnpauseMusic();
        GameObject.FindObjectOfType<FishShowAnswer>().isSpeakingWord = false;



    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetectionDescriptionController : MonoBehaviour
{
    private GameObject DescriptionText;
    private GameObject Octo;
    private AudioSource OctoAudioSource;
    private string description_text;
    //음원 재생 중 여러번 클릭되는 것을 방지하기 위한 플래그
    private Boolean isPlaying;
    void Start()
    {
        Octo = transform.parent.gameObject;
        OctoAudioSource = Octo.GetComponent<AudioSource>();
        DescriptionText = transform.Find("DescriptionText").gameObject;
        DescriptionText.GetComponent<TextMeshPro>().text = "다시 듣기";
        isPlaying = false;
    }

    // 다시 듣기를 눌렀을 떄.
    private void OnMouseUp()
    {
        if (Time.timeScale == 0) return;
        if (isPlaying) return;
        StartCoroutine(StopMusicPlayWord());
        
    }

    // 백그라운드 음악을 껐다가 켠다
    IEnumerator StopMusicPlayWord()
    {
        isPlaying = true;
        if (SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.PauseMusic();
        }
        yield return new WaitForSeconds(1.0f);

        if (OctoAudioSource.clip != null)
        {
            OctoAudioSource.Play();  
            yield return new WaitForSeconds(OctoAudioSource.clip.length+1.5f);
        }
        else
        {
            Debug.Log("Clip Not Exists");
        }
        
        
        SoundManager.Instance.UnpauseMusic();
        isPlaying = false;



    }
 
}

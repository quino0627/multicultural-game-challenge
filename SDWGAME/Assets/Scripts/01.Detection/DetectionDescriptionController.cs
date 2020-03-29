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
    void Start()
    {
        Octo = transform.parent.gameObject;
        OctoAudioSource = Octo.GetComponent<AudioSource>();
        DescriptionText = transform.Find("DescriptionText").gameObject;
        DescriptionText.GetComponent<TextMeshPro>().text = "다시 듣기";
//        DefaultDescription();
    }

    // 다시 듣기를 눌렀을 떄.
    private void OnMouseUp()
    {
        StartCoroutine(MusicPauseAndStart());
        
    }

    // 백그라운드 음악을 잠시 꺼야합니다.
    IEnumerator MusicPauseAndStart()
    {
        if (OctoAudioSource.clip != null)
        {
            if (SoundManager.Instance.IsMusicPlaying())
            {
                SoundManager.Instance.StopMusic();
            }
            yield return new WaitForSeconds(1f);
            OctoAudioSource.Play();
            yield return new WaitForSeconds(OctoAudioSource.clip.length + 1f);
            SoundManager.Instance.Play_DetectionMusic();
        }
    }
    // 아래 코드들은 탐지과제에서 문어의 다시 듣기 펑션이 없을 때,
    // 유저의 정답/오답 선택에 따른 말풍선 내부의 텍스트값을 변경하는 코드입니다.
    
//    // Update is called once per frame
//    void Update()
//    {
//        this.DescriptionText.GetComponent<TextMeshPro>().text = description_text;
//    }
//
//    public void CorrectAnswer()
//    {
//        description_text = "정답이야. \n 축하해!";
//        Invoke("DefaultDescription", 2.0f);
//    }
//
//    public void WrongAnswer()
//    {
//        description_text = "틀렸어.\n나중에 다시 골라봐\nㅠㅠ";
//        Invoke("DefaultDescription", 2.0f);
//    }
//
//    public void DefaultDescription()
//    {
//        description_text = "소리나는\n글자를\n찾아보자!";
//    }
}

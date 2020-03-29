using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeahorseRightController : MonoBehaviour
{
    private Animator animator;
    private Animation _animation;
    private AudioSource audioSource;
    public Animator waterFallAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();
        this.animator.speed = .25f;
        this.waterFallAnimator = transform.Find("WaterFallAnimation").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
// 다시 듣기 말풍선 클릭시 실행됨
// 이 함수들은 SeahorseRight의 히위 오브젝트인 SpeechBubble으로 이동됨.


//    public void ClickBubble()
//    {
////        SoundManager.Instance.StopMusic();
////        Invoke("PlayWordSound" ,1.0f);
//        SoundManager.Instance.Play_EliminationTutorialSampleSound();
//    }
//
//    private void PlayWordSound()
//    {
////        this.audioSource.Play();
//        Invoke("ReStartMusic", 1.0f);
//    }
//
//    private void ReStartMusic()
//    {
//        SoundManager.Instance.Play_EliminationTutorialSampleSound();
//    }

    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialEliminationManager : MonoBehaviour
{
    
    private DialogueManagerV2 theDM;

    // 상어
    public GameObject Shark;
    public AudioSource SharkAudio;

    public GameObject thinkBubble;
    
    // in SpeechBubble Text
    public GameObject stimulation;
    public TextMeshPro stimulText;
    // in SpeechBubble Text
    public GameObject eliminStimul;
    public TextMeshPro eliminText;
    // in SpeechBubble Text
//    public GameObject syllable;
//    public TextMeshPro syllableText;

    // Fishes
    public GameObject CorrectFish;
    public GameObject WrongFish1;
    public GameObject WrongFish2;
    public GameObject WrongFish3;
    public GameObject WrongFish4;
    
    
    // Flag
    
    // is Clicked Speaker? default: false
    public Boolean isClickedSpeaker;
    // is Clicked Correct Fish? default: false
    public Boolean isClickedCorrectFish;
    
    // 관련된 페이지 이외에서는 스피커 클릭을 막는다.
    public Boolean enableSpeaker;
    // 괸련된 페이지 이외에는 물고기 클릭을 막는다.
    public Boolean enableFish;
    
    

    private Transform SharkArriveTransform;
    public float fastSpeed = 20f;
    public float flowSpeed = 0.006f;
    public bool isSharkArrived;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManagerV2>();

        // Initialize fishes;
        SharkAudio = Shark.GetComponent<AudioSource>();
        SharkArriveTransform = GameObject.Find("SharkArrivePos").transform;
        eliminText = eliminStimul.GetComponent<TextMeshPro>();
        stimulText = stimulation.GetComponent<TextMeshPro>();
//        syllableText = syllable.GetComponent<TextMeshPro>();
        
        // Initialize Some Flags
        isClickedSpeaker = false;
        isClickedCorrectFish = false;

        enableFish = false;
        enableSpeaker = false;

        QuizInit();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void QuizInit()
    {
        StartCoroutine(EliminationTutorialStage());
    }
    
    IEnumerator EliminationTutorialStage()
    {
        yield return new WaitForSeconds(1f);
        // 상어 소리
        SoundManager.Instance.Play_SharkShowedUp();
        
        while (!isSharkArrived)
        {
            yield return new WaitForEndOfFrame();
            float distance = Vector2.Distance(
                Shark.transform.position,
                SharkArriveTransform.position);
            if (distance > 0.01f)
            {
                MoveShark(SharkArriveTransform.position);
                //Debug.Log("Shark moved");
            }
            else
            {
                isSharkArrived = true;

                //Debug.Log("Shark arrived");
            }
        }
        
        yield return new WaitForSeconds(2.0f);
        
        theDM.ShowDialogue();
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Elimination", theDM.GetCurrentSentenceNumber()) + 1f);

        
        
        var tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Elimination", theDM.GetCurrentSentenceNumber()) + 1f);

        
        
        WrongFish1.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        CorrectFish.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        WrongFish2.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        WrongFish3.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        WrongFish4.SetActive(true);
        
        yield return new WaitForSeconds(2.0f);
        
        
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Elimination", theDM.GetCurrentSentenceNumber()) + 1f);

        
        thinkBubble.SetActive(true);
        yield return new WaitForSeconds(.5f);
        // 초성 종성 표시
//        syllableText.text = "종성";
        
        // 자극 제시 ex) 만들
        stimulText.text = "친구";
        SoundManager.Instance.Play_SpeechBubblePop();
        stimulation.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(1f);
        
        // 탈락 자극 제시
        stimulation.SetActive(false);
        SoundManager.Instance.Play_SpeechBubblePop();
        eliminText.text = "ㄴ";
        
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Elimination", theDM.GetCurrentSentenceNumber()) + 1f);
        
        
        
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Elimination", theDM.GetCurrentSentenceNumber()) + 1f);


        // 스피커 클릭 연습하기
        enableSpeaker = true;
        isClickedSpeaker = false;

        while (!isClickedSpeaker)
        {
            yield return new WaitForFixedUpdate();
        }
        enableSpeaker = false;
        theDM.AllowNextStep();
        theDM.StartNextScript();
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Elimination", theDM.GetCurrentSentenceNumber()) + 1f);

        
        
        // 정답 물고기 클릭 연습하기
        enableFish = true;
        isClickedCorrectFish = false;
        while (!isClickedCorrectFish)
        {
            yield return new WaitForFixedUpdate();
        }

        enableFish = false;
        
        theDM.AllowNextStep();
        theDM.StartNextScript();
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Elimination", theDM.GetCurrentSentenceNumber()) + 1f);

        
        theDM.AllowNextStep();
        






//        
//        yield return new WaitForSeconds(1f);
//        SoundManager.Instance.Play_SeahorseWaves();
//        SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
//        Animator left_waterFallAnimator = SeahorseLeft.GetComponent<SeahorseLeftController>().waterFallAnimator;
//        left_waterFallAnimator.Play("WaterFall");
//        yield return new WaitForSeconds(left_waterFallAnimator.GetCurrentAnimatorStateInfo(0).length);
//        SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(false);
//        
//        // 제시어 박스 살리기
////        WordBoxOrigin.transform.position = new Vector3(-2, 1, 0);
//        WordBoxOrigin.SetActive(true);
//        SoundManager.Instance.Play_AlterWordShowedUp();
//        yield return new WaitForSeconds(.4f);
//
//        
//        SoundManager.Instance.Play_SeahorseWaves();
//        SeahorseRight.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
//        Animator right_waterFallAnimator = SeahorseRight.GetComponent<SeahorseRightController>().waterFallAnimator;
//        right_waterFallAnimator.Play("WaterFall");
//
//        yield return new WaitForSeconds(right_waterFallAnimator.GetCurrentAnimatorStateInfo(0).length);
//        SeahorseRight.transform.Find("WaterFallAnimation").gameObject.SetActive(false);
//
//        // 목적어 박스 살리기
////        WordBoxExpect.GetComponent<SpriteRenderer>().color = Color.white;
//        WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color = Color.white;
//        WordBoxExpect.SetActive(true);
//        SoundManager.Instance.Play_AlterWordShowedUp();
//        yield return new WaitForSeconds(.4f);
//        
//        theDM.ShowDialogue();
//        
//        var tmpCount = theDM.GetCurrentSentenceNumber();
//        theDM.AllowNextStep();
//        while (tmpCount == theDM.GetCurrentSentenceNumber())
//        {
//            yield return new WaitForFixedUpdate();
//        }
//        
//        yield return new WaitForSeconds(1.0f);
//        
//        SoundManager.Instance.Play_AlterBubbleShowedUp();
//        WrongBubble_1.SetActive(true);
//        yield return new WaitForSeconds(0.6f);
//        SoundManager.Instance.Play_AlterBubbleShowedUp();
//        WrongBubble_2.SetActive(true);
//        yield return new WaitForSeconds(0.6f);
//        SoundManager.Instance.Play_AlterBubbleShowedUp();
//        WrongBubble_3.SetActive(true);
//        yield return new WaitForSeconds(0.6f);
//        SoundManager.Instance.Play_AlterBubbleShowedUp();
//        WrongBubble_4.SetActive(true);
//        yield return new WaitForSeconds(0.6f);
//        SoundManager.Instance.Play_AlterBubbleShowedUp();
//        CorrectBubble.SetActive(true);
//        yield return new WaitForSeconds(0.6f);
//        
//        
//        
//        
//        tmpCount = theDM.GetCurrentSentenceNumber();
//        theDM.AllowNextStep();
//        while (tmpCount == theDM.GetCurrentSentenceNumber())
//        {
//            yield return new WaitForFixedUpdate();
//        }
//
//        yield return new WaitForSeconds(1f);
//        Color color = WordBoxExpect.GetComponent<SpriteRenderer>().color;
//        while (color.a > 0.0f)
//        {
//            color.a -= 0.2f;
//            WordBoxExpect.GetComponent<SpriteRenderer>().color = color;
//            WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color = color;
//            yield return new WaitForSeconds(0.1f);
//        }
//        is_loading = false;
//        
//        yield return new WaitForSeconds(2.0f);
//        // 샘플 사운드
//        SoundManager.Instance.Play_EliminationTutorialSampleSound();
//
//        yield return new WaitForSeconds(2.0f);
//        
//        tmpCount = theDM.GetCurrentSentenceNumber();
//        theDM.AllowNextStep();
//        while (tmpCount == theDM.GetCurrentSentenceNumber())
//        {
//            yield return new WaitForFixedUpdate();
//        }
//        
//        yield return new WaitForSeconds(2.0f);
//        
//        // 다시 듣기 말풍선 띄우기
//        SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(true);
//
//
//        while (!clickedSpeechBubble)
//        {
//            yield return new WaitForEndOfFrame();
//        }
//        theDM.StartNextScript();
//        
//        
//        while (!clickedCorrectAnswer)
//        {
//            yield return new WaitForEndOfFrame();
//        }
//        
//        theDM.StartNextScript();
//        
//        yield return new WaitForSeconds(2.0f);
//        
//        theDM.AllowNextStep();





    }
    
    
    void MoveShark(Vector2 destination)
    {
        float step = fastSpeed * Time.deltaTime;
        Shark.transform.position = Vector2.MoveTowards(
            Shark.transform.position, destination, step);
    }
}

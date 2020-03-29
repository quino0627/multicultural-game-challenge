using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSynthesisManager : MonoBehaviour
{

    private DialogueManagerV2 theDM;
    
    
    // Crab
    public GameObject Crab;
    
    // Jellyfishes
    public GameObject CorrectJellyfish;
    public GameObject WrongJellyfish1;
    public GameObject WrongJellyfish2;
    public GameObject WrongJellyfish3;
    public GameObject WrongJellyfish4;
    public GameObject WrongJellyfish5;
    
    // Jellyfish positions
    public Transform CorrectJellyfishPosition;
    public Transform WrongJellyfishPosition1;
    public Transform WrongJellyfishPosition2;
    public Transform WrongJellyfishPosition3;
    public Transform WrongJellyfishPosition4;
    public Transform WrongJellyfishPosition5;
    
    
    // Positions
    // 해파리가 도망가는 지점
    public Transform FinishPosition;
    // 크랩이 시작하는 지점
    public Transform CrabStartTransform;
    
    // Flags
    public Boolean allJellyfishArrived;
    private Boolean[] JellyfishArrived = new bool[8];
    public int cntJellyfishArrived;
    private float speed = 8f;
    
    // Flags
    public Boolean isCorrected;


    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManagerV2>();
        isCorrected = false;
        QuizInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuizInit()
    {
        Crab.transform.position = CrabStartTransform.position;
        Crab.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        StartCoroutine(SynthesisTutorialStage());
    }

    IEnumerator SynthesisTutorialStage()
    {
        yield return new WaitForSeconds(1.0f);
        theDM.ShowDialogue();
        var tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        // 게의 상황 설명
//        yield return new WaitForSeconds(3.0f);

        yield return new WaitForSeconds(3f);
        
        StartCoroutine(InitialJellyfish(0, CorrectJellyfish, CorrectJellyfishPosition));
        
        StartCoroutine(InitialJellyfish(1, WrongJellyfish1, WrongJellyfishPosition1));
        StartCoroutine(InitialJellyfish(2, WrongJellyfish2, WrongJellyfishPosition2));
        StartCoroutine(InitialJellyfish(3, WrongJellyfish3, WrongJellyfishPosition3));
        StartCoroutine(InitialJellyfish(4, WrongJellyfish4, WrongJellyfishPosition4));
        StartCoroutine(InitialJellyfish(5, WrongJellyfish5, WrongJellyfishPosition5));
        
        yield return new WaitForSeconds(3.0f);
        
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }


        // 게가 단어를 말 함
        yield return new WaitForSeconds(3.0f);
        SoundManager.Instance.Play_EliminationTutorialSampleSound();
        yield return new WaitForSeconds(3.0f);
        
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        // 해파리들이 날라와서 장전됨
    
        yield return new WaitForSeconds(3.0f);
        
        
        
        

        isCorrected = false;
        while (!isCorrected)
        {
            yield return new WaitForFixedUpdate();
        }
        SoundManager.Instance.Play_ClickedCorrectAnswer();
        
        yield return new WaitForSeconds(3.0f);
        
        theDM.StartNextScript();
        
        yield return new WaitForSeconds(3.0f);
        
        theDM.AllowNextStep();
        
        

    }

    IEnumerator InitialJellyfish(int index, GameObject JellyfishObject, Transform jellyfishTransform)
    {
        //Debug.Log("InitialJellyfish "+i);
        // 얘는 OneShot이 아니고 백그라운드 뮤직
        if (!SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.Play_JellyFishShowedUp();
        }

        while (!JellyfishArrived[index])
        {
            yield return new WaitForEndOfFrame();
            //Debug.Log("InitialJellyfish while문 "+i);
            float distance = Vector2.Distance(
                JellyfishObject.transform.position,
                jellyfishTransform.position);
            //Debug.Log("distance: "+distance);
            if (distance > 0.02f)
            {
                MoveJellyfish(JellyfishObject, jellyfishTransform.position);
            }
            else
            {
                JellyfishArrived[index] = true;

                //Debug.Log("InitialJellyfish JF "+ i +" became true");
            }
        }

        //DragAndDrop script에서 해파리가 제자리로 돌아가기위한 코드
        JellyfishObject.GetComponent<TutorialDragAndDrop>().initialPosition
            = jellyfishTransform.position;
    }
    
    void MoveJellyfish(GameObject JellyfishObject, Vector2 destination)
    {
        float step = speed * Time.deltaTime;
        Transform jellyfish = JellyfishObject.transform;
        /*float distance = Vector2.Distance(choices[i].transform.position,
            destination);*/
        jellyfish.position = Vector2.MoveTowards(jellyfish.position,
            destination, step);
    }
}

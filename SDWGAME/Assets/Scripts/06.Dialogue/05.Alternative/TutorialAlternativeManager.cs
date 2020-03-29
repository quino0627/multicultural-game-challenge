using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialAlternativeManager : MonoBehaviour
{
    private DialogueManagerV2 theDM;
    
    // 다섯개의 통 중 가장 오른쪽에 놓여있는 버블
    public GameObject CorrectBubble;
    
    // 정답이 아닌 버블들
    public GameObject WrongBubble_1;
    public GameObject WrongBubble_2;
    public GameObject WrongBubble_3;
    public GameObject WrongBubble_4;

    public GameObject SeahorseLeft;
    public GameObject SeahorseRight;

    // 처음에 보여지는 글자 (excel에 origin에 해당)
    public GameObject WordBoxOrigin;

    // 목표 글자 (excel에 target에 해당)
    public GameObject WordBoxExpect;

    [HideInInspector] private string[] answerStrings = new string[5];

    public Boolean clickedCorrectAnswer;
    public Boolean clickedSpeechBubble;
    public Boolean is_loading;
    
    
    [SerializeField] private float moveSpeed = 5f;
    public Boolean isMoving = false;
    private float previousDistanceToTouchPos, currentDistanceToTouchPos;
    private Vector3 centerPosition, whereToMove;
    private Rigidbody2D rb;
    
    

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManagerV2>();

        clickedCorrectAnswer = false;
        clickedSpeechBubble = false;
        is_loading = true;
        
        centerPosition = transform.Find("Words").transform.Find("WordCenterPosition").position;
        rb = WordBoxOrigin.GetComponent<Rigidbody2D>();

        QuizInit();

    }

    private void Update()
    {
        if (isMoving)
        {
            currentDistanceToTouchPos = (centerPosition - WordBoxOrigin.transform.position).magnitude;
        }

        if (!is_loading)
        {
            // origin box moving
            previousDistanceToTouchPos = 0;
            currentDistanceToTouchPos = 0;
            isMoving = true;
            whereToMove = (centerPosition - WordBoxOrigin.transform.position).normalized;
            rb.velocity = new Vector2(whereToMove.x * moveSpeed, whereToMove.y * moveSpeed);
        }


        if (currentDistanceToTouchPos > previousDistanceToTouchPos)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }


        if (isMoving)
        {
            previousDistanceToTouchPos = (centerPosition - WordBoxOrigin.transform.position).magnitude;
        }
    }

    public void QuizInit()
    {
        answerStrings[0] = "개";
        answerStrings[1] = "가";
        answerStrings[2] = "글";
        answerStrings[3] = "고";
        answerStrings[4] = "강";
        StartCoroutine(AlternativeTutorialStage());
    }

    IEnumerator AlternativeTutorialStage()
    {
                
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.Play_SeahorseWaves();
        SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
        Animator left_waterFallAnimator = SeahorseLeft.GetComponent<SeahorseLeftController>().waterFallAnimator;
        left_waterFallAnimator.Play("WaterFall");
        yield return new WaitForSeconds(left_waterFallAnimator.GetCurrentAnimatorStateInfo(0).length);
        SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(false);
        
        // 제시어 박스 살리기
//        WordBoxOrigin.transform.position = new Vector3(-2, 1, 0);
        WordBoxOrigin.SetActive(true);
        SoundManager.Instance.Play_AlterWordShowedUp();
        yield return new WaitForSeconds(.4f);

        
        SoundManager.Instance.Play_SeahorseWaves();
        SeahorseRight.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
        Animator right_waterFallAnimator = SeahorseRight.GetComponent<SeahorseRightController>().waterFallAnimator;
        right_waterFallAnimator.Play("WaterFall");

        yield return new WaitForSeconds(right_waterFallAnimator.GetCurrentAnimatorStateInfo(0).length);
        SeahorseRight.transform.Find("WaterFallAnimation").gameObject.SetActive(false);

        // 목적어 박스 살리기
//        WordBoxExpect.GetComponent<SpriteRenderer>().color = Color.white;
        WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color = Color.white;
        WordBoxExpect.SetActive(true);
        SoundManager.Instance.Play_AlterWordShowedUp();
        yield return new WaitForSeconds(.4f);
        
        theDM.ShowDialogue();
        
        var tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(1.0f);
        
        SoundManager.Instance.Play_AlterBubbleShowedUp();
        WrongBubble_1.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        SoundManager.Instance.Play_AlterBubbleShowedUp();
        WrongBubble_2.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        SoundManager.Instance.Play_AlterBubbleShowedUp();
        WrongBubble_3.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        SoundManager.Instance.Play_AlterBubbleShowedUp();
        WrongBubble_4.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        SoundManager.Instance.Play_AlterBubbleShowedUp();
        CorrectBubble.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        
        
        
        
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);
        Color color = WordBoxExpect.GetComponent<SpriteRenderer>().color;
        while (color.a > 0.0f)
        {
            color.a -= 0.2f;
            WordBoxExpect.GetComponent<SpriteRenderer>().color = color;
            WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color = color;
            yield return new WaitForSeconds(0.1f);
        }
        is_loading = false;
        
        yield return new WaitForSeconds(2.0f);
        // 샘플 사운드
        SoundManager.Instance.Play_EliminationTutorialSampleSound();

        yield return new WaitForSeconds(2.0f);
        
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(2.0f);
        
        // 다시 듣기 말풍선 띄우기
        SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(true);


        while (!clickedSpeechBubble)
        {
            yield return new WaitForEndOfFrame();
        }
        theDM.StartNextScript();
        
        
        while (!clickedCorrectAnswer)
        {
            yield return new WaitForEndOfFrame();
        }
        
        theDM.StartNextScript();
        
        yield return new WaitForSeconds(2.0f);
        
        theDM.AllowNextStep();

        
        
        
        
        
    }
}

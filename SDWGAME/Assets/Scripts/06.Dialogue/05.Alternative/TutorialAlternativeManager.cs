using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * 대체과제 튜토리얼의 전체적인 흐름을 담당하는 코드입니다.
 * Canvas > DialogueManager 오브젝트의 <DialogueManager> 컴포넌트에서 제공하는 각종 플래그에 따라,
 * 다이얼로그를 제외한 오브젝트들의 모션을 음직이게 합니다.
 *
 * @author: 송동욱
 */

public class TutorialAlternativeManager : MonoBehaviour
{
    // Canvas > DialogueManager
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

    // 플래그들
    public Boolean clickedCorrectAnswer;
    public Boolean clickedSpeechBubble;
    public Boolean clickedOriginalWordBox;
    
    
    
    // WordBox들을 움직이기 위해 필요한 변수들
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
        clickedOriginalWordBox = false;
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
        // 해마가 물대포를 발사하는 애니메이션
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

        // 해마가 물대포를 발사하는 애니메이션
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
        
        
        // 다이얼로그 화면에 나타남(클릭 불가능)
        theDM.ShowDialogue();
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Alternative", theDM.GetCurrentSentenceNumber()) + 1f);


        var tmpCount = 0;
        
        // 헤당 코드블럭은 다이얼로그를 클릭 가능하게 하고, 유저가 다이얼로그를 클릭했는지 계속 검사합니다.
        // 유저가 클릭한 경우 해당 코드블럭을 벗어나 다음 동작으로 진행됩니다.
        {
            tmpCount = theDM.GetCurrentSentenceNumber();
            theDM.AllowNextStep();
            while (tmpCount == theDM.GetCurrentSentenceNumber())
            {
                yield return new WaitForFixedUpdate();
            }
        }

        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Alternative", theDM.GetCurrentSentenceNumber()) + 1f);

        // 버블들을 화면에 표시
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

        {
            tmpCount = theDM.GetCurrentSentenceNumber();
            theDM.AllowNextStep();
            while (tmpCount == theDM.GetCurrentSentenceNumber())
            {
                yield return new WaitForFixedUpdate();
            }
        }

        // 왼쪽 상자를 눌러봐! 음성이 들어갈 자리
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Alternative", theDM.GetCurrentSentenceNumber()) + 1f);


        clickedCorrectAnswer = false;
        while (!clickedOriginalWordBox)
        {
            yield return new WaitForEndOfFrame();
        }
        theDM.StartNextScript();
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Alternative", theDM.GetCurrentSentenceNumber()) + 1f);

        
        yield return new WaitForSeconds(1f);
        // 목표 단어 사라짐
        Color color = WordBoxExpect.GetComponent<SpriteRenderer>().color;
        while (color.a > 0.0f)
        {
            color.a -= 0.2f;
            WordBoxExpect.GetComponent<SpriteRenderer>().color = color;
            WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color = color;
            yield return new WaitForSeconds(0.1f);
        }
        // 블럭 이동
        is_loading = false;
        
        yield return new WaitForSeconds(2.0f);
        // 샘플 사운드
        SoundManager.Instance.Play_AlternativeOriginSound();

        yield return new WaitForSeconds(2.0f);

        
        {
            tmpCount = theDM.GetCurrentSentenceNumber();
            theDM.AllowNextStep();
            while (tmpCount == theDM.GetCurrentSentenceNumber())
            {
                yield return new WaitForFixedUpdate();
            }
        }
        
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Alternative", theDM.GetCurrentSentenceNumber()) + 1f);

        
        // 다시 듣기 말풍선 띄우기
        SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(true);


        clickedSpeechBubble = false;
        while (!clickedSpeechBubble)
        {
            yield return new WaitForEndOfFrame();
        }
        theDM.StartNextScript();
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Alternative", theDM.GetCurrentSentenceNumber()) + 1f);


        clickedCorrectAnswer = false;
        while (!clickedCorrectAnswer)
        {
            yield return new WaitForEndOfFrame();
        }
        
        theDM.StartNextScript();
        
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(
            SoundManager.Instance.Play_Narration("Alternative", theDM.GetCurrentSentenceNumber()) + 1f);
        
        
        theDM.AllowNextStep();

        
        
        
        
        
    }
}

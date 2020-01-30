using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BarrelController : MonoBehaviour
{

    
    // 통 위에 표시되는 글자
    public Text currentText;
    // 틀렸을 때 애니메이션
    public GameObject wrongAnimation;
    // 맞췄을 때 애니메이션
    public GameObject rightAnimation;

    private GameObject director;
    private GameObject description;

    // 배럴이 여러번 클릭되는 것을 방지한다.
    // 기본값은 false, 정답일 경우 한번 클릭된 이후에는 true로 
    private bool preventSeveralTouch = false;

    // Start is called before the first frame update
    void Start()
    {
        this.director = GameObject.Find("GameDirector");
        this.description = GameObject.Find("DescriptionBubble");
        preventSeveralTouch = false;
    }

    private void OnEnable()
    {
        preventSeveralTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private void OnMouseUp()
    {
//        Debug.Log(!IsPointerOverUIObject()); //true
//        if (IsPointerOverUIObject()) //false
        if(!preventSeveralTouch)
        {
            BarrelClicked();
        }
        
    }


    //배럴들을 눌렀을 때 정답인지? 아닌지? 
    public void BarrelClicked()
    {
        Transform tmpTransform = GameObject.Find("QuizContainer").transform;
        
        GameObject childText = transform.Find("Word").gameObject;
        // 현재 클릭된 배럴의 글자
        string currentBarrelText = childText.GetComponent<TextMeshPro>().text;
        
        //wj
        tmpTransform.GetComponent<DetectionQuizManager>().chosenAns = currentBarrelText;
        
        // 해당 stage의 정답 string
        string currentStageAnswerText = DetectionQuizManager.answer_string_list[DetectionQuizManager.stage_no];

        // 만약 유저가 클릭한 배럴에 쓰여 있는 글자가 해당 stage의 정답과 일치하면,
        if (currentBarrelText == currentStageAnswerText)
        {
            SoundManager.Instance.Play_ClickedCorrectAnswer();
            description.GetComponent<DetectionDescriptionController>().CorrectAnswer();
            preventSeveralTouch = true;
            // 점수 올리기
            this.director.GetComponent<GameDirector>().GetPoint(100);
            // 다음 stage로 넘어가기
            //Transform tmpTransform = GameObject.Find("QuizContainer").transform;
            tmpTransform.GetComponent<DetectionQuizManager>().StageOver();

            // total clicked 와 total correct 를 올린다.
            tmpTransform.GetComponent<DetectionQuizManager>().total_clicked++;
            tmpTransform.GetComponent<DetectionQuizManager>().total_correct++;
            
            //wj
            tmpTransform.GetComponent<DetectionQuizManager>().isUserRight = true;
        }
        else
        {
            // 점수 그대로 두기
            // 애니메이션 출력
            // 말풍선 출력
            
            // 틀렸으니 까 total clicked만 올린다.
            SoundManager.Instance.Play_ClickedWrongAnswer();
            description.GetComponent<DetectionDescriptionController>().WrongAnswer();
//            Transform tmpTransform = GameObject.Find("QuizContainer").transform;
            tmpTransform.GetComponent<DetectionQuizManager>().total_clicked++;
        }



    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

//    private void OnMouseDown()
//    {
//        throw new NotImplementedException();
//    }
}

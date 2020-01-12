using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    
    // 버블 위에 표시되는 글자
    public TextMeshPro currentText;
    // 틀렸을 때 애니메이션
    public GameObject wrongAnimation;
    // 맞췄을 때 애니메이션
    public GameObject rightAnimation;
    // director
    private GameObject director;
    private GameObject description;

    private GameObject manager;
    
    // Start is called before the first frame update
    void Start()
    {
        this.director = GameObject.Find("AlternativeGameDirector");
        this.description = GameObject.Find("DescriptionBubble");
        this.manager = GameObject.Find("QuizContainer");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        BubbleClicked();
    }

    public void BubbleClicked()
    {
        if (manager.GetComponent<AlternativeQuizManager>().is_loading)
        {
            return;
        }
        GameObject childText = transform.Find("Text").gameObject;
        // 현재 클릭된 배럴의 글자
        string currentBubbleText = childText.GetComponent<TextMeshPro>().text;
        // 해당 stage의 정답 string
        string currentStageAnswerText = AlternativeQuizManager.answer_string_list[AlternativeQuizManager.stage_no];
        
        // 만약 유저가 클릭한 버블에 쓰여 있는 글자가 해당 stage의 정답과 일치하면
        if (currentBubbleText == currentStageAnswerText)
        {
            description.GetComponent<AlternativeDescriptionController>().CorrectAnswer();
            // 점수 올리기
            this.director.GetComponent<AlternativeGameDirector>().GetPoint(100);
            Transform tmpTransform = GameObject.Find("QuizContainer").transform;
            tmpTransform.GetComponent<AlternativeQuizManager>().StageOver();
            
            // 점수판에 올라가는 것
            tmpTransform.GetComponent<AlternativeQuizManager>().total_clicked++;
            tmpTransform.GetComponent<AlternativeQuizManager>().total_correct++;
        }
        else
        {
            description.GetComponent<AlternativeDescriptionController>().WrongAnswer();
            // 애니메이션 출력
            Transform tmpTransform = GameObject.Find("QuizContainer").transform;
            tmpTransform.GetComponent<AlternativeQuizManager>().total_clicked++;
        }
    }
}

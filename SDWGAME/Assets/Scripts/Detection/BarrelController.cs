using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarrelController : MonoBehaviour
{

    
    // 통 위에 표시되는 글자
    public Text currentText;
    // 틀렸을 때 애니메이션
    public GameObject wrongAnimation;
    // 맞췄을 때 애니메이션
    public GameObject rightAnimation;

    private GameObject director;

    // Start is called before the first frame update
    void Start()
    {
        this.director = GameObject.Find("GameDirector");
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private void OnMouseDown()
    {
        
        BarrelClicked();
    }


    //배럴들을 눌렀을 때 정답인지? 아닌지? 
    public void BarrelClicked()
    {
        GameObject childText = transform.Find("Word").gameObject;
        // 현재 클릭된 배럴의 글자
        string currentBarrelText = childText.GetComponent<TextMeshPro>().text;
        // 해당 stage의 정답 string
        string currentStageAnswerText = DetectionQuizManager.answer_string_list[DetectionQuizManager.stage_no];

        // 만약 유저가 클릭한 배럴에 쓰여 있는 글자가 해당 stage의 정답과 일치하면,
        if (currentBarrelText == currentStageAnswerText)
        {
            // 점수 올리기
            this.director.GetComponent<GameDirector>().GetPoint(100);
            // 다음 stage로 넘어가기
            Transform tmpTransform = GameObject.Find("QuizContainer").transform;
            tmpTransform.GetComponent<DetectionQuizManager>().StageOver();

        }
        else
        {
            // 점수 그대로 두기
            // 애니메이션 출력
            // 말풍선 출력
        }



    }

//    private void OnMouseDown()
//    {
//        throw new NotImplementedException();
//    }
}

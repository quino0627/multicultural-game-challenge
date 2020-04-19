using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDragAndDrop : MonoBehaviour
{
   public Transform ObjectPlace;
    public Vector2 initialPosition;
    private Vector2 mousePosition;
    private float deltaX, deltaY;
    //public Transform AnswerTransform;

    private TutorialWrongJellyfishController wrongMoveScript;
    private TutorialCorrectJellyfishController correctMoveScript; 

    public static bool locked;

    private bool isRight;

    private GameObject QuizManager; 
    private SpreadChoices spreadChoicesScript;

    private void Start()
    {
        //initialPosition은 SpreadChoices.cs에서 세팅됨
        if (QuizManager = GameObject.FindWithTag("QuizManager"))
        {
            //QuizManager = GameObject.FindWithTag("QuizManager");
            spreadChoicesScript = QuizManager.GetComponent<SpreadChoices>();
        }
    }

 
    private void OnMouseDown()
    {
        if (!GameObject.FindObjectOfType<TutorialSynthesisManager>().enableJellyFish) return;
        if (Time.timeScale == 0) return;
        Debug.Log("Crab2 OnMouseDown");
        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Crab2 OnMouseDown EventSystem");
            return;
        }*/
        if (!locked)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
            Debug.Log("Crab2 OnMouseDown deltaXY");
        }
    }

    private void OnMouseDrag()
    {
        if (!GameObject.FindObjectOfType<TutorialSynthesisManager>().enableJellyFish) return;
        if (Time.timeScale == 0) return;
        Debug.Log("Crab2 OnMouseDrag");
        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Crab2 OnMouseDrag EventSystem");
            return;
        }*/
        if (!locked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX,mousePosition.y - deltaY);
            Debug.Log("Crab2 OnMouseDrag transform position");
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("MOUSE UP");
        if (ObjectPlace != null) 
        {
            
            //해파리를 원의 일정범위내에 놓으면...
            if (Math.Abs(transform.position.x - ObjectPlace.position.x) <= 0.7f &&
                Math.Abs(transform.position.y - ObjectPlace.position.y) <= 0.7f)
            {
                //놓여짐
                transform.position = new Vector2(ObjectPlace.position.x, ObjectPlace.position.y);
                
             
                
                
                
                //crab를 점프시키기
                //Projectile script = GameObject.Find("Crab").GetComponent<Projectile>();
                //script.justJump = true;
                //script.Launch();

                //transform.SetParent(AnswerTransform);
                
                /*StageClear script2 = GameObject.Find("AnswerPlatform").GetComponent<StageClear>();
                script2.numAns++;
                transform.SetParent(AnswerTransform);*/
                //StartCoroutine("UpdateNumAns");

                wrongMoveScript = GetComponent<TutorialWrongJellyfishController>();
                correctMoveScript = GetComponent<TutorialCorrectJellyfishController>();
                if (wrongMoveScript == null)
                {
                    Debug.Log("GGOOODD");
                    correctMoveScript.onCircle = true;
                }
                else if(correctMoveScript == null)
                {
                    wrongMoveScript.onCircle = true;
                }
                //moveScript.initialPosition = initialPosition;
               
                
            }
            else //원이 아닌 곳에 놓이면 해파리를 제자리로
            {
                Debug.Log("not on circle");
                transform.position = new Vector2(initialPosition.x,initialPosition.y);
            }
        }
        
    }



}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;

public class DragAndDrop : MonoBehaviour
{
    public Transform ObjectPlace;
    public Vector2 initialPosition;
    private Vector2 mousePosition;

    private float deltaX, deltaY;
    //public Transform AnswerTransform;

    public bool bLocked;

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

    private void Update()
    {
        /*//Debug.Log(spreadChoicesScript.watch.IsRunning);
        if (!(spreadChoicesScript.watch.IsRunning) || spreadChoicesScript.isUserRight)
        {
            //Debug.Log("bLock: " + bLocked);
            bLocked = true;
        }*/

        if (spreadChoicesScript.watch.IsRunning && !spreadChoicesScript.isUserRight)
        {
            bLocked = false;
        }
        else
        {
            bLocked = true;
        }
    }

    private void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        if (!bLocked)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
            //Debug.Log("Crab2 OnMouseDown deltaXY");
        }
    }

    private void OnMouseDrag()
    {
        if (Time.timeScale == 0) return;
        //Debug.Log("Crab2 OnMouseDrag");
        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Crab2 OnMouseDrag EventSystem");
            return;
        }*/
        if (!bLocked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
            Debug.Log("Crab2 OnMouseDrag transform position");
        }
    }

    private void OnMouseUp()
    {
        if (bLocked)
        {
            return;
        }

        if (ObjectPlace != null)
        {
            //해파리를 원의 일정범위내에 놓으면...
            if (Math.Abs(transform.position.x - ObjectPlace.position.x) <= 0.7f &&
                Math.Abs(transform.position.y - ObjectPlace.position.y) <= 0.7f)
            {
                //놓여짐
                transform.position = new Vector2(ObjectPlace.position.x, ObjectPlace.position.y);

                spreadChoicesScript.PlusTotalTry();


                //crab를 점프시키기
                //Projectile script = GameObject.Find("Crab").GetComponent<Projectile>();
                //script.justJump = true;
                //script.Launch();

                //transform.SetParent(AnswerTransform);

                /*StageClear script2 = GameObject.Find("AnswerPlatform").GetComponent<StageClear>();
                script2.numAns++;
                transform.SetParent(AnswerTransform);*/
                //StartCoroutine("UpdateNumAns");

                MoveJellyfish moveScript = GetComponent<MoveJellyfish>();
                //moveScript.initialPosition = initialPosition;
                moveScript.onCircle = true;
                spreadChoicesScript.chosenAns.Add(moveScript.child.text);
            }
            else //원이 아닌 곳에 놓이면 해파리를 제자리로
            {
                Debug.Log("not on circle");
                transform.position = new Vector2(initialPosition.x, initialPosition.y);
            }
        }
    }

    IEnumerator UpdateNumAns()
    {
        yield return new WaitForSeconds(2.0f);
        StageClear script2 = GameObject.Find("AnswerPlatform").GetComponent<StageClear>();
        script2.numAns++;
        Debug.Log("updateNumAns");
        //isRight = false;
    }
}
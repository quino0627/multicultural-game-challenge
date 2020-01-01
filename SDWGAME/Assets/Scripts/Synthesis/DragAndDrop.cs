using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Transform ObjectPlace;
    private Vector2 initialPosition;
    private Vector2 mousePosition;
    private float deltaX, deltaY;
    public Transform AnswerTransform;

    public static bool locked;

    private bool isRight;
    
    private void Start()
    {
        initialPosition = gameObject.transform.position;
    }

    private void OnMouseDown()
    {
        if (!locked)
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
            
        }
    }

    private void OnMouseDrag()
    {
        if (!locked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX,mousePosition.y - deltaY);
        }
    }

    private void OnMouseUp()
    {
        if (ObjectPlace != null) //정답인 해파리만 objectplace가 있음.
        {
            //해파리를 원의 일정범위내에 놓으면...
            if (Math.Abs(transform.position.x - ObjectPlace.position.x) <= 0.5f &&
                Math.Abs(transform.position.y - ObjectPlace.position.y) <= 0.5f)
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

                MoveJellyfish moveScript = GetComponent<MoveJellyfish>();
                moveScript.initialPosition = initialPosition;
                moveScript.onCircle = true;
                
            }
        }
        else //원이 아닌 곳에 놓이면 해파리를 제자리로
        {
            transform.position = new Vector2(initialPosition.x,initialPosition.y);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MovingScript와 차이점은
// 버블의 경우에는 마우스 호버 시에 상하 움직임이 멈춰야 하기 때문이다.
public class BubbleMovingController : MonoBehaviour
{
    
    public float acc = 0.0005f, defaultSpeed =  0.0005f;
    int var = 1, var2 = 1;
    float speed = 0;
    public int cnt = 0, cntMax = 30;

    // false일 경우 상하운동하지않음
    private bool isMoving;
    // Start is called before the first frame update
    private void Start()
    {
        speed = defaultSpeed;
        isMoving = true;
    }
    
    void Update()
    {
        if(Time.timeScale == 0)return;
        if (!isMoving)
        {
            return;
        }
        
        cnt += var;
        speed += acc * var2;
        this.transform.Translate(new Vector3(0, speed* var*-1, 0));  //이동
        if (cnt % (cntMax/2) == 0) var2 = var2 * -1;
        if (cnt == cntMax)
        {
            var = -1;
            speed = defaultSpeed;
        }
        else if (cnt == 0)
        {
            var = 1;
            speed = defaultSpeed;
        }
    }

    private void OnMouseEnter()
    {
        isMoving = false;
        Debug.Log("isMoving is " + isMoving);
    }

    private void OnMouseExit()
    {
        isMoving = true;
        Debug.Log("isMoving is " + isMoving);
    }

 
}

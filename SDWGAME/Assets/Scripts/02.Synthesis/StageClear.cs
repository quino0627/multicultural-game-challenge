using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear : MonoBehaviour
{
    [SerializeField]
    public int numAns = 0; //현재까지 맞춘 글자수
    public int threshold = 1; //글자 개수 ex. 가지 - 2개
    public float speed;
   
    // Update is called once per frame
    void Update()
    {
        
        if (numAns == threshold) // 현재까지 맞춘 글자수가 정답과 맞으면
        {
            Debug.Log("numAns==threshold");
            transform.Translate(transform.right*speed*Time.deltaTime);
            
            //Projectile script = GameObject.Find("Crab").GetComponent<Projectile>(); //crab가 clear지점으로 점프
            //script.stopPosition = GameObject.Find("Clear").transform.position;
            //script.Launch();
            //numAns = 0;
            
            
            //crab 애니메이션
            
            //stage load.
        }
        
    }

    
}

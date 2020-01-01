using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class MoveJellyfish : MonoBehaviour
{
    //CrabMove.cs와 같이 보시길 권장합니다.
    public float speed = 2f;
    public bool onCircle;
    public bool isCorrect;
    public GameObject Crab;
    public Vector2 aboardPosition;
    public Vector2 departPosition;
    private Animator crabAnimator;
    public Vector2 initialPosition;
    public bool isCrabAboard;

    public bool willCrabReturn; //이걸 안해주면 해파리가 데려다주고 다시 탑승지점으로감...
    // Start is called before the first frame update
    void Start()
    {
        crabAnimator = Crab.GetComponent<Animator>();
        aboardPosition = GameObject.Find("AboardPosition").transform.position;
        departPosition = GameObject.Find("DepartPosition").transform.position;
        willCrabReturn = true;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        float fly = 2 * speed * Time.deltaTime;
        if (!onCircle)
        {
            return; //해파리가 circle에 놓여져 있지 않으면 return
        }
        
            Debug.Log("Jellyfish onCircle");
         
        // 1. 해파리를 move toward crab의 탑승지점
        if(willCrabReturn)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                aboardPosition, step);
        }    
        
        /*** 오답인경우 ***/
        /*if (!isCorrect)
        {
            // move toward initial position 
            transform.position = Vector2.MoveTowards(transform.position,
                initialPosition, step);
        }*/
        
        /*** 정답인 경우 ***/
        //else
        //{

        //오답 코드 작성하면 주석풀기
        //willCrabReturn = false; //Crab will not return to startPosition

            // 2. when arrive, invoke crab animation
            if (Mathf.Abs(transform.position.x - aboardPosition.x) <= 0.01f &&
                !isCrabAboard)
            {
                Debug.Log("Jellyfish aboard ready");
                crabAnimator.SetFloat("WalkSpeed", 2f);
                isCrabAboard = true;
                
                //오답 코드 작성하면 밑줄 삭제
                willCrabReturn = false; //Crab will not return to startPosition
            }

            //CrabMove.cs
            //3. crab 이동
            //4. crab 탑승후 멈춤

            // 5. 목적지 향해 move
            if (crabAnimator.GetFloat("WalkSpeed") <= 0 && isCrabAboard)
            {
                // 5-1 목적지: 다른 해파리(글자수 2개 이상)
                // 5-2 목적지: clear 지점
                transform.position = Vector2.MoveTowards(transform.position,
                    departPosition, fly);
            }
            
            // 6. 목적지 도달하면 stop
            if (Mathf.Abs(transform.position.x - departPosition.x) <= 0.1f
                && isCrabAboard)
            {
                Debug.Log("Jellyfish depart ready");
                crabAnimator.SetFloat("WalkSpeed", 2f);
                isCrabAboard = false;
            }
            
    }

    
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JfSuccess : MonoBehaviour
{

    public int currCntCorrAns;  // 현재 정답 글자수
    public int cntCorrAns; // 정답 글자수
    public float speed = 2f;
    //public bool onCircle;
    //public bool sparked;
    public GameObject Crab;
    public GameObject Carrier;

    //public GameObject Spark;
    public Vector2 aboardPosition;
    public Vector2 departPosition;
    private Animator crabAnimator;
    //public Transform InitialTransform;
    public bool isCrabAboard;
    public bool isSuccess;
    public bool willCrabReturn;
    // Start is called before the first frame update

    public bool tmpValue;
    void Start()
    {
        //일단 정답 하나인 경우
        //Carrier = GameObject.FindGameObjectWithTag("Carrier");
        crabAnimator = Crab.GetComponent<Animator>();
        aboardPosition = GameObject.Find("AboardPosition").transform.position;
        departPosition = GameObject.Find("DepartPosition").transform.position;
        willCrabReturn = true;
        tmpValue = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (CheckSuccess())
        {
            //Carrier.SetActive(true);
            if (!tmpValue)
            {
                GameObject.Find("QuizManager").GetComponent<SpreadChoices>().PlusTotalCorrectStage();
                tmpValue = true;
//                Debug.Log("GOOGGOOD");
                
            }
            FinishAnimation();
        }
    }

    void FinishAnimation()
    {
        float step = speed * Time.deltaTime; //움직이는 속도
        float fly = 2 * speed * Time.deltaTime; //빠른 속도...
        
        // 1. 해파리를 move toward crab의 탑승지점
        if (willCrabReturn)
        {
            Carrier.transform.position = Vector2.MoveTowards(Carrier.transform.position,
                aboardPosition, step);
        }
            
        //오답 코드 작성하면 주석풀기
        //willCrabReturn = false; //Crab will not return to startPosition

        // 2. when arrive, invoke crab animation
        if (Mathf.Abs(Carrier.transform.position.x - aboardPosition.x) <= 0.01f &&
            !isCrabAboard)
        {
            //Debug.Log("Jellyfish aboard ready");
            crabAnimator.SetFloat("WalkSpeed", 2f);
            isCrabAboard = true;
            Crab.GetComponent<CrabMove>().isAboarding = true;
            //오답 코드 작성하면 밑줄 삭제
            willCrabReturn = false; //Crab will not return to startPosition
        }

        //CrabMove.cs
        //3. crab 이동
        //4. crab 탑승후 멈춤
//        Debug.Log("BBBBBBBB");

        // 5. 목적지 향해 move
        if (crabAnimator.GetFloat("WalkSpeed") <= 0 && isCrabAboard)
        {
            // 5-1 목적지: 다른 해파리(글자수 2개 이상)
            // 5-2 목적지: clear 지점
            Carrier.transform.position = Vector2.MoveTowards(Carrier.transform.position,
                departPosition, fly);
        }

        // 6. 목적지 도달하면 stop
        if (Mathf.Abs(Carrier.transform.position.x - departPosition.x) <= 0.1f
            && isCrabAboard)
        {
//            Debug.Log("Jellyfish depart ready");
            crabAnimator.SetFloat("WalkSpeed", 2f);
            isCrabAboard = false;
            Crab.transform.SetParent(null);
        }
    }

    public bool CheckSuccess()
    {
        //Debug.Log("currCntCorrAns: "+ currCntCorrAns);
        if (cntCorrAns == currCntCorrAns)
        {
//            Debug.Log("ASDFASDF");
            return true;
        }
        
        
        return false;
    }
    
    
}

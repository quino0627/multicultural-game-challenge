using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Diagnostics;
public class JfSuccess : MonoBehaviour
{

    public GameObject descriptionBubble;
    
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

    public GameObject QuizManager;
    public SpreadChoices spreadChoicesScript;
    
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

        descriptionBubble = Crab.transform.Find("DescriptionBubble").gameObject;
        QuizManager = GameObject.FindWithTag("QuizManager");
        spreadChoicesScript = QuizManager.GetComponent<SpreadChoices>();
    }

    // Update is called once per frame
    void Update()
    {

        if (CheckSuccess())
        {
            spreadChoicesScript.responseTime = spreadChoicesScript.watch.ElapsedMilliseconds;
            //Carrier.SetActive(true);
            if (!tmpValue)
            {
                GameObject.Find("QuizManager").GetComponent<SpreadChoices>().PlusTotalCorrectStage();
                tmpValue = true;
//                Debug.Log("GOOGGOOD");
                descriptionBubble.GetComponent<SynthesisDescriptionController>().CorrectAnswer();
            }

            spreadChoicesScript.isUserRight = true;
            Invoke("FinishAnimation",1.2f);
        }
    }

    void FinishAnimation()
    {
        float step = speed * Time.deltaTime; //움직이는 속도
        float fly = 2 * speed * Time.deltaTime; //빠른 속도...
        
        // 1. 해파리를 move toward crab의 탑승지점
        if (willCrabReturn)
        {
            descriptionBubble.SetActive(false);
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
            crabAnimator.SetFloat("WalkSpeed", 2f);
            isCrabAboard = false;
            Crab.transform.SetParent(null);
        }
    }

    public bool CheckSuccess()
    {
        if (cntCorrAns == currCntCorrAns)
        {
            spreadChoicesScript.responseTime = spreadChoicesScript.watch.ElapsedMilliseconds;
            return true;
        }
        return false;
    }
    
    
}

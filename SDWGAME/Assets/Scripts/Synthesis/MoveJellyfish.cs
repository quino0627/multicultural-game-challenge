using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MoveJellyfish : MonoBehaviour
{
    //CrabMove.cs와 같이 보시길 권장합니다.
    public float speed = 2f;
    public bool onCircle;
    public bool sparked;
    public bool ischecked;
    public GameObject Crab;
    public GameObject Spark;
    public Vector2 aboardPosition;
    public Vector2 departPosition;
    private Animator crabAnimator;
    public Transform InitialTransform;
    public bool isCrabAboard;
    public GameObject QuizManager;
    public bool willCrabReturn; //이걸 안해주면 해파리가 데려다주고 다시 탑승지점으로감...
    public GameObject Carrier;
    public SpreadChoices spreadChoicesScript;
    public SpreadChoices tmp;

    public TextMeshPro child;
    // Start is called before the first frame update
    void Start()
    {
        /*crabAnimator = Crab.GetComponent<Animator>();
        aboardPosition = GameObject.Find("AboardPosition").transform.position;
        departPosition = GameObject.Find("DepartPosition").transform.position;
        willCrabReturn = true;*/
        tmp = QuizManager.GetComponent<SpreadChoices>();
        child = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; //움직이는 속도
        float fly = 2 * speed * Time.deltaTime; //빠른 속도...
        
        if (!onCircle) //onCircle은 DragAndDrop.cs에서 설정해줌
        {
            return; //해파리가 circle에 놓여져 있지 않으면 return
        }
        //Debug.Log("Jellyfish onCircle");
        
        
        /*** 오답인경우 ***/
        if (gameObject.CompareTag("WrongAns"))
        {
            //json을 위해 chosenAns;
            tmp.chosenAns.Add(child.text); 
            
            StartCoroutine(ChoseWrongAnswer(fly));
            Debug.Log("WRONG!!!");
            
        }
        
        
        /*** 정답인 경우 ***/ 
        //여기서는 단순히 글자처리. 
        //모든 글자 성공후 animation은 JfSuccess.js
        if (gameObject.CompareTag("CorrectAns") && !ischecked)
        {
            Debug.Log("");
            QuizManager.GetComponent<SpreadChoices>().PlusTotalCorrect();
            //QuizManager.GetComponent<SpreadChoices>().PlusTotalTry();
            Carrier.SetActive(true);
            //TextMeshPro child = GetComponentInChildren<TextMeshPro>();
            /*GameObject quizmanager = GameObject.Find("QuizManager");
            SpreadChoices tmp = quizmanager.GetComponent<SpreadChoices>();*/
            
            
            
            // level 올라가면 그저 정답간에 순서 맞춰서 배정하면됨
            for (int i = 0; i < tmp.corrAnsCnt+tmp.wrongAnsCnt; i++)
            {
                if (child.text == tmp.choiceTexts[tmp.corrAnsPosIndex[i]].text)
                {
                    transform.SetParent(tmp.PickedJfPos[i]);
                    transform.localPosition= Vector2.zero;
                }
            }
            
            //quizManager에서 맞춘 글자수 올려줌.
            JfSuccess script = QuizManager.GetComponent<JfSuccess>();
            script.currCntCorrAns++;
            ischecked = true;

            //json을 위해 chosenAns;
            tmp.chosenAns.Add(child.text); 
            
            //정답란에 글자를 띄워줌
            for (int i = 0; i < tmp.corrAnsCnt+tmp.wrongAnsCnt; i++)
            {
                if (child.text == tmp.choiceTexts[tmp.corrAnsPosIndex[i]].text)
                {
                    tmp.PickedAnswer[i].SetActive(true);
                }
            }
            
        }


    }

    IEnumerator ChoseWrongAnswer(float step)
    {
        //spark animation
        if (!sparked)
        {
            Debug.Log("MAYBe ONLY ONCE?");
            
            // totaltried ++
            QuizManager.GetComponent<SpreadChoices>().PlusTotalTry();
            
            Instantiate(Spark, transform.position, Quaternion.identity);
            sparked = true;
            
        }
        
        yield return new WaitForSeconds(1.2f);
        
        // move toward initial position
        transform.position = Vector2.MoveTowards(transform.position,
            InitialTransform.position, step);

        
        //다시 도착했을때
        float distance = Vector2.Distance(transform.position, InitialTransform.position);
        if (distance < 0.01f)
        {
            onCircle = false;
            sparked = false;
            
        }
    }
    
    
    
    
}


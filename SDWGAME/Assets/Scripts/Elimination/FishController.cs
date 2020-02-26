using System;
using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishController : MonoBehaviour
{
    public Text currentText;
    private GameObject QuizManager;
    public GameObject Shark;
    private FishShowAnswer script;
    private bool fishSwallown;
    public Transform SharkMouthTransform;
    public Transform SwallowTransform;
    public float speed = 2f;
    public float sharkSpeed = 20f;
    private GameObject speaker;
    public bool isCaught;
    public bool isSharkToFish;

    public GameObject xSign;

    public GameObject thinkBubble;

    // Start is called before the first frame update
    void Start()
    {
        QuizManager = GameObject.Find("QuizManager");
        script = QuizManager.GetComponent<FishShowAnswer>();
        speaker = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        if (script.canClick)
        {
            StartCoroutine(FishClicked());
        }
    }

    IEnumerator FishClicked()
    {
        //현재 클릭된 물고기 글자
        string currFishText = GetComponentInChildren<TextMeshPro>().text;
        script.chosenAns.Add(currFishText);
        //해당 stage의 정답 
        string currStageAnsText = script.choiceTexts[script.ansPosIndex[0]].text;

        if (currFishText == currStageAnsText)
        {
            // total clicked와 total correct ++
            script.PlusTotalClick();
            script.PlusTotalCorrect();

            // 소리
            SoundManager.Instance.Play_ClickedCorrectAnswer();

            //점수 올리기
//            director.GetComponent<EliminationDirector>().GetPoint(100);
            script.isUserRight = true;
            isCaught = true;

            //상어가 먹으러 온다.
            //Shark.GetComponent<Animator>().SetTrigger("Swim");
            thinkBubble.SetActive(false);
            StartCoroutine(SharkToFish());

            //setactive(false) speaker
            speaker.SetActive(false);

            //상어 먹기시작
            Shark.GetComponent<Animator>().SetTrigger("Eat");
            StartCoroutine(SharkEating());
        }
        else
        {
            //오답을 골랐을시 코드들
            
            
            SoundManager.Instance.Play_ClickedWrongAnswer();
            script.PlusTotalClick();
            
            //상어가 x표시함
            script.eliminStimul.SetActive(false);
            xSign.SetActive(true);
            yield return new WaitForSeconds(1f);

            //그리고 다시 자극 제시 
            /*script.eliminStimul.SetActive(true);
            xSign.SetActive(false);*/
            
            //가 아닌 넘어감.
            script.bFail = true;
        }
    }

    IEnumerator SharkToFish()
    {
        yield return new WaitForSeconds(.5f);
        SoundManager.Instance.Play_SharkShowedUp();
        while (!isSharkToFish)
        {
            yield return new WaitForEndOfFrame();
            float distance = Vector2.Distance(
                Shark.transform.position,
                transform.position);
            if (distance > 0.04f)
            {
                MoveShark(transform.position);
                //Debug.Log("Shark moved");
            }
            else
            {
                isSharkToFish = true;
                //Debug.Log("Shark arrived");
            }
        }
    }

    void MoveShark(Vector2 destination)
    {
        float step = sharkSpeed * Time.deltaTime;
        Shark.transform.position = Vector2.MoveTowards(
            Shark.transform.position, destination, step);
    }

    IEnumerator SharkEating()
    {
        yield return new WaitForSeconds(.5f);
        SoundManager.Instance.Play_SharkEatingFish();

        while (!fishSwallown)
        {
            yield return new WaitForEndOfFrame();


            float distance = Vector2.Distance(
                transform.position,
                SwallowTransform.position);
            //Debug.Log("distance: "+distance);
            if (distance > 0.01f)
            {
                MoveFish(SwallowTransform.position);
            }
            else
            {
                Debug.Log("fishSwallown");
                fishSwallown = true;
                script.sharkAte = true;
//                script.GoNextStage();
            }
        }
    }

    void MoveFish(Vector2 destination)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
            destination, step);
    }
}
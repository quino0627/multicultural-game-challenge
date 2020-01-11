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

    private GameObject speaker;
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
        FishClicked();
    }

    public void FishClicked()
    {
      
        
        //현재 클릭된 물고기 글자
        string currFishText = GetComponentInChildren<TextMeshPro>().text;
        
        //해당 stage의 정답 
        string currStageAnsText = script.choiceTexts[script.ansPosIndex[0]].text;
            if (currFishText == currStageAnsText )
            {
                //transform.SetParent(Shark.transform);
                //transform.localPosition= Vector2.zero;
                Debug.Log("Fish should move...");
                //상어입
                transform.position = SharkMouthTransform.position;
                
                //setactive(false) speaker
                speaker.SetActive(false);
                
                //상어 먹기시작
                StartCoroutine(SharkEating());
            }
        
    }
    
    IEnumerator SharkEating()
    {
        Shark.GetComponent<Animator>().SetTrigger("Eat");
        while (!fishSwallown)
        {
            yield return new WaitForEndOfFrame();
            
           
            float distance = Vector2.Distance(
                transform.position, 
                SwallowTransform.position);
            
            if (distance > 0.01f)
            {
                MoveJellyfish(SwallowTransform.position);
            }
            else
            {
                fishSwallown = true;
                script.GoNextStage();
            }
            
        }
    }

    void MoveJellyfish( Vector2 destination)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
            destination, step);
    }

}

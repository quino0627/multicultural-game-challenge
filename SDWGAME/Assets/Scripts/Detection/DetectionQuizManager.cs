using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using System.Diagnostics;
using TMPro;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class DetectionQuizManager : MonoBehaviour
{
   
    
    //excel data
    public Entity_LEVEL1 list;
    
    //쉬움, 보통, 어려움 난이도
    public int level = 0;
    //각 난이도 안에는 stage 0 부터 max_stage_no - 1까지의 stage가 존재한다.
    public int stage_no = 0;
    //엑셀 데이터 개수 가져와서 저장할 것.
    public int max_stage_no;
    //Barrel1, Barrel2, Barrel3, Barrel4, Barrel5 -> 텍스트를 담고 있는 Object
    public GameObject[] Barrels;
    //문어새기
    public GameObject Octo;
    public Text quiz_level;
    public Text quiz_step;
//시간 테스트를 위한 임시 변수
    public Text timeText;
// 타이머 관련 변수
    private float timer = 0f;
    private float timeLimit = 60f;
    public Stopwatch watch = new Stopwatch();
    
    //stage 1 ~ 5까지의 정답 리스트를 저장 -> stage_no는 0~4부터가 된다.
    // 정답은 Barrel 1~5중 랜덤으로 위치해야 함
    [HideInInspector] private int[] answer_list;

    // 매 스테이지에서 Barrel 1~5에 들어갈 텍스트가 여기 들어감.
    // 즉 매 스테이지마다 초기화되어야함.
    public TextMeshPro[] QuizTextList;
    
    // 매 Stage가 time이 넘어가게 되면 TimeOver 함수를 호출하는 과정에 필요한 변
    [HideInInspector] private bool run_once = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //엑셀에서 파싱해서 들고 와야 함 
        max_stage_no = 3;
        answer_list = new int[max_stage_no];
        QuizInit();
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = watch.ElapsedMilliseconds.ToString();
        if (!run_once && watch.ElapsedMilliseconds > 5000)
        {
            Debug.Log("Stage Over");
            run_once = true;
            this.TimeOver();
        }

    }

    public void QuizInit()
    {
        
        int i = 0;
        //stage 1 ~ 5까지의 정답을 정해서 answer_list에 저장한다.
        while (i < max_stage_no)
        {
            answer_list[i] = UnityEngine.Random.Range(0, 5);
            
            i++;
        }
        Debug.Log($"Stage 0~{max_stage_no-1}까지의 정답은");
        for (int j = 0; j < max_stage_no; j++)
        {
            Debug.Log(answer_list[j].ToString());
        }
        
        // 단계(easy=0, normal=1, hard=2)를 받아서 그에 따른 stage를 띄워주면 될 듯
        StartCoroutine(StageEach(level));
    }

    // IEnumerator을 사용하는 이유는 타이머 때문인 듯?!
    // Original Code at RandomQuizScript/EnableCoroutine
    IEnumerator StageEach(int level)
    {
        
        //
        //시작 오디오
        run_once = false;
        //QuizTextList 초기화
//        QuizTextList = new Text[5];
        // 퀴즈 배열
        //정답을 랜덤위치에 넣고
        Debug.Log($"answerlist[stage_no]는 {answer_list[stage_no]}");
        Debug.Log(QuizTextList);
        Debug.Log(list.sheets[level].list[stage_no].cor);
        QuizTextList[answer_list[stage_no]].text = list.sheets[level].list[stage_no].cor;
        Debug.Log(QuizTextList[answer_list[stage_no]].text);
     
        
        // 퀴즈 시작 && 타이머 시작
        {
            //퀴즈 띄우기
//            StartCoroutine(ShowChoices());
            //타이머 줄이기 호출(Director Object 접근)
//            GameObject director = GameObject.Find("GameDirector");
            int i = 0;
            while (i < 5)
            {
                yield return new WaitForSeconds(0.2f);
                //하나씩 SetActive
                Barrels[i].SetActive(true);
                i += 1;
            }
            Debug.Log("타이머 스타트");
            //클릭 타임이 될 것이야..
            watch.Start();
//            director.GetComponent<GameDirector>().DecreaseTimer(1f);
        }  
        
    }

    public void TimeOver()
    {
        Debug.Log("Time Over");
        watch.Stop();
        //기록 남기기
        Debug.Log($"{watch.ElapsedMilliseconds}ms 이후 종 ");
        watch.Reset();
        StartCoroutine(HideAnswers());

    }

    IEnumerator HideAnswers()
    {
        int i = 0;
        while (i < 5)
        {
            yield return new WaitForSeconds(0.1f);
            Barrels[i].SetActive(false);
            i = i + 1;
        }
        this.GoNextStage();
        
    }
    
    // stage가 모두 끝났다면 결과 창으로, 그렇지 않다면 다음 스테이지로 넘어간
    public void GoNextStage()
    {
        Debug.Log($"stage_no는 {stage_no}이고, max_stage_no는 {max_stage_no}");
        if (stage_no < max_stage_no-1)
        {
            stage_no++;
            StartCoroutine(StageEach(level));
        }
        else
        {
            Debug.Log("결과창 setActive");
        }
        
    }
}

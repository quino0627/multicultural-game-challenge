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
   
    // director
    private GameObject director;

    [HideInInspector] private Animator[] animators = new Animator[5];
    // excel data
    public Entity_Detection list;
    
    // 쉬움, 보통, 어려움 난이도
    public int level = 0;
    // 각 난이도 안에는 stage 0 부터 max_stage_no - 1까지의 stage가 존재한다.
    public static int stage_no = 0;
    //엑셀 데이터 개수 가져와서 저장할 것.
    public static int max_stage_no;
    //Barrel1, Barrel2, Barrel3, Barrel4, Barrel5 -> 텍스트를 담고 있는 Object
    [HideInInspector] public GameObject[] Barrels = new GameObject[5];
    //문어새기
    public GameObject Octo;
    // 타이머 관련 변수
    private float timer = 0f;
    private float timeLimit = 60f;
    public Stopwatch watch = new Stopwatch();
    
    //stage 1 ~ 5까지의 정답 리스트를 저장 -> stage_no는 0~4부터가 된다.
    // 정답은 Barrel 1~5중 랜덤으로 위치해야 함
    [HideInInspector] public static int[] answer_list;
    // answer_string_list는 각 스테이지별 정답
    [HideInInspector] public static string[] answer_string_list;
    
    // 매 스테이지에서 Barrel 1~5에 들어갈 텍스트가 여기 들어감.
    // 즉 매 스테이지마다 초기화되어야함.
    public TextMeshPro[] QuizTextList = new TextMeshPro[5];
    
    // 매 Stage가 time이 넘어가게 되면 TimeOver 함수를 호출하는 과정에 필요한 변
    [HideInInspector] private bool run_once = false;
    
    // Start is called before the first frame update
    void Start()
    {
        this.director = GameObject.Find("GameDirector");

        this.Barrels[0] = transform.Find("Barrel1").gameObject;
        this.Barrels[1] = transform.Find("Barrel2").gameObject;
        this.Barrels[2] = transform.Find("Barrel3").gameObject;
        this.Barrels[3] = transform.Find("Barrel4").gameObject;
        this.Barrels[4] = transform.Find("Barrel5").gameObject;

        this.QuizTextList[0] = this.Barrels[0].transform.Find("Word").GetComponent<TextMeshPro>();
        this.QuizTextList[1] = this.Barrels[1].transform.Find("Word").GetComponent<TextMeshPro>();
        this.QuizTextList[2] = this.Barrels[2].transform.Find("Word").GetComponent<TextMeshPro>();
        this.QuizTextList[3] = this.Barrels[3].transform.Find("Word").GetComponent<TextMeshPro>();
        this.QuizTextList[4] = this.Barrels[4].transform.Find("Word").GetComponent<TextMeshPro>();

        this.animators[0] = this.Barrels[0].GetComponent<Animator>();
        this.animators[1] = this.Barrels[1].GetComponent<Animator>();
        this.animators[2] = this.Barrels[2].GetComponent<Animator>();
        this.animators[3] = this.Barrels[3].GetComponent<Animator>();
        this.animators[4] = this.Barrels[4].GetComponent<Animator>();
        
        max_stage_no = 3;
        answer_list = new int[max_stage_no];
        answer_string_list = new string[max_stage_no];
        QuizInit();
    }

    // Update is called once per frame
    void Update()
    {
//        timeText.text = watch.ElapsedMilliseconds.ToString();
        if (!run_once && watch.ElapsedMilliseconds > 10000)
        {
            Debug.Log("Stage Over");
            run_once = true;
            this.StageOver();
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
//        Debug.Log($"Stage 0~{max_stage_no-1}까지의 정답은");
//        for (int j = 0; j < max_stage_no; j++)
//        {
//            Debug.Log(answer_list[j].ToString());
//        }
        
        // 단계(easy=0, normal=1, hard=2)를 받아서 그에 따른 stage를 띄워주면 될 듯
        StartCoroutine(StageEach(level));
    }

    // IEnumerator을 사용하는 이유는 타이머 때문인 듯?!
    // Original Code at RandomQuizScript/EnableCoroutine
    IEnumerator StageEach(int level)
    {
        // 시작 오디오 세팅
        
        run_once = false;
        
        // UI 설정
        this.director.GetComponent<GameDirector>().setStage(stage_no);
        this.director.GetComponent<GameDirector>().setLevel(level);
        
        // 퀴즈 배열
        //정답을 랜덤위치에 넣고
        Debug.Log(list.sheets[level].list[stage_no].cor);
        answer_string_list[stage_no] = list.sheets[level].list[stage_no].cor;
        QuizTextList[answer_list[stage_no]].text = list.sheets[level].list[stage_no].cor;

        
        //보기들을 나머지 위치에 넣음
        //
        int tmp = 1;
        for (int i = 0; i < 5; i++)
        {
            // i 가 만약 정답이 있는 index가 아니라면 해당 index의 text에 값을 넣어야 함.
            if (i != answer_list[stage_no])
            {
                switch (tmp)
                {
                    case 1:
                        QuizTextList[i].text = list.sheets[level].list[stage_no].ex1;
                        break;
                    case 2:
                        QuizTextList[i].text = list.sheets[level].list[stage_no].ex2;
                        break;
                    case 3:
                        QuizTextList[i].text = list.sheets[level].list[stage_no].ex3;
                        break;
                    case 4:
                        QuizTextList[i].text = list.sheets[level].list[stage_no].ex4;
                        break;
                    default:
                        Debug.Log("?");
                        break;
                    
                }
                tmp++;
            }

            
        }
            
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
                animators[i].Play("Entry");
                i += 1;
            }

            string wordFileLink = $"Sounds/Detection/{list.sheets[level].list[stage_no].filename}";
            Debug.Log(wordFileLink);
            Octo.GetComponent<AudioSource>().loop = false;
            Octo.GetComponent<AudioSource>().clip = Resources.Load(wordFileLink) as AudioClip;
            Debug.Log(Resources.Load(wordFileLink) as AudioClip);
            Octo.GetComponent<AudioSource>().Play();
            Debug.Log("타이머 스타트");
            //클릭 타임이
            watch.Start();
//            director.GetComponent<GameDirector>().DecreaseTimer(1f);
        }  
        
    }

    public void StageOver()
    {
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
            animators[i].Play("BarrelDestroy");
//            yield return new WaitForSeconds(2f);
            Debug.Log(GetAnimationClip(animators[i], "BarrelDestroy").length);
            yield return new WaitForSeconds(GetAnimationClip(animators[i], "BarrelDestroy").length);
            Barrels[i].SetActive(false);
            i = i + 1;
        }
        
        this.GoNextStage();
        
    }
    
    // stage가 모두 끝났다면 결과 창으로, 그렇지 않다면 다음 스테이지로 넘어간
    public void GoNextStage()
    {
        Debug.Log($"stage_no는 {stage_no}이고, max_stage_no는 {max_stage_no}");
        if (stage_no < max_stage_no - 1)
        {
            stage_no++;
            StartCoroutine(StageEach(level));
        }
        else
        {
            Debug.Log("결과창 setActive");
        }
        
    }
    
    public AnimationClip GetAnimationClip(Animator animator, string name) {
        if (!animator) return null; // no animator
 
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
            if (clip.name == name) {
                return clip;
            }
        }
        return null; // no clip by that name
    }
}

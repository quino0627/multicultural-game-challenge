using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class SpreadChoices : MonoBehaviour
{
    //KeepTrackController ConclusionData
    public GameObject totalStorageObject;
    private KeepTrackController totalStorageScript;
    // director
    private GameObject director;
    // Show Result
    public GameObject StarLeft;
    public GameObject StarMiddle;
    public GameObject StarRight;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI onesentenceText;
    
    // result handler
    public ResultHandler _resultHandler;
    // 한 게임에서 전체 시도한 횟수와 전체 맞춘 갯수
    // Result 페이지에서 이에 따라 보물상자 여는 것을 달리 해야 함.
    public static int total_tried = 0;
    public static int total_correct = 0;
    public static int total_correct_stage = 0;
    public int ref_total_tried;
    
    public bool allJFArrived;
    private bool[] JfArrived = new bool[8];
    public int cntJFArrived;
    private float speed = 8f;

    private bool initialDone;
    
    private bool waitUser;

    public bool isUserRight;
    //excel data
    public Entity_Synthesis data;
    
    // 초급/중급/고급
    public int level;
    
    // 각 난이도 안의 stage index
    public static int stageIndex;
    // 해당 난이도의 전체 stage 개수 
    public static int stageMaxIndex;

    public int refStageIndex;
    
    //보기 이 게임에선 해파리
    public GameObject[] choices;
    public Transform[] choiceTransforms;
    
    //Crab
    public GameObject crab;
    
    // ui level과 stage표시 하기 위한 변수
    public Text quizLevel;
    public Text quizStage;
    
    //each stage jellyfish's texts
    public TextMeshPro[] choiceTexts;
    
    //correct jellyfish's position Index
    public List<int> corrAnsPosIndex;
    
    //Number of Correct Answers
    public int corrAnsCnt;
    
    //Number of Wrong Answers
    public int wrongAnsCnt;

    public List<string> chosenAns;

    public GameObject[] PickedAnswer;
    
    public Transform[] PickedJfPos;
    //시간 테스트를 위한 임시 변수
    public Text timeText;

    // 타이머 관련 변수
    private float timer = 0f;
    private float timeLimit = 60f;
    public Stopwatch watch = new Stopwatch();
    
    //해파리 도망가는 지점
    public Transform FinishPosition;

    public Transform crabStartTransform;
    
    // 매 스테이지에서 time이 넘어가게 되면 TimeOver 함수를 호출하는 과정에 필요한 변수 
    [HideInInspector] private bool run_once = false;
    // 일시 정지에 사용되는 불린 
    private bool CheckPaused = false;
    
    // Start is called before the first frame update
    void Start()
    {
        totalStorageObject = GameObject.Find("TotalStorage");
        totalStorageScript = totalStorageObject.GetComponent<KeepTrackController>();

        this.director = GameObject.Find("SynthesisGameDirector");
//        Debug.Log("Level, StageIndex = ("+level+", "+stageIndex+")");
        stageMaxIndex = 3;
        refStageIndex = stageIndex;
        crab.transform.Find("DescriptionBubble").gameObject.SetActive(false);
        level = totalStorageScript.chosenLevel;
        stageIndex = totalStorageScript.tmpStage[1]; 
        QuizInit();
        
    }

    void Update()
    {
        ref_total_tried = total_tried;
        
        // timeScale이 1이 아니고 CheckPaused가 false이면 timer를 stop
       if(Time.timeScale != 1 && !CheckPaused)
       {
           watch.Stop();
           CheckPaused = true;
       }
       // timeScale이 1이고 CheckPaused가 true이면 timer를 restart
       if (Time.timeScale == 1 && CheckPaused)
       {
           watch.Start();
           CheckPaused = false;
       }
       
       if (watch.ElapsedMilliseconds > 0)
       {
           director.GetComponent<SynthesisGameDirector>().SetTime(watch.ElapsedMilliseconds/1000.0f);
       }
       // 타임오버 되었을 떄
       if (!run_once && watch.ElapsedMilliseconds > 60000f)
       {
//           Debug.Log("Stage Over");
           run_once = true;
//           this.StageOver();
       }

       

    }

    public void QuizInit()
    {
        crab.transform.position = crabStartTransform.position;
        if (level == 0)
        {
            crab.transform.localScale=new Vector3(0.2f ,0.2f,1);
        }

        if (level == 1)
        {
            crab.transform.localScale = new Vector3(0.25f,0.25f, 1);
        }
        if (level == 2)
        {
            crab.transform.localScale=new Vector3(0.3f,0.3f,1);
        }

        if (level == 3)
        {
            crab.transform.localScale=new Vector3(0.4f,0.4f,1);
        }
        //StartCoroutine(EnableCoroutine());
        StartCoroutine(EnableCoroutine());
    }

    IEnumerator EnableCoroutine()
    {
        //quizLevel.text = "" + (level + 1);
        //quizStage.text = "" + (stageIndex + 1);
        //Debug.Log("level "+level);
        //Debug.Log("stageIndex "+stageIndex);
        //yield return new WaitForSecondsRealtime(GetComponent<AudioSource>().clip.length);
        //yield return new WaitForSeconds(1.0f);
        //CheckToggleGroup.SetAllTogglesOff();
        
        run_once = false;
        // 감독의 시간을 60초로 초기화한다.
        director.GetComponent<SynthesisGameDirector>().InitTime();;
        // UI 설정
        director.GetComponent<SynthesisGameDirector>().setStage(stageIndex);
        director.GetComponent<SynthesisGameDirector>().setLevel(level);
        
        //정답의 index (corrAnsCnt: 초급-1,2개 중급-3개 고급-4개)
        //8개의 자리중 랜덤한 위치
        for (int i = 0; i < (corrAnsCnt+wrongAnsCnt); i++)
        {
            int tmp; 
            do {
                tmp = Random.Range(0, 8);
            } while (corrAnsPosIndex.Contains(tmp));
            //Debug.Log("corrAnsPosIndex[" + i + "] = " + tmp );
            corrAnsPosIndex.Add(tmp);
        }

        
        //정답 글자를 랜덤 위치에 넣는다.
        for (int i = 0; i < corrAnsCnt; i++)
        {
            choices[corrAnsPosIndex[i]].tag = "CorrectAns";
            if (i == 0)
            {
                //Debug.Log("level "+level);
                //Debug.Log("stageIndex "+stageIndex);
                //Debug.Log(corrAnsPosIndex[0]);
                //Debug.Log(data.sheets[level].list[stageIndex].정답1);
                PickedAnswer[0].GetComponent<TextMeshPro>().text 
                    = choiceTexts[corrAnsPosIndex[0]].text 
                    = data.sheets[level].list[stageIndex].정답1;
                
                //Debug.Log(choiceTexts[corrAnsPosIndex[0]].text);
            }

            if (i == 1)
            {
                PickedAnswer[1].GetComponent<TextMeshPro>().text 
                    = choiceTexts[corrAnsPosIndex[1]].text
                    = data.sheets[level].list[stageIndex].정답2;
            }
            if (i == 2)
            {
                PickedAnswer[2].GetComponent<TextMeshPro>().text 
                    = choiceTexts[corrAnsPosIndex[2]].text
                    = data.sheets[level].list[stageIndex].정답3;
            }
            if (i == 3)
            {
                PickedAnswer[3].GetComponent<TextMeshPro>().text 
                    = choiceTexts[corrAnsPosIndex[3]].text
                    = data.sheets[level].list[stageIndex].정답4;
            }
        }

        //정답이 아닌 글자들 처리
        for (int i = corrAnsCnt; i < (corrAnsCnt + wrongAnsCnt); i++)
        {
            choices[corrAnsPosIndex[i]].tag = "WrongAns";
            if (i == corrAnsCnt)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[level].list[stageIndex].보기1;
            }
            if (i == corrAnsCnt+1)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[level].list[stageIndex].보기2;
            }
            if (i == corrAnsCnt+2)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[level].list[stageIndex].보기3;
            }
            if (i == corrAnsCnt+3)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[level].list[stageIndex].보기4;
            }
        }

        //show quiz
        yield return StartCoroutine(ShowAnswers());
        
        Debug.Log("Asdf");
        //waitUser = true;
    }

    IEnumerator ShowAnswers()
    {
        
        yield return new WaitForSeconds(2.0f);
        crab.transform.Find("DescriptionBubble").gameObject.SetActive(true);
        SoundManager.Instance.Play_SpeechBubblePop();
        yield return new WaitForSeconds(2.0f);

        if (SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.StopMusic();
        }
        yield return new WaitForSeconds(1.5f);
        string wordFileLink = $"Sounds/Synthesis/{data.sheets[level].list[stageIndex].filename}";
        Debug.Log(data.sheets[level].list[stageIndex].filename);
        Debug.Log(wordFileLink);
        crab.GetComponent<AudioSource>().loop = false;
        crab.GetComponent<AudioSource>().clip = Resources.Load(wordFileLink) as AudioClip;
        crab.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1.5f);
        if (!SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.Play_JellyFishShowedUp();    
        }
        
        if (crab.GetComponent<AudioSource>().clip)
        {
            yield return new WaitForSeconds(crab.GetComponent<AudioSource>().clip.length + 1f);
        }
        //Jellyfish comes out
        int cnt=0;
        int jellyfish_index;
        while (cnt < corrAnsCnt+wrongAnsCnt)
        {
            yield return new WaitForSeconds(0.0f); 
            //choices[maxChoiceNumber].SetActive(true);
            jellyfish_index = corrAnsPosIndex[cnt];
            
            //1. move to each choicePosition
            StartCoroutine(InitialJellyfish(jellyfish_index));
            
            cnt++;
        }
        
        Debug.Log("aaaaaaaaa");

        //Debug.Log("End ShowAnswers");
        // Crab Speaks - no audio available right now
        /*yield return new WaitForSeconds(1.0f);
        string p = "02.Sounds/Stimulus/" + data.sheets[level].list[stageIndex].filename;
        crab.GetComponent<AudioSource>().loop = false;
        crab.GetComponent<AudioSource>().clip = Resources.Load(p) as AudioClip;
        crab.GetComponent<AudioSource>().Play();*/
        
    }

    IEnumerator InitialJellyfish(int i)
    {
        //Debug.Log("InitialJellyfish "+i);
        // 얘는 OneShot이 아니고 백그라운드 뮤직
        if (!SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.Play_JellyFishShowedUp();    
        }
        while (!JfArrived[i])
        {
            yield return new WaitForEndOfFrame();
            //Debug.Log("InitialJellyfish while문 "+i);
            float distance = Vector2.Distance(
                choices[i].transform.position, 
                choiceTransforms[i].position);
            //Debug.Log("distance: "+distance);
            if (distance > 0.02f)
            {
                MoveJellyfish(i,choiceTransforms[i].position);
            }
            else
            {
                JfArrived[i] = true;
                Debug.Log("wjjwwjj");
                //Debug.Log("InitialJellyfish JF "+ i +" became true");
            }
            
        }
        
    }

    void MoveJellyfish(int i, Vector2 destination)
    {
        
        float step = speed * Time.deltaTime;
        Transform jellyfish = choices[i].transform;
        /*float distance = Vector2.Distance(choices[i].transform.position,
            destination);*/
        jellyfish.position = Vector2.MoveTowards(jellyfish.position,
                destination, step);
        
    }
    
    
    // Update is called once per frame
     void FixedUpdate()
     { 
         //Debug.Log("initialDone: "+initialDone);
         if (!initialDone && CheckAllArrived())
         {
             Debug.Log("DONE");
             // 해파리가 모두 제 자리에 왔을 때 시간을 시작.
             initialDone = true;
             Invoke("StartTime", 2.0f);
             
             //Debug.Log("In update Jfarrived Initialized");
         }
         /*Debug.Log("JFArrived : "
                   +JfArrived[0]+", "
                   +JfArrived[1]+", "
                   +JfArrived[2]+", "
                   +JfArrived[3]+", ");*/
         
         
         
     }

     private void StartTime()
     {
         watch.Start();
         for (int i = 0; i < wrongAnsCnt + corrAnsCnt; i++)
         {
             JfArrived[corrAnsPosIndex[i]] = false;
         }
     }

    public void GoNext()
    {
        Debug.Log("HERE?");
        if (stageIndex == 29)
        { 
            level++;
            stageIndex = 0;
        }
        else
        { 
            stageIndex++;
            totalStorageScript.tmpStage[1] = stageIndex;
        }
        
        StartCoroutine(HideAnswers());
    }

    IEnumerator HideAnswers()
    {
        int i = 0;
        int jfIndex;
        while (i < wrongAnsCnt + corrAnsCnt)
        {
            
            yield return null;
            jfIndex = corrAnsPosIndex[i];
            StartCoroutine(ExitJellyFish(jfIndex));
            i++;
        }
        
       
        yield return new WaitForSeconds(4f);
        //Debug.Log("Re Quiz Init");

        // stageIndex는 0부터 시작 
        // stageMaxIndex-1: 전체 stage개수
        // stage가 끝났을 경우에는 Result창을 보여주어야 한다.
        if (stageIndex >= stageMaxIndex)
        {
            Debug.Log("Game Is Over");
            totalStorageScript.tmpLevel[1]++;
            yield return DecideResult(total_tried, total_correct, total_correct_stage);
        }

        if (stageIndex < stageMaxIndex)
        {
            if (level == 0 && stageIndex == 15)
            {
                //글자수 1 -> 2
                //초급인 건 그대로
                SceneManager.LoadScene("CrabLevel2");
            }
            
            if (level == 1 && stageIndex == 15)
            {
                //중급으로 넘어가기 
                SceneManager.LoadScene("CrabLevel3");
            }
            if (level == 2 && stageIndex==30) 
            {
                //고급으로 넘어가기
                SceneManager.LoadScene("CrabLevel4");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        
        
            
        
        
    }

    IEnumerator ExitJellyFish(int i)
    {
        //Debug.Log("Enter Exit && JfArrived " + i +": " + JfArrived[i]);
        while (!JfArrived[i])
        {
            //Debug.Log("Jf" + i + " starts moving");
            float distance = Vector2.Distance(
                choices[i].transform.position, 
                FinishPosition.position
            );
            if (distance > 0.01f)
            {
                //Debug.Log(i+"th jf is moving to finish position");
                MoveJellyfish(i, FinishPosition.position);
            }
            else
            {
                JfArrived[i] = true;
                //Debug.Log("Exit JellyFish Jf " + i +" became true");
            }
            yield return null;
        }
    }

    bool CheckAllArrived()
    {
        bool flag = true;
        for (int i = 0; i < corrAnsCnt + wrongAnsCnt; i++)
        {
            //Debug.Log("CheckAllArrived JfArrived " + i + ": " + JfArrived[i]);
            
            if (JfArrived[corrAnsPosIndex[i]] == false)
            {
                flag = false;
                //Debug.Log("flag is false, break");
                break;
            }
        }
        
        //Debug.Log("return flag: "+flag);
        return flag;
    }
    
    // ttr: total tried, tco: total corrected 
    IEnumerator DecideResult(float ttr, float tco, float tcos)
    {
        yield return new WaitForSeconds(1.0f);
        _resultHandler.OpenResult();
        // tcl : total_clicked
        // tco : total_correct
        string result_text =  $"{ttr - tco}번 틀리고 {tcos}개를 맞췄어요!";
        descriptionText.text = result_text;
        string[] sentences = {"정말 잘했어요", "조금 더 신중하게 해 보자", "더 연습하자"};
        float rate = ttr / tco;
        //        // 만약에 2.5배보다 더 많이 클릭했으면
        if (rate > 2.5f)
        {
            // 아무것도 열리지 않을 것.
            // do nothing
            SoundManager.Instance.Play_NoStarShowedUp();
            StarLeft.SetActive(false);
            onesentenceText.text = sentences[2];
            
        }
        else
        {
            SoundManager.Instance.Play_StarShowedUp();
            onesentenceText.text = sentences[2];
            StarLeft.SetActive(true);
            yield return new WaitForSeconds(.5f);
        }

        if (rate > 2)
        {
            StarMiddle.SetActive(false);
        }
        else
        {
            SoundManager.Instance.Play_StarShowedUp();
            onesentenceText.text = sentences[1];
            StarMiddle.SetActive(true);
            yield return new WaitForSeconds(.5f);
        }

        if (rate > 1.5)
        {
            StarRight.SetActive(false);
        }
        else
        {
            SoundManager.Instance.Play_StarShowedUp();
            onesentenceText.text = sentences[0];
            StarRight.SetActive(true);
            yield return new WaitForSeconds(.5f);

        }

        
    }
    
    public void PlusTotalTry()
    {
        Debug.Log("totaltried");
        total_tried++;
    }

    // 얘는 한 번 드래그 할때마다 올라가고,
    public void PlusTotalCorrect()
    {
        Debug.Log("totalCorrected");
        total_correct++;
    }

    //얘는 한 스테이지를 맞출 떄마다 올라간다.
    public void PlusTotalCorrectStage()
    {
        // 시간을 멉춥니다.
        watch.Stop();
        Debug.Log("totalCorrectedStage");
        total_correct_stage++;
    }
}

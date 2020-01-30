using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class FishShowAnswer : MonoBehaviour
{
    //KeepTrackController ConclusionData
    public GameObject totalStorageObject;
    private KeepTrackController totalStorageScript;
    // director
    private GameObject director;
    public bool canClick;
    
    public GameObject StarLeft;
    public GameObject StarMiddle;
    public GameObject StarRight;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI onesentenceText;
    
    // Show Result
    public ResultHandler _resultHandler;

    // 한 게임에서 전체 클릭한 갯수와 전체 맞춘 갯수
    // Result 페이지에서 이에 따라 보물상자 여는 것을 달리 해야 함.
    public static int total_clicked = 0;
    public static int total_correct = 0;
    public int ref_total_clicked;
    
    
    //excel data
    //public Entity_EliminationTest data;
    public Entity_EliminationTestCnt10 data;
    
    // 초급/중급/고급
    public int level;
    
    // 각 난이도 안의 stage index
    public static int stageIndex;
    
    
    // 해당 난이도의 전체 stage 개수
    public static int stageMaxIndex;
    

    public int refStageIndex;
    //보기 이 게임에선 물고기
    public GameObject[] choices;
    public TextMeshPro[] choiceTexts;
    public List<int> ansPosIndex;
    
    //Shark
    public GameObject shark;
    private AudioSource sharkAudio;

    public GameObject stimulation;
    private TextMeshPro stimulText;

    public GameObject eliminStimul;
    public TextMeshPro eliminText;

    public GameObject syllable;
    public TextMeshPro syllableText;
    
    // ui level과 stage표시 하기 위한 변수
    public TextMeshPro quizLevel;
    public TextMeshPro quizStage;
    
    // 타이머 관련 변수
    private float timer = 0f;
    private float timeLimit = 60000f; //60초
    public Stopwatch watch = new Stopwatch();

    //
    private GameObject QuizManager;

    private Transform sharkArriveTransform;
    public float fastSpeed = 20f;
    public float slowSpeed = 0.006f;
    public bool isSharkArrived;
    
    public Transform fishExitPos;
    public Transform Fishes;
    public Transform FishesCenter;
    public bool isFishExited;
    public bool isTimeSetted;
    public bool isTimeOver;
    public bool sharkAte;

    public string chosenAns;
    public bool isUserRight;
    
    // Start is called before the first frame update

    private bool CheckPaused = false;
    
    void Start()
    {totalStorageObject = GameObject.Find("TotalStorage");
        totalStorageScript = totalStorageObject.GetComponent<KeepTrackController>();

        director = GameObject.Find("EliminationGameDirector");
        refStageIndex = stageIndex;
        level = totalStorageScript.chosenLevel;
        stageIndex = totalStorageScript.tmpStage[2];
        QuizManager = GameObject.Find("QuizManager");
        fishExitPos = GameObject.Find("FishExitPos").transform;
        Fishes = GameObject.Find("Fishes").transform;
        FishesCenter = GameObject.Find("FishesCenter").transform;
        sharkAudio = shark.GetComponent<AudioSource>();
        stimulText = stimulation.GetComponent<TextMeshPro>();
        sharkArriveTransform = GameObject.Find("SharkArrivePos").transform;
        // 전체 stage 개수를 3으로 한다.
        stageMaxIndex = 3;
        
        QuizInit();
    }

    // Update is called once per frame
    void Update()
    {
        // timeScale이 1이 아니고 CheckPaused가 false이면 timer를 stop
        if (Time.timeScale != 1 && !CheckPaused)
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
            director.GetComponent<EliminationDirector>()
                .SetTime(watch.ElapsedMilliseconds/1000.0f);
            //Debug.Log(watch.ElapsedMilliseconds/1000.0f);
        }

        // 타임오버 되었을 떄
        if (watch.ElapsedMilliseconds > timeLimit)
        {
            Debug.Log("Stage Over");
            isTimeOver = true;
            this.GoNextStage(100,false);
        }
        
        //이제 왼쪽으로 서서히 움직일꺼야
        if ( isTimeSetted && watch.ElapsedMilliseconds < timeLimit)
        {
            for (int i = 0; i < 5; i++)
            {
                if (choices[i].GetComponent<FishController>().isCaught == false)
                {
                    Fishes.Translate(
                        Vector2.left * slowSpeed * Time.deltaTime,
                        Space.World);
                }
                else
                {
                    //Debug.Log("jdklsfjalkjfalksjf;lkasjfal;ksdjfl");
                    isTimeSetted = false;
                    GoNextStage(i, true);
                }
                
            }
        }
    }

    public void QuizInit()
    {
        
        StartCoroutine(EnableCoroutine());
    }

    IEnumerator EnableCoroutine()
    {
        director.GetComponent<EliminationDirector>().InitTime();
        director.GetComponent<EliminationDirector>().setStage(stageIndex);
        director.GetComponent<EliminationDirector>().setLevel(level);
        
        //yield return new WaitForSecondsRealtime(GetComponent<AudioSource>().clip.length);
        yield return new WaitForSeconds(0.0f);
        

        for (int i = 0; i < 5; i++)
        {
            int tmp; 
            do {
                tmp = Random.Range(0, 5);
            } while (ansPosIndex.Contains(tmp));
            ansPosIndex.Add(tmp);
        }
        
        //정답 물고기에 정답음성 넣기
        string wordFileLink = 
            $"Sounds/Detection/{data.sheets[level].list[stageIndex].정답음성}";
        AudioSource corrFish = 
            choices[ansPosIndex[0]].GetComponentInChildren<AudioSource>();
        corrFish.loop = false;
        corrFish.clip = Resources.Load(wordFileLink) as AudioClip;
        
        //테스트로 글자 넣기
        choiceTexts[ansPosIndex[0]].text = 
            data.sheets[level].list[stageIndex].정답;
        
        //오답 물고기에 오답음성 넣기
        int j = 1;
        string link;
        AudioSource fishSpeaker;
        
        for (int i = 1; i < 5; i++)
        {
            fishSpeaker = choices[ansPosIndex[i]].GetComponentInChildren<AudioSource>();

            if (i == 1)
            {
                link = $"Sounds/Detection/{data.sheets[level].list[stageIndex].오답음성1}";
                fishSpeaker.loop = false;
                fishSpeaker.clip = Resources.Load(link) as AudioClip;
                
                choiceTexts[ansPosIndex[i]].text =
                    data.sheets[level].list[stageIndex].오답1;
            }

            else if (i == 2)
            {
                link = $"Sounds/Detection/{data.sheets[level].list[stageIndex].오답음성2}";
                fishSpeaker.loop = false;
                fishSpeaker.clip = Resources.Load(link) as AudioClip;
                
                choiceTexts[ansPosIndex[i]].text =
                    data.sheets[level].list[stageIndex].오답2;
            }

            else if (i == 3)
            {
                link = $"Sounds/Detection/{data.sheets[level].list[stageIndex].오답음성3}";
                fishSpeaker.loop = false;
                fishSpeaker.clip = Resources.Load(link) as AudioClip;
                
                choiceTexts[ansPosIndex[i]].text =
                    data.sheets[level].list[stageIndex].오답3;
            }
            else if (i == 4)
            {
                link = $"Sounds/Detection/{data.sheets[level].list[stageIndex].오답음성4}";
                fishSpeaker.loop = false;
                fishSpeaker.clip = Resources.Load(link) as AudioClip;
                
                choiceTexts[ansPosIndex[i]].text =
                    data.sheets[level].list[stageIndex].오답4;
            }
        }
        
        StartCoroutine(ShowSharkNeed());

    }

    IEnumerator ShowSharkNeed()
    {
        // 먼저 상어가 들어온다
        // 상어 소리
        SoundManager.Instance.Play_SharkShowedUp();
        while (!isSharkArrived)
        {
            yield return new WaitForEndOfFrame();
            float distance = Vector2.Distance(
                shark.transform.position,
                sharkArriveTransform.position);
            if (distance > 0.01f)
            {
                MoveShark(sharkArriveTransform.position);
                //Debug.Log("Shark moved");
            }
            else
            {
                isSharkArrived = true;
                
                //Debug.Log("Shark arrived");
            }
        }
        
        //초성 종성 표시
        syllableText.text = $"<{data.sheets[level].list[stageIndex].초성종성}>";
        
        //syllable.SetActive(true);
        
        // 자극 제시 ex) 만들
        stimulText.text = data.sheets[level].list[stageIndex].자극;
        SoundManager.Instance.Play_SpeechBubblePop();
        stimulation.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        // 탈락 자극 제시 ex) ㄴ
        stimulation.SetActive(false);
        SoundManager.Instance.Play_SpeechBubblePop();
        eliminText.text = data.sheets[level].list[stageIndex].탈락자극;
        eliminStimul.SetActive(true);
        
        Debug.Log("HEHEHEHE");
        
        StartCoroutine(ShowAnswer());
    }

    void MoveShark(Vector2 destination)
    {
        float step = fastSpeed * Time.deltaTime;
        shark.transform.position = Vector2.MoveTowards(
            shark.transform.position, destination, step);
    }
    IEnumerator ShowAnswer()
    {
        if (SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.StopMusic();
        }
        yield return new WaitForSeconds(1f);
        int i = 0;
        //int fish_index;
        while (i < 5)
        {
            yield return new WaitForSeconds(1.0f);
            choices[i].SetActive(true);
            
            //물고기들이 가운데 생김
            
            i++;
        }
        yield return new WaitForSeconds(2.0f);
        
        SoundManager.Instance.Play_EliminationMusic();
       
        //Debug.Log("watch start");
        watch.Start();
//        sliderTimer.GetComponent<SliderTimer>().shouldStart = true;
        canClick = true;
        isTimeSetted = true;
        for (int j = 0; j < 5; j++)
        {
            choices[j].GetComponent<Animator>().SetTrigger("Swim");
        }
    }
    
    
    IEnumerator HideAnswers(int poorFishIndex, bool isCaught)
    {
        yield return new WaitForSeconds(0.1f); 
        //choices[i].SetActive(false);
        Debug.Log("Inside HideAnswers");
        while (!isFishExited)
        {
            yield return new WaitForEndOfFrame();
            float distance = Vector2.Distance(
                FishesCenter.position,
                fishExitPos.position);
            if (distance > 0.01f)
            {
                if (isCaught)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (j != poorFishIndex)
                        {
                            //Debug.Log("surrvied fish: "+j);
                            MoveSurrvivedFish(j,
                                fishExitPos.position, fastSpeed);
                        }
                    }
                    MoveFishesCenter(fishExitPos.position, fastSpeed);
                }
                else
                {
                    MoveFishes(fishExitPos.position, fastSpeed);
                    MoveFishesCenter(fishExitPos.position, fastSpeed);
                }

            }
            else
            {
                isFishExited= true;
            }
        }
        
        Debug.Log($"stageIndex:{stageIndex}");
        Debug.Log($"stageMaxIndex:{stageMaxIndex}");
        // stageIndex는 0 부터 시작
        // stageMaxIndex-1 : 전체 stage개수
        // stage가 끝났을 경우에. Result창을 보여줌
        if (stageIndex >= stageMaxIndex)
        {
            Debug.Log("Game Is Over");
            shark.SetActive(false);
            Fishes.gameObject.SetActive(false);
            totalStorageScript.tmpLevel[2]++;
            yield return new WaitForSeconds(1f);
            yield return DecideResult(total_clicked, total_correct);
        }
        
        yield return new WaitForSeconds(1.0f);
        
        // stage가 아직 남았을 경우에
        if (stageIndex < stageMaxIndex)
        {
            Debug.Log("StageIndex<StageMaxIndex");
            if (sharkAte||isTimeOver)
            {
                Debug.Log("LoadNextScene");
                
                yield return new WaitForSeconds(2.0f);
                LoadNextScene();
            }
        }
        
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MoveFishes( Vector2 destination,float speed)
    {
        //Debug.Log("Fishes exiting");
        float step = speed * Time.deltaTime;
      
             Fishes.position 
                = Vector2.MoveTowards(Fishes.position,
            destination, step);

    }

    void MoveFishesCenter(Vector2 destination, float speed)
    {
        //Debug.Log("Fishes exiting");
        float step = speed * Time.deltaTime;
        FishesCenter.position 
            = Vector2.MoveTowards(FishesCenter.position,
                destination, step);
    }
    void MoveSurrvivedFish( int i, Vector2 destination,float speed)
    {
        //Debug.Log("Fishes Escaped");
        float step = speed * Time.deltaTime;
        choices[i].transform.position = 
            Vector2.MoveTowards(choices[i].transform.position,
            destination, step);
        
    }

    public void GoNextStage(int a, bool b)
    {
        watch.Stop();
        isTimeSetted = false;
        stageIndex++;
        totalStorageScript.tmpStage[2] = stageIndex;
        watch.Reset();
        Debug.Log("In GoNextSTage Invoke HideAnswer");
        StartCoroutine(HideAnswers(a,b));
    }
    
    
    IEnumerator DecideResult(float tcl, float tco)
    {
        yield return new WaitForSeconds(1.0f);
        _resultHandler.OpenResult();
//        // Text 설정
//        // tcl : total_clicked
//        // tco : total_correct
        string result_text = $"{tcl}번만에 {tco}개를 맞췄어요!";
        descriptionText.text = result_text;
        string[] sentences = {"정말 잘했어요", "조금 더 신중하게 해 보자", "더 연습하자"};
        float rate = tcl / tco;
//        // 만약에 2.5배보다 더 많이 클릭했으면
        if (rate > 2.5f)
        {
            // 아무것도 열리지 않을 것.
            // do nothing
            // 별 아무것도 못 받았을 떄 소리
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

    // Total Click과 Total Correct를 증가시키기 위한 함수들
    public void PlusTotalClick()
    {
        Debug.Log("totalClicked");
        total_clicked++;
        ref_total_clicked = total_clicked;
    }

    public void PlusTotalCorrect()
    {
        Debug.Log("totalCorrected");
        total_correct++;
    }
    
    
}

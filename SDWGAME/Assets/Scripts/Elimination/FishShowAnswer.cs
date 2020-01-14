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
    
    //excel data
    public Entity_EliminationTest data;
    
    // 초급/중급/고급
    public int level;
    
    // 각 난이도 안의 stage index
    public static int stageIndex;

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
    
    // ui level과 stage표시 하기 위한 변수
    public TextMeshPro quizLevel;
    public TextMeshPro quizStage;
    
    // 타이머 관련 변수
    private float timer = 0f;
    private float timeLimit = 10000f; //10초
    public Stopwatch watch = new Stopwatch();
    public Slider sliderTimer;
    
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
    // Start is called before the first frame update
    void Start()
    {
        refStageIndex = stageIndex;
        QuizManager = GameObject.Find("QuizManager");
        fishExitPos = GameObject.Find("FishExitPos").transform;
        Fishes = GameObject.Find("Fishes").transform;
        FishesCenter = GameObject.Find("FishesCenter").transform;
        sharkAudio = shark.GetComponent<AudioSource>();
        stimulText = stimulation.GetComponent<TextMeshPro>();
        sharkArriveTransform = GameObject.Find("SharkArrivePos").transform;
        QuizInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (watch.ElapsedMilliseconds > 0)
        {
            QuizManager.GetComponent<EliminationDirector>()
                .SetTime(watch.ElapsedMilliseconds/1000.0f);
            //Debug.Log(watch.ElapsedMilliseconds/1000.0f);
        }

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
        QuizManager.GetComponent<EliminationDirector>().setStage(stageIndex);
        QuizManager.GetComponent<EliminationDirector>().setLevel(level);
        
        //yield return new WaitForSecondsRealtime(GetComponent<AudioSource>().clip.length);
        yield return new WaitForSeconds(0.0f);
        //CheckToggleGroup.SetAllTogglesOff();

        for (int i = 0; i < 5; i++)
        {
            int tmp; 
            do {
                tmp = Random.Range(0, 5);
            } while (ansPosIndex.Contains(tmp));
            //Debug.Log("AnsPosIndex[" + i + "] = " + tmp );
            ansPosIndex.Add(tmp);
        }
        //Debug.Log("correctAnsPosIndex: " + ansPosIndex[0]);
        
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
        //먼저 상어가 들어온다
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
        
        // 자극 제시 ex) 만들
        stimulText.text = data.sheets[level].list[stageIndex].자극;
        stimulation.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        
        // 탈락 자극 제시 ex) ㄴ
        stimulation.SetActive(false);
        eliminText.text = data.sheets[level].list[stageIndex].탈락자극;
        eliminStimul.SetActive(true);

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
        int i = 0;
        //int fish_index;
        while (i < 5)
        {
            yield return new WaitForSeconds(1.0f);
            choices[i].SetActive(true);
            
            //물고기들이 가운데 생김
            
            i++;
        }
       
        //Debug.Log("watch start");
        watch.Start();
        sliderTimer.GetComponent<SliderTimer>().shouldStart = true;

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
        
        
        
     
        if (sharkAte||isTimeOver)
        {
            Debug.Log("LoadNextScene");
            yield return new WaitForSeconds(2.0f);
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MoveFishes( Vector2 destination,float speed)
    {
        Debug.Log("Fishes exiting");
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
        watch.Reset();
        StartCoroutine(HideAnswers(a,b));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using System.Diagnostics;
using Newtonsoft.Json;
using TMPro;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class DetectionQuizManager : MonoBehaviour
{
    //KeepTrackController ConclusionData
    public GameObject totalStorageObject;
    private TotalDataManager _totalStorageScript;

    private GameObject eachQuestionStorage;
    private EachQuestionDataManager eachQuestionStorageScript;

    private GameObject stageStorage;
    private StageDataManager stageStorageScript;

    private GameObject levelStorage;
    private LevelDataManager levelStorageScript;

    // director
    private GameObject director;
    private GameObject description;

    public GameObject StarLeft;
    public Image StarMiddle;
    public GameObject StarRight;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI onesentenceText;

    // result handler
    public ResultHandler _resultHandler;

    [HideInInspector] private Animator[] animators = new Animator[5];

    // excel data
    public Entity_Detection list;

    // 쉬움, 보통, 어려움 난이도
    public int level = 0;

    // 각 난이도 안의 step
    public int stage; //0,1,2

    //문제 엑셀 ID
    public int questionId;
    private static List<int> randomNoDuplicates;

    // 각 stage 안에는 question 0 부터 max_stage_no - 1까지의 질문이 존재한다.
    public static int question_no = 0;

    //복사본 wj
    public int refQuestionNumber;


    //엑셀 데이터 개수 가져와서 저장할 것.
    public static int max_question_no;

    // 한 레벨이 끝날 때 까지 
    public int total_clicked = 0;
    public int total_correct = 0;

    // Barrel1, Barrel2, Barrel3, Barrel4, Barrel5 -> 텍스트를 담고 있는 Object
    [HideInInspector] public GameObject[] Barrels = new GameObject[5];

    // Barrel 안 Coin들
    [HideInInspector] public GameObject[] CoinInBarrel = new GameObject[5];
    [HideInInspector] public GameObject[] TrashInBarrel = new GameObject[5];


    //문어새기
    public GameObject Octo;

    // 타이머 관련 변수
    private float timer = 0f;
    private float timeLimit = 60f;
    public Stopwatch watch = new Stopwatch();
    public float responseTime; //wj

    //stage 1 ~ 5까지의 정답 리스트를 저장 -> stage_no는 0~4부터가 된다.
    // 정답은 Barrel 1~5중 랜덤으로 위치해야 함
    [HideInInspector] public static int[] answer_list;

    // answer_string_list는 각 스테이지별 정답
    [HideInInspector] public static string[] answer_string_list;

    //wj corrAns
    public string ref_answer_string;

    //wj chosenAns
    public List<string> chosenAns;
    public bool isUserRight;

    // 매 스테이지에서 Barrel 1~5에 들어갈 텍스트가 여기 들어감.
    // 즉 매 스테이지마다 초기화되어야함.
    public TextMeshPro[] QuizTextList = new TextMeshPro[5];

    // 매 Stage가 time이 넘어가게 되면 TimeOver 함수를 호출하는 과정에 필요한 변
    [HideInInspector] private bool run_once = false;

    private bool CheckPaused = false;

    // 로딩중인지?
    private bool isLoading = true;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.StopMusic();

        totalStorageObject = GameObject.Find("TotalStorage");
        _totalStorageScript = totalStorageObject.GetComponent<TotalDataManager>();
        eachQuestionStorage = GameObject.Find("EachQuestionStorage");
        eachQuestionStorageScript = eachQuestionStorage.GetComponent<EachQuestionDataManager>();
        stageStorage = GameObject.Find("StageStorage");
        stageStorageScript = stageStorage.GetComponent<StageDataManager>();
        levelStorage = GameObject.Find("LevelStorage");
        levelStorageScript = levelStorage.GetComponent<LevelDataManager>();

        question_no = 0;
        level = _totalStorageScript.chosenLevel;
        stage = _totalStorageScript.chosenStage;
        chosenAns = new List<string>();

        max_question_no = 3;
        this.director = GameObject.Find("GameDirector");
        this.description = GameObject.Find("Octopus").transform.Find("DescriptionBubble").gameObject;

        this.Barrels[0] = transform.Find("Barrel1").gameObject;
        this.Barrels[1] = transform.Find("Barrel2").gameObject;
        this.Barrels[2] = transform.Find("Barrel3").gameObject;
        this.Barrels[3] = transform.Find("Barrel4").gameObject;
        this.Barrels[4] = transform.Find("Barrel5").gameObject;

        this.CoinInBarrel[0] = Barrels[0].transform.Find("Coin").gameObject;
        this.CoinInBarrel[1] = Barrels[1].transform.Find("Coin").gameObject;
        this.CoinInBarrel[2] = Barrels[2].transform.Find("Coin").gameObject;
        this.CoinInBarrel[3] = Barrels[3].transform.Find("Coin").gameObject;
        this.CoinInBarrel[4] = Barrels[4].transform.Find("Coin").gameObject;

        this.TrashInBarrel[0] = Barrels[0].transform.Find("Trash").gameObject;
        this.TrashInBarrel[1] = Barrels[1].transform.Find("Trash").gameObject;
        this.TrashInBarrel[2] = Barrels[2].transform.Find("Trash").gameObject;
        this.TrashInBarrel[3] = Barrels[3].transform.Find("Trash").gameObject;
        this.TrashInBarrel[4] = Barrels[4].transform.Find("Trash").gameObject;

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

        this.Octo = transform.Find("Octopus").gameObject;

        max_question_no = 3;
        answer_list = new int[max_question_no];
        answer_string_list = new string[max_question_no];


        if (question_no == 0)
        {
            Debug.Log("make randomeNoDuplicates");
            randomNoDuplicates = new List<int>();
            for (int i = 0; i < max_question_no; ++i)
            {
                int tmp = UnityEngine.Random.Range(0, max_question_no);
                while (randomNoDuplicates.Contains(tmp))
                {
                    tmp = UnityEngine.Random.Range(0, max_question_no);
                }

                randomNoDuplicates.Add(tmp);
            }
        }

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
            if (!isLoading)
            {
                Debug.Log("????????????");
                watch.Start();
                CheckPaused = false;
            }
        }

        if (watch.ElapsedMilliseconds > 0)
        {
            director.GetComponent<GameDirector>().SetTime(watch.ElapsedMilliseconds / 1000.0f);
        }

//        timeText.text = watch.ElapsedMilliseconds.ToString();
        if (!run_once && watch.ElapsedMilliseconds > 60000f)
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
        while (i < max_question_no)
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
        total_clicked = 0;
        questionId = stage * 10 + randomNoDuplicates[question_no];
        //questionId = randomNoDuplicates[question_no];
        Debug.Log("questionID: " + questionId);
        // 시작 오디오 세팅
        run_once = false;

        // UI 설정
        this.director.GetComponent<GameDirector>().setStage(question_no);
        this.director.GetComponent<GameDirector>().setLevel(level);
        description.GetComponent<DetectionDescriptionController>().DefaultDescription();

        // 퀴즈 배열
        //정답을 랜덤위치에 넣고
        Debug.Log(list.sheets[level].list[questionId].cor);
        answer_string_list[question_no] = list.sheets[level].list[questionId].cor;
        QuizTextList[answer_list[question_no]].text = list.sheets[level].list[questionId].cor;

        //wj
        ref_answer_string = answer_string_list[question_no];
        _totalStorageScript.tmpLevel[0] = level;
//        totalStorageScript.tmpStage[0] = stage_no;
        //보기들을 나머지 위치에 넣음
        //
        int tmp = 1;
        for (int i = 0; i < 5; i++)
        {
            // i 가 만약 정답이 있는 index가 아니라면 해당 index의 text에 값을 넣어야 함.
            if (i != answer_list[question_no])
            {
                switch (tmp)
                {
                    case 1:
                        QuizTextList[i].text = list.sheets[level].list[questionId].ex1;
                        break;
                    case 2:
                        QuizTextList[i].text = list.sheets[level].list[questionId].ex2;
                        break;
                    case 3:
                        QuizTextList[i].text = list.sheets[level].list[questionId].ex3;
                        break;
                    case 4:
                        QuizTextList[i].text = list.sheets[level].list[questionId].ex4;
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
//            StartCoroutine(Octo.GetComponent<OctoSinusodialMove>().MoveOctopus());
            yield return Octo.GetComponent<OctoSinusodialMove>().MoveOctopus();
            yield return new WaitForSeconds(1f);
            SoundManager.Instance.Play_SpeechBubblePop();
            Octo.transform.Find("DescriptionBubble").gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);

            int k = 0;
            while (k < 5)
            {
                TrashInBarrel[k].SetActive(true);
                CoinInBarrel[k].SetActive(true);
                CoinInBarrel[k].GetComponent<Animator>().SetBool("Appear", false);
                k = k + 1;
            }

            int i = 0;
            while (i < 5)
            {
                yield return new WaitForSeconds(0.2f);

                //하나씩 SetActive
                SoundManager.Instance.Play_BarrelCreated();
                Barrels[i].SetActive(true);
                animators[i].Play("Entry");
                i += 1;
            }

            yield return new WaitForSeconds(1f);
            //originalPosition 이 false인 경우에는 아무것도 하지 않다가 true가 되면 break한다.
            string wordFileLink = $"Sounds/Detection/{list.sheets[level].list[questionId].filename}";
            Octo.GetComponent<AudioSource>().loop = false;
            Octo.GetComponent<AudioSource>().clip = Resources.Load(wordFileLink) as AudioClip;
            Debug.Log(Resources.Load(wordFileLink) as AudioClip);
            Octo.GetComponent<AudioSource>().Play();
            Debug.Log("타이머 스타트");
            //클릭 타임이
            isLoading = false;
            watch.Start();
            yield return new WaitForSeconds(1.0f);
            if (!SoundManager.Instance.IsMusicPlaying())
            {
                SoundManager.Instance.Play_SoundOctopusMove();
            }


//            director.GetComponent<GameDirector>().DecreaseTimer(1f);
        }
    }

    public void StageOver()
    {
        responseTime = watch.ElapsedMilliseconds;
        watch.Stop();
        //기록 남기기
        Debug.Log($"{watch.ElapsedMilliseconds}ms 이후 종 ");
        watch.Reset();

        StartCoroutine(HideAnswers());
    }

    IEnumerator HideAnswers()
    {
        yield return new WaitForSeconds(1f);
        int i = 0;
        while (i < 5)
        {
            TrashInBarrel[i].SetActive(false);
            CoinInBarrel[i].SetActive(false);
            animators[i].Play("BarrelDestroy");
//            yield return new WaitForSeconds(2f);
            Debug.Log(GetAnimationClip(animators[i], "BarrelDestroy").length);
            yield return new WaitForSeconds(GetAnimationClip(animators[i], "BarrelDestroy").length);
            Barrels[i].SetActive(false);
            i = i + 1;
        }

        eachQuestionStorageScript.DQM = this;
        refQuestionNumber = question_no;

        eachQuestionStorageScript.SaveDetectionDataForEachQuestion();

        chosenAns = new List<string>();
        Debug.Log("level:" + level + ",stage: " + question_no);
        Debug.Log(JsonConvert.SerializeObject(chosenAns, Formatting.Indented));

        // stage가 모두 끝났다면 결과 창으로, 그렇지 않다면 다음 스테이지로 넘어간
        Debug.Log($"stage_no는 {question_no}이고, max_stage_no는 {max_question_no}");
        if (question_no < max_question_no - 1)
        {
            question_no++;

            //_totalStorageScript.tmpStage[0] = stage_no;

            StartCoroutine(StageEach(level));
        }
        else
        {
            _totalStorageScript.tmpLevel[0]++;

            // 지워야할 코드
            question_no++;

            //_totalStorageScript.tmpStage[0] = 0;

            //totalStorageScript.InitStageData();
            Debug.Log("결과창 setActive");

            //totalStorageScript.tmpPerfection = total_correct * 100;
            //totalStorageScript.CheckObtainedStarSlider((uint) total_correct);
            //eachQuestionStorageScript.SaveGameOver(EGameName.Detection,level,ref_stage_no,questionId,);
            //totalStorageScript.Save(EGameName.Detection, level);

            yield return DecideResult(total_clicked, total_correct);
        }
    }


    public AnimationClip GetAnimationClip(Animator animator, string name)
    {
        if (!animator) return null; // no animator

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null; // no clip by that name
    }

    IEnumerator DecideResult(int totalClicked, int totalCorrect)
    {
        yield return new WaitForSeconds(1.0f);
        _resultHandler.OpenResult();
        SoundManager.Instance.Play_StarShowedUp();
        string result_text = $"{totalCorrect/10.0f * 100} 도달!";
        descriptionText.text = result_text;
        string[] sentences = {"우리 계속 동전을 찾아보자!", "와~ 고마워!"};

        ///////////////임시 코드///////////////////////////
        if (totalCorrect == max_question_no)
        {
            StarMiddle.fillAmount = 1f;
            // 별 1개 채워짐
            onesentenceText.text = sentences[1];
            levelStorageScript.obtainedStarCnt[level, stage] = 4;
        }
        else
        {
            StarMiddle.fillAmount = 0.25f;
            onesentenceText.text = sentences[0];
            levelStorageScript.obtainedStarCnt[level, stage] = 1;
        }
        //////////////////////////////////////////////

        /*if (totalCorrect <= 3)
        {
            onesentenceText.text = sentences[0];
            // 별 1/4
            StarMiddle.fillAmount = 0.25f;
            if (levelStorageScript.obtainedStarCnt[level, stage] != 4)
            {
                levelStorageScript.obtainedStarCnt[level, stage] = 1;
            }
            
        }
        else if (totalCorrect <= 6)
        {
            // 별 2/4
            onesentenceText.text = sentences[0];
            StarMiddle.fillAmount = 0.5f;
            if (levelStorageScript.obtainedStarCnt[level, stage] != 4)
            {
                levelStorageScript.obtainedStarCnt[level, stage] = 2;
            }
            
        }
        else if (totalCorrect <= 9)
        {
            // 별 3/4
            onesentenceText.text = sentences[0];
            StarMiddle.fillAmount = 0.75f;
           if (levelStorageScript.obtainedStarCnt[level, stage] != 4)
            {
                levelStorageScript.obtainedStarCnt[level, stage] = 3;
            }
        }
        else if (totalCorrect == questionMaxNumber)
        {
            // 별 1개 채워짐
            onesentenceText.text = sentences[1];
            StarMiddle.fillAmount = 1f;
            levelStorageScript.obtainedStarCnt[level, stage] = 4;
        }
        else
        {
            Debug.Assert(false, "문제가 10개 초과");
        }*/

        stageStorageScript.LoadGameStageData(EGameName.Detection, _totalStorageScript.currId, level, stage);
        stageStorageScript.playCnt++;
        eachQuestionStorageScript.SaveGameOver(EGameName.Detection, level, stage, questionId,
            stageStorageScript.playCnt);

        
        
        stageStorageScript.SaveGameStageData(EGameName.Detection, _totalStorageScript.currId, level, stage,
            totalCorrect);

        //levelData 계산
        levelStorageScript.avgPerfection[level] =
            stageStorageScript.GetAvgCorrectAnswerCountForLevel(_totalStorageScript.currId, level, EGameName.Detection);
        levelStorageScript.avgResponseTime[level] =
            stageStorageScript.GetAvgResponseTimeForLevel(_totalStorageScript.currId, level, EGameName.Detection);
        levelStorageScript.SaveLevelData(EGameName.Detection, _totalStorageScript.currId, level);

        _totalStorageScript.Save(EGameName.Detection, level, stage);

        eachQuestionStorageScript.initializeQuestionData();
        
        
    }


    /*IEnumerator DecideResult(float tcl, float tco)
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
            _totalStorageScript.tmpStars[0, level] = 0;
        }
        else
        {
            SoundManager.Instance.Play_StarShowedUp();
            onesentenceText.text = sentences[2];
            StarLeft.SetActive(true);
            _totalStorageScript.tmpStars[0, level]++;
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
            _totalStorageScript.tmpStars[0, level]++;
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
            _totalStorageScript.tmpStars[0, level]++;
            if (_totalStorageScript.tmpMaxLevel[0] == level)
            {
                _totalStorageScript.tmpMaxLevel[0] = level + 1;
            }

            yield return new WaitForSeconds(.5f);
        }

        
        if (_totalStorageScript.tmpMaxLevel[0] > level)
        {
            _totalStorageScript.tmpStars[0, level] = 3;
        }
        
    }*/
}
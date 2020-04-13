using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class AlternativeQuizManager : MonoBehaviour
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


    // Bubble 없어지는 animation
    [HideInInspector] Animator[] animators = new Animator[5];

    // excel data
//    public Entity_Alternative list;
    public ALT_DataList_0329 data;


    // 쉬움, 보통, 어려움 난이도 
    public int level = 0;

    // 각 난이도 안의 step
    public int stage; //0,1,2

    //문제 엑셀 ID
    public int questionId;
    private static List<int> randomNoDuplicates;

    // 각 stage 안에는 질문 0 부터 max_question_no - 1까지의 질문이 존재한다.
    public static int question_no = 0;
    public static int max_question_no = 0;

    //wj
    public int ref_stage_no;

    // 한 레벨이 끝날 때 까지 
    public int total_clicked = 0;
    public int total_correct = 0;

    // Bubble1, Bubble2, Bubble3, Bubble4, Bubble5 -> 음소를 담고 있는 Object
    [HideInInspector] public GameObject[] Bubbles = new GameObject[5];

    // 왼쪽에 있는 해마
    [HideInInspector] public GameObject SeahorseLeft;

    // 오른쪽에 있는 해마
    [HideInInspector] public GameObject SeahorseRight;

    // 처음에 보여지는 글자 (excel에 origin에 해당)
    [HideInInspector] public GameObject WordBoxOrigin;

    // 목표 글자 (excel에 target에 해당)
    [HideInInspector] public GameObject WordBoxExpect;

    // 원 글자 스피커
//    [HideInInspector] public GameObject OriginWordSpeakerGameObject;

    // Speakers

    // WordBoxOrigin의 하위 오브젝트인 Speaker의 AudioSource Component,
    // Start에서 초기화함
    [HideInInspector] public AudioSource OriginWordSpeaker;

    // SeahorseRight의 하위 오브젝트인 Speaker의 AudioSource Component
    // Start에서 초기화함
    [HideInInspector] public AudioSource ExpectWordSpeaker;


    // 타이머 관련 변수
    private float timer = 0f;
    private float timeLimit = 60f;
    public Stopwatch watch = new Stopwatch();
    public float responseTime;

    // stage 1 ~ 5 까지의 정답 리스를 저장 -> stage_no는 0~4가 된다.
    // 정답은 Bubble[0~4]중 랜덤으로 위치해야 한다.
    [HideInInspector] public static int[] answer_list;

    // answer_string_list는 각 스테이지별 정답
    [HideInInspector] public static string[] answer_string_list;

    //wj
    public string ref_answer_string;
    public List<string> chosenAns;
    public bool isUserRight;

    // 매 스테이지에서 Bubble[0~4] 에 들어갈 보기들이 여기 들어감.
    // 매 스테이지마다 초기화되어야 함
    public TextMeshPro[] QuizTextList = new TextMeshPro[5];

    // 매 스테이지에서 time이 넘어가게 되면 TimeOver 함수를 호출하는 과정에 필요한 변수 
    [HideInInspector] private bool run_once = false;

    [HideInInspector] public bool is_loading = true;

    // Start is called before the first frame update


    // WordBoxOrigin을 가운데로 이동시키는데 필요한 변수들
    [SerializeField] private float moveSpeed = 5f;
    private float previousDistanceToTouchPos, currentDistanceToTouchPos;
    private bool isMoving = false;
    private Vector3 centerPosition, whereToMove;
    private Rigidbody2D rb;


    private bool CheckPaused = false;


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


        level = _totalStorageScript.chosenLevel;
        stage = _totalStorageScript.chosenStage;
        question_no = 0;
        chosenAns = new List<string>();

        max_question_no = 10;

        this.director = GameObject.Find("AlternativeGameDirector");
        this.description = GameObject.Find("DescriptionBubble");

        // 해마새기들
        this.SeahorseLeft = transform.Find("SeahorseLeft").gameObject;
        this.SeahorseRight = transform.Find("SeahorseRight").gameObject;

        // 보기글자들
        Transform WordsTransform = transform.Find("Words").transform;
        this.WordBoxOrigin = WordsTransform.Find("WordBoxOrigin").gameObject;
        this.WordBoxExpect = WordsTransform.Find("WordBoxExpect").gameObject;
//        this.OriginWordSpeakerGameObject = WordsTransform.Find("Speaker").gameObject;

        // 원 발음 스피커 컴포넌트
//        this.OriginWordSpeaker = WordBoxOrigin.transform.Find("Speaker").gameObject.GetComponent<AudioSource>();
        this.OriginWordSpeaker = WordBoxOrigin.GetComponent<AudioSource>();
        // 기대 발음 스피커 컴포넌트
        this.ExpectWordSpeaker = SeahorseRight.transform.Find("RepeatSound").gameObject.GetComponent<AudioSource>();
        ExpectWordSpeaker.playOnAwake = false;

        centerPosition = transform.Find("Words").transform.Find("WordCenterPosition").position;
        rb = WordBoxOrigin.GetComponent<Rigidbody2D>();


        // Bubble GameObject에 담자..
        for (int i = 0; i < 5; i++)
        {
            this.Bubbles[i] = transform.Find($"Bubble{i + 1}").gameObject;
            this.QuizTextList[i] = this.Bubbles[i].transform.Find("Text").GetComponent<TextMeshPro>();
//            this.animators[i] = this.Bubbles[i].GetComponent<Animator>();
        }

        answer_list = new int[max_question_no];
        answer_string_list = new string[max_question_no];


        if (question_no == 0)
        {
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
            watch.Start();
            CheckPaused = false;
        }

        if (watch.ElapsedMilliseconds > 0)
        {
            director.GetComponent<AlternativeGameDirector>().SetTime(watch.ElapsedMilliseconds / 1000.0f);
        }

        if (isMoving)
        {
            currentDistanceToTouchPos = (centerPosition - WordBoxOrigin.transform.position).magnitude;
        }

        if (!is_loading)
        {
            // origin box moving
            previousDistanceToTouchPos = 0;
            currentDistanceToTouchPos = 0;
            isMoving = true;
            whereToMove = (centerPosition - WordBoxOrigin.transform.position).normalized;
            rb.velocity = new Vector2(whereToMove.x * moveSpeed, whereToMove.y * moveSpeed);
        }


        if (currentDistanceToTouchPos > previousDistanceToTouchPos)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }


        if (isMoving)
        {
            previousDistanceToTouchPos = (centerPosition - WordBoxOrigin.transform.position).magnitude;
        }

        // 타임오버 되었을 떄
        if (!run_once && watch.ElapsedMilliseconds > 60000f)
        {
            Debug.Log("Stage Over");
            run_once = true;
            this.StageOver();
        }

        // 오른쪽 해마를 눌렀을 때
    }

    public void QuizInit()
    {
        int i = 0;
        // stage 0~4까지의 정답을 정해서 answer_list에 저장한다.
        while (i < max_question_no)
        {
            answer_list[i] = UnityEngine.Random.Range(0, 5);
            i++;
        }

        StartCoroutine(StageEach(level));
    }

    IEnumerator StageEach(int level)
    {
        questionId = stage * 10 + randomNoDuplicates[question_no];
        //questionId = randomNoDuplicates[question_no];
        total_clicked = 0;

        run_once = false;
        director.GetComponent<AlternativeGameDirector>().InitTime();
        description.GetComponent<AlternativeDescriptionController>().DefaultDescription();

        // UI 설정
        this.director.GetComponent<AlternativeGameDirector>().setStage(question_no);
        this.director.GetComponent<AlternativeGameDirector>().setLevel(level);

        // 보기 설정
        WordBoxOrigin.transform.Find("Text").GetComponent<TextMeshPro>().text =
            data.sheets[level].list[questionId].원자극;
        WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().text =
            data.sheets[level].list[questionId].후자극;

        // 원자극파일연결
        string originWordFileLink = $"Sounds/Alternative/{data.sheets[level].list[questionId].원자극음성}";
//        Debug.Log(originWordFileLink);
        OriginWordSpeaker.loop = false;
        OriginWordSpeaker.clip = Resources.Load(originWordFileLink) as AudioClip;

        // 퀴즈 배열
        // 정답을 랜덤위치에 넣고
        answer_string_list[question_no] = data.sheets[level].list[questionId].정답;
        QuizTextList[answer_list[question_no]].text = data.sheets[level].list[questionId].정답;

        //wj
        ref_answer_string = answer_string_list[question_no];
        _totalStorageScript.tmpLevel[3] = level;


        // 보기들을 나머지 위치에 넣음
        int tmp = 1;
        for (int i = 0; i < 5; i++)
        {
            // i 가 만약 정답이 있는 index가 아니라면 해당 index의 text에 값을 넣어야 함.
            if (i != answer_list[question_no])
            {
                switch (tmp)
                {
                    case 1:
                        QuizTextList[i].text = data.sheets[level].list[questionId].오답1;
                        break;
                    case 2:
                        QuizTextList[i].text = data.sheets[level].list[questionId].오답2;
                        break;
                    case 3:
                        QuizTextList[i].text = data.sheets[level].list[questionId].오답3;
                        break;
                    case 4:
                        QuizTextList[i].text = data.sheets[level].list[questionId].오답4;
                        break;
                    default:
                        Debug.Log("? 일어날 수 없습니다.");
                        break;
                }

                tmp++;
            }
        }

        // 퀴즈 시작 && 타이머 시작
        {
            // 제시어와 목적 단어 띄우기 ()
            // 왼쪽 해마 워터폴 이펙
            yield return new WaitForSeconds(.2f);
            SoundManager.Instance.Play_SeahorseWaves();
            SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
//            Debug.Log(SeahorseLeft.GetComponent<SeahorseLeftController>().waterFallAnimator);
            Animator left_waterFallAnimator = SeahorseLeft.GetComponent<SeahorseLeftController>().waterFallAnimator;

            left_waterFallAnimator.Play("WaterFall");

            yield return new WaitForSeconds(left_waterFallAnimator.GetCurrentAnimatorStateInfo(0).length);
            SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(false);

            // 제시어 박스 살리기
            WordBoxOrigin.transform.position = new Vector3(-2, 1, 0);
            WordBoxOrigin.SetActive(true);
            SoundManager.Instance.Play_AlterWordShowedUp();
            yield return new WaitForSeconds(.4f);

//            Debug.Log("right start");
            SoundManager.Instance.Play_SeahorseWaves();
            SeahorseRight.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
            Animator right_waterFallAnimator = SeahorseRight.GetComponent<SeahorseRightController>().waterFallAnimator;
            right_waterFallAnimator.Play("WaterFall");

            yield return new WaitForSeconds(right_waterFallAnimator.GetCurrentAnimatorStateInfo(0).length);
            SeahorseRight.transform.Find("WaterFallAnimation").gameObject.SetActive(false);

            // 목적어 박스 살리기
            WordBoxExpect.GetComponent<SpriteRenderer>().color = Color.white;
            WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color = Color.white;
            WordBoxExpect.SetActive(true);
            SoundManager.Instance.Play_AlterWordShowedUp();
            yield return new WaitForSeconds(.4f);

            // 퀴즈 띄우기
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.3f);
                // 하나씩 setActive
//                Debug.Log($"{i}번째꺼 ");
                SoundManager.Instance.Play_AlterBubbleShowedUp();
                Bubbles[i].SetActive(true);
//                animators
            }

            // 마지막 보기가 나온 뒤에 0.3초 더 기다림
            yield return new WaitForSeconds(1f);


            if (SoundManager.Instance.IsMusicPlaying())
            {
                SoundManager.Instance.PauseMusic();
            }

            yield return new WaitForSeconds(1f);
            string wordFileLink = $"Sounds/Alternative/{data.sheets[level].list[questionId].후자극음성}";
            // 2020.03.29 송동욱
            // 유니티 오브젝트에 익숙치 않아서 유사 코드가 중복...ㅠ

            // 처음 들려주는 후자극음성
            SeahorseRight.GetComponent<AudioSource>().loop = false;
            SeahorseRight.GetComponent<AudioSource>().clip = Resources.Load(wordFileLink) as AudioClip;
            SeahorseRight.GetComponent<AudioSource>().Play();

            // 사용자가 버튼을 클릭하면 
            ExpectWordSpeaker.loop = false;
            ExpectWordSpeaker.clip = Resources.Load(wordFileLink) as AudioClip;

            yield return new WaitForSeconds(1f);
            SoundManager.Instance.UnpauseMusic();

            yield return new WaitForSeconds(1f);
            Color color = WordBoxExpect.GetComponent<SpriteRenderer>().color;
            while (color.a > 0.0f)
            {
                color.a -= 0.2f;
                WordBoxExpect.GetComponent<SpriteRenderer>().color = color;
                WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color = color;
                yield return new WaitForSeconds(0.1f);
            }


//            Debug.Log("타이머 스타트");
            // 클릭 타임 
            watch.Start();
            is_loading = false;
            yield return new WaitForSeconds(1f);
            if (!SoundManager.Instance.IsMusicPlaying())
            {
                SoundManager.Instance.Play_AlternativeMusic();
            }

            // 다시 듣기 말풍선 띄우기
            SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(true);
//            OriginWordSpeakerGameObject.SetActive(true);
        }
    }

    public void StageOver()
    {
        responseTime = watch.ElapsedMilliseconds;
        watch.Stop();
        is_loading = true;
//        Debug.Log($"{watch.ElapsedMilliseconds}ms 이후 종료되었습니다. ");
        watch.Reset();
        StartCoroutine(HideAnswers());
    }

    IEnumerator HideAnswers()
    {
        // 다시 듣기 말풍선 가리기
        SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(false);
//        OriginWordSpeakerGameObject.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.3f);
            Bubbles[i].SetActive(false);
        }

        yield return new WaitForSeconds(0.3f);
        WordBoxOrigin.SetActive(false);
        WordBoxExpect.SetActive(false);


        // 다음 스테이지로 가던지, 아니면 결과창을 보여 주던지
        SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(false);

        eachQuestionStorageScript.AQM = this;
        ref_stage_no = question_no;
        //Debug.Log("stage"+stage_no+"ChosenAns전 = " + JsonConvert.SerializeObject(chosenAns, Formatting.Indented));
        eachQuestionStorageScript.SaveAlternativeDataForEachQuestion();
        chosenAns = new List<string>();
        //Debug.Log("stage"+stage_no+"ChosenAns후 = " + JsonConvert.SerializeObject(chosenAns, Formatting.Indented));


        if (question_no < max_question_no - 1)
        {
            question_no = question_no + 1;
            // _totalStorageScript.tmpStage[3] = stage_no;

            StartCoroutine(StageEach(level));
        }
        else
        {
            // 지워야할 코드
            question_no++;

            //  _totalStorageScript.tmpStage[3] = 0;


            //totalStorageScript.InitStageData();
//            Debug.Log("결과창 setActive");
            // Game 이 끝났다는 효과 보여주기
            _totalStorageScript.tmpLevel[3]++;

            //totalStorageScript.tmpPerfection = total_correct * 100;
            //totalStorageScript.CheckObtainedStarSlider((uint) total_correct);
            // StageStorageScript.SaveGameOver(EGameName.Alternative);
            //totalStorageScript.Save(EGameName.Alternative, level);

            yield return DecideResult(this.total_clicked, this.total_correct);
        }

//        this.GoNextStage();
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
        string result_text = $"{totalCorrect / 10.0f * 100} 도달!";
        descriptionText.text = result_text;
        string[] sentences = {"다시 해보자", "와~ 너 너무 잘한다!"};
        //////////////////임시코드
//        if (totalCorrect == max_question_no)
//        {
//            // 별 1개 채워짐
//            StarMiddle.fillAmount = 1f;
//            onesentenceText.text = sentences[1];
//            levelStorageScript.obtainedStarCnt[level, stage] = 4;
//        }
//        else
//        {
//            onesentenceText.text = sentences[0];
//            StarMiddle.fillAmount = 0.25f;
//            levelStorageScript.obtainedStarCnt[level, stage] = 1;
//        }

        ///////////////

        if (totalCorrect <= 3)
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
        else if (totalCorrect == 10)
        {
            // 별 1개 채워짐
            onesentenceText.text = sentences[1];
            StarMiddle.fillAmount = 1f;
            levelStorageScript.obtainedStarCnt[level, stage] = 4;
        }
        else
        {
            Debug.Assert(false, "문제가 10개 초과");
        }
        
        
        SoundManager.Instance.StopMusic();
        
        yield return new WaitForSeconds(1.0f);

        if (totalCorrect == max_question_no)
        {
            SoundManager.Instance.Play_Narration("Alternative", 7);
        }
        else
        {
            SoundManager.Instance.Play_Narration("Alternative", 9);
        }


        stageStorageScript.LoadGameStageData(EGameName.Alternative, _totalStorageScript.currId, level, stage);
        stageStorageScript.playCnt++;
        eachQuestionStorageScript.SaveGameOver(EGameName.Alternative, level, stage, questionId,
            stageStorageScript.playCnt);


        stageStorageScript.SaveGameStageData(EGameName.Alternative, _totalStorageScript.currId, level, stage,
            totalCorrect);

        //levelData 계산
        levelStorageScript.avgPerfection[level] =
            stageStorageScript.GetAvgCorrectAnswerCountForLevel(_totalStorageScript.currId, level,
                EGameName.Alternative);

        levelStorageScript.avgResponseTime[level] =
            stageStorageScript.GetAvgResponseTimeForLevel(_totalStorageScript.currId, level, EGameName.Alternative);
        levelStorageScript.SaveLevelData(EGameName.Alternative, _totalStorageScript.currId, level);

        _totalStorageScript.Save(EGameName.Alternative, level, stage);

        eachQuestionStorageScript.initializeQuestionData();
    }
}


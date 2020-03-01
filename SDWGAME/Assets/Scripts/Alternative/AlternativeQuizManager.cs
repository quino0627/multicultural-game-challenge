using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AlternativeQuizManager : MonoBehaviour
{
    //KeepTrackController ConclusionData
    public GameObject totalStorageObject;
    private KeepTrackController totalStorageScript;

    private GameObject StageStorage;
    private DataController StageStorageScript;

    // director
    private GameObject director;
    private GameObject description;

    public GameObject StarLeft;
    public GameObject StarMiddle;
    public GameObject StarRight;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI onesentenceText;

    // result handler
    public ResultHandler _resultHandler;


    // Bubble 없어지는 animation
    [HideInInspector] Animator[] animators = new Animator[5];

    // excel data
    public Entity_Alternative list;

    // 쉬움, 보통, 어려움 난이도 
    public int level = 0;

    // 각 난이도 안에는 stage0 부터 max_stage_no - 1까지의 stage가 존재한다.
    public static int stage_no = 0;
    public static int max_stage_no = 0;

    //wj
    public int ref_stage_no;

    // 한 레벨이 끝날 때 까지 
    public int total_clicked = 0;
    public int total_correct = 0;

    // Bubble1, Bubble2, Bubble3, Bubble4, Bubble5 -> 음소를 담고 있는 Object
    [HideInInspector] public GameObject[] Bubbles = new GameObject[5];

    // 왼쪽에 있는 해마새기
    [HideInInspector] public GameObject SeahorseLeft;

    // 오른쪽에 있는 해마새기
    [HideInInspector] public GameObject SeahorseRight;

    // 처음에 보여지는 글자 (excel에 origin에 해당)
    [HideInInspector] public GameObject WordBoxOrigin;

    // 목표 글자 (excel에 target에 해당)
    [HideInInspector] public GameObject WordBoxExpect;

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
        totalStorageObject = GameObject.Find("TotalStorage");
        totalStorageScript = totalStorageObject.GetComponent<KeepTrackController>();
        StageStorage = GameObject.Find("StageStorage");
        StageStorageScript = StageStorage.GetComponent<DataController>();

        //stage_no = 0;
        level = totalStorageScript.chosenLevel;
        stage_no = totalStorageScript.tmpStage[3];
        chosenAns = new List<string>();

        max_stage_no = 3;
        this.director = GameObject.Find("AlternativeGameDirector");
        this.description = GameObject.Find("DescriptionBubble");

        // 해마새기들
        this.SeahorseLeft = transform.Find("SeahorseLeft").gameObject;
        this.SeahorseRight = transform.Find("SeahorseRight").gameObject;

        // 보기글자들
        Transform WordsTransform = transform.Find("Words").transform;
        this.WordBoxOrigin = WordsTransform.Find("WordBoxOrigin").gameObject;
        this.WordBoxExpect = WordsTransform.Find("WordBoxExpect").gameObject;

        centerPosition = transform.Find("Words").transform.Find("WordCenterPosition").position;
        rb = WordBoxOrigin.GetComponent<Rigidbody2D>();


        // Bubble GameObject에 담자..
        for (int i = 0; i < 5; i++)
        {
            this.Bubbles[i] = transform.Find($"Bubble{i + 1}").gameObject;
            this.QuizTextList[i] = this.Bubbles[i].transform.Find("Text").GetComponent<TextMeshPro>();
//            this.animators[i] = this.Bubbles[i].GetComponent<Animator>();
        }

        max_stage_no = 3;
        answer_list = new int[max_stage_no];
        answer_string_list = new string[max_stage_no];

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
        while (i < max_stage_no)
        {
            answer_list[i] = UnityEngine.Random.Range(0, 5);
            i++;
        }

        StartCoroutine(StageEach(level));
    }

    IEnumerator StageEach(int level)
    {
        // 시작 오디오 세팅

        run_once = false;
        director.GetComponent<AlternativeGameDirector>().InitTime();
        description.GetComponent<AlternativeDescriptionController>().DefaultDescription();

        // UI 설정
        this.director.GetComponent<AlternativeGameDirector>().setStage(stage_no);
        this.director.GetComponent<AlternativeGameDirector>().setLevel(level);

        // 보기 설정
        WordBoxOrigin.transform.Find("Text").GetComponent<TextMeshPro>().text =
            list.sheets[level].list[stage_no].origin;
        WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().text =
            list.sheets[level].list[stage_no].expect;

        // 퀴즈 배열
        // 정답을 랜덤위치에 넣고
        answer_string_list[stage_no] = list.sheets[level].list[stage_no].cor;
        QuizTextList[answer_list[stage_no]].text = list.sheets[level].list[stage_no].cor;

        //wj
        ref_answer_string = answer_string_list[stage_no];
        totalStorageScript.tmpLevel[3] = level;


        // 보기들을 나머지 위치에 넣음
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
//            Debug.Log("left start");
            SoundManager.Instance.Play_SeahorseWaves();
            SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
            Debug.Log(SeahorseLeft.GetComponent<SeahorseLeftController>().waterFallAnimator);
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
                Debug.Log($"{i}번째꺼 ");
                SoundManager.Instance.Play_AlterBubbleShowedUp();
                Bubbles[i].SetActive(true);
//                animators
            }

            // 마지막 보기가 나온 뒤에 0.3초 더 기다림
            yield return new WaitForSeconds(1f);


            if (SoundManager.Instance.IsMusicPlaying())
            {
                SoundManager.Instance.StopMusic();
            }

            yield return new WaitForSeconds(1f);
            string wordFileLink = $"Sounds/Alternative/{list.sheets[level].list[stage_no].filename}";
//            Debug.Log(wordFileLink);
            SeahorseRight.GetComponent<AudioSource>().loop = false;
            SeahorseRight.GetComponent<AudioSource>().clip = Resources.Load(wordFileLink) as AudioClip;
            SeahorseRight.GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(1f);
            SoundManager.Instance.Play_AlternativeMusic();

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

        StageStorageScript.AQM = this;
        ref_stage_no = stage_no;
        //Debug.Log("stage"+stage_no+"ChosenAns전 = " + JsonConvert.SerializeObject(chosenAns, Formatting.Indented));
        StageStorageScript.SaveAlternative();
        chosenAns = new List<string>();
        //Debug.Log("stage"+stage_no+"ChosenAns후 = " + JsonConvert.SerializeObject(chosenAns, Formatting.Indented));


        if (stage_no < max_stage_no - 1)
        {
            stage_no = stage_no + 1;
            totalStorageScript.tmpStage[3] = stage_no;
            totalStorageScript.Save();
            StartCoroutine(StageEach(level));
        }
        else
        {
            // 지워야할 코드
            stage_no++;
            
            totalStorageScript.tmpStage[3] = 0;
            
            
            //totalStorageScript.InitStageData();
//            Debug.Log("결과창 setActive");
            // Game 이 끝났다는 효과 보여주기
            totalStorageScript.tmpLevel[3]++;

            yield return DecideResult(this.total_clicked, this.total_correct);
            totalStorageScript.Save();
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
            totalStorageScript.tmpStars[3, level] = 0;
        }
        else
        {
            SoundManager.Instance.Play_StarShowedUp();
            onesentenceText.text = sentences[2];
            StarLeft.SetActive(true);
            totalStorageScript.tmpStars[3, level]++;
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
            totalStorageScript.tmpStars[3, level]++;
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
            totalStorageScript.tmpStars[3, level]++;
            if (totalStorageScript.tmpMaxLevel[3] == level)
            {
                totalStorageScript.tmpMaxLevel[3] = level + 1;
            }

            yield return new WaitForSeconds(.5f);
        }

        if (totalStorageScript.tmpMaxLevel[3] > level)
        {
            totalStorageScript.tmpStars[3, level] = 3;
        }
    }
}
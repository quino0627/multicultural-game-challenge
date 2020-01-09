using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AlternativeQuizManager : MonoBehaviour
{
    
    // director
    private GameObject director;
    private GameObject description;
    
    // Bubble 없어지는 animation
    [HideInInspector] Animator[] animators = new Animator[5];
    // excel data
    public Entity_Alternative list;
    // 쉬움, 보통, 어려움 난이도 
    public int level = 0;
    // 각 난이도 안에는 stage0 부터 max_stage_no - 1까지의 stage가 존재한다.
    public static int stage_no = 0;
    public static int max_stage_no;
        
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
    
    // stage 1 ~ 5 까지의 정답 리스를 저장 -> stage_no는 0~4가 된다.
    // 정답은 Bubble[0~4]중 랜덤으로 위치해야 한다.
    [HideInInspector] public static int[] answer_list;
    // answer_string_list는 각 스테이지별 정답
    [HideInInspector] public static string[] answer_string_list;
    
    // 매 스테이지에서 Bubble[0~4] 에 들어갈 보기들이 여기 들어감.
    // 매 스테이지마다 초기화되어야 함
    public TextMeshPro[] QuizTextList = new TextMeshPro[5];
    
    // 매 스테이지에서 time이 넘어가게 되면 TimeOver 함수를 호출하는 과정에 필요한 변수 
    [HideInInspector] private bool run_once = false;

    [HideInInspector] public bool is_loading = true;
    
    // Start is called before the first frame update
    
    
    // WordBoxOrigin을 가운데로 이동시키는데 필요한 변수들
    [SerializeField]
    private float moveSpeed = 5f;
    private float previousDistanceToTouchPos, currentDistanceToTouchPos;
    private bool isMoving = false;
    private Vector3 centerPosition, whereToMove;
    private Rigidbody2D rb;
    
    void Start()
    {
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
            this.Bubbles[i] = transform.Find($"Bubble{i+1}").gameObject;
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
        if (watch.ElapsedMilliseconds > 0)
        {
            director.GetComponent<AlternativeGameDirector>().SetTime(watch.ElapsedMilliseconds/1000.0f);
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
            Debug.Log(centerPosition);
            Debug.Log(WordBoxOrigin.transform.position);
            Debug.Log(whereToMove);
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
        WordBoxOrigin.transform.Find("Text").GetComponent<TextMeshPro>().text = list.sheets[level].list[stage_no].origin;
        WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().text = list.sheets[level].list[stage_no].expect;
        
        // 퀴즈 배열
        // 정답을 랜덤위치에 넣고
        answer_string_list[stage_no] = list.sheets[level].list[stage_no].cor;
        QuizTextList[answer_list[stage_no]].text = list.sheets[level].list[stage_no].cor;
        
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
            Debug.Log("left start");
            SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
            Animator left_waterFallAnimator = SeahorseLeft.GetComponent<SeahorseLeftController>().waterFallAnimator;
            left_waterFallAnimator.Play("WaterFall");
            
            yield return new WaitForSeconds(left_waterFallAnimator.GetCurrentAnimatorStateInfo(0).length);
            SeahorseLeft.transform.Find("WaterFallAnimation").gameObject.SetActive(false);

            // 제시어 박스 살리기
            WordBoxOrigin.transform.position = new Vector3(-2,1,0);
            WordBoxOrigin.SetActive(true);
            yield return new WaitForSeconds(.4f);
            
            Debug.Log("right start");
            SeahorseRight.transform.Find("WaterFallAnimation").gameObject.SetActive(true);
            Animator right_waterFallAnimator = SeahorseRight.GetComponent<SeahorseRightController>().waterFallAnimator;
            right_waterFallAnimator.Play("WaterFall");
            
            yield return new WaitForSeconds(right_waterFallAnimator.GetCurrentAnimatorStateInfo(0).length);
            SeahorseRight.transform.Find("WaterFallAnimation").gameObject.SetActive(false);
            
            // 목적어 박스 살리기
            WordBoxExpect.GetComponent<SpriteRenderer>().color = Color.white;
            WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color =  Color.white;
            WordBoxExpect.SetActive(true);
            
            yield return new WaitForSeconds(.4f);
            
            // 퀴즈 띄우기
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.3f);
                // 하나씩 setActive
                Debug.Log($"{i}번째꺼 ");
                Bubbles[i].SetActive(true);
//                animators
            }
            // 마지막 보기가 나온 뒤에 0.3초 더 기다림
            yield return new WaitForSeconds(0.3f);
            
            
            

            string wordFileLink = $"Sounds/Alternative/{list.sheets[level].list[stage_no].filename}";
            Debug.Log(wordFileLink);
            SeahorseRight.GetComponent<AudioSource>().loop = false;
            SeahorseRight.GetComponent<AudioSource>().clip = Resources.Load(wordFileLink) as AudioClip;
            SeahorseRight.GetComponent<AudioSource>().Play();
            
            yield return new WaitForSeconds(1f);
            Color color = WordBoxExpect.GetComponent<SpriteRenderer>().color;
            while (color.a > 0.0f) {
                color.a -= 0.2f;
                WordBoxExpect.GetComponent<SpriteRenderer>().color = color;
                WordBoxExpect.transform.Find("Text").GetComponent<TextMeshPro>().color = color;
                yield return new WaitForSeconds(0.1f);
            }


            Debug.Log("타이머 스타트");
            // 클릭 타임 
            watch.Start();
            
            is_loading = false;
            yield return new WaitForSeconds(1f);
            // 다시 듣기 말풍선 띄우기
            SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(true);
        }
    }

    public void StageOver()
    {
        watch.Stop();
        is_loading = true;
        Debug.Log($"{watch.ElapsedMilliseconds}ms 이후 종료되었습니다. ");
        watch.Reset();
        StartCoroutine(HideAnswers());
    }

    IEnumerator HideAnswers()
    {
        // 다시 듣기 말풍선 띄우기
        SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.3f);
            Bubbles[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.3f);
        WordBoxOrigin.SetActive(false);
        WordBoxExpect.SetActive(false);
        

        this.GoNextStage();
    }

    public void GoNextStage()
    {
        SeahorseRight.transform.Find("RepeatSound").gameObject.SetActive(false);
        if (stage_no < max_stage_no - 1)
        {
            stage_no = stage_no + 1;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class SpreadChoices : MonoBehaviour
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

    // Show Result
    public GameObject StarLeft;
    public Image StarMiddle;
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
    public SYN_DataList data;

    // 초급/중급/고급
    public int excelLevel;
    public int realLevel;

    // 각 난이도 안의 stage
    public int stage; //0,1,2

    //문제 엑셀 ID
    public int questionId;
    private static List<int> randomNoDuplicates;

    // 각 stage안의 질문index
    public static int questionNumber;

    // 해당 스테이지의 전체 question 개수 
    public static int questionMaxIndex;
    public static int realQuestionIndex; // 진짜 level

    public int refQuestionIndex;
    //public int levelOneQuestionMaxIndex; //일단 씬에서 결정

    //보기 이 게임에선 해파리
    public GameObject[] choices;
    public Transform[] choiceTransforms;

    //Crab
    public GameObject crab;


    //each stage jellyfish's texts
    public TextMeshPro[] choiceTexts;

    //correct jellyfish's position Index
    public List<int> corrAnsPosIndex;

    //Number of Correct Answers
    public int corrAnsCnt; // Inspector 창에서 설정

    //Number of Wrong Answers
    public int wrongAnsCnt; // Inspector 창에서 설정

    public List<string> chosenAns;

    public GameObject[] PickedAnswer;

    public Transform[] PickedJfPos;

    //시간 테스트를 위한 임시 변수
    public Text timeText;

    // 타이머 관련 변수
    private float timer = 0f;
    private float timeLimit = 60f;
    public Stopwatch watch = new Stopwatch();
    public float responseTime;

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
        SoundManager.Instance.StopMusic();

        totalStorageObject = GameObject.Find("TotalStorage");
        _totalStorageScript = totalStorageObject.GetComponent<TotalDataManager>();
        eachQuestionStorage = GameObject.Find("EachQuestionStorage");
        eachQuestionStorageScript = eachQuestionStorage.GetComponent<EachQuestionDataManager>();
        stageStorage = GameObject.Find("StageStorage");
        stageStorageScript = stageStorage.GetComponent<StageDataManager>();
        levelStorage = GameObject.Find("LevelStorage");
        levelStorageScript = levelStorage.GetComponent<LevelDataManager>();

        director = GameObject.Find("SynthesisGameDirector");

        questionMaxIndex = 10;

        //levelOneQuestionMaxIndex = data.sheets[0].list.Count - 1;


        crab.transform.Find("DescriptionBubble").gameObject.SetActive(false);

        realLevel = _totalStorageScript.chosenLevel;
        excelLevel = _totalStorageScript.chosenLevel;
        stage = _totalStorageScript.chosenStage;
        if (questionNumber == questionMaxIndex)
        {
            questionNumber = 0;
            realQuestionIndex = 0;
            total_correct = 0;
            total_correct_stage = 0;
            total_tried = 0;
        }

        //excelLevel++;


        total_tried = 0;

        refQuestionIndex = realQuestionIndex;

        if (realQuestionIndex == 0)
        {
            randomNoDuplicates = new List<int>();
            for (int i = 0; i < questionMaxIndex; ++i)
            {
                int tmp = Random.Range(0, questionMaxIndex);
                while (randomNoDuplicates.Contains(tmp))
                {
                    tmp = Random.Range(0, questionMaxIndex);
                }

                randomNoDuplicates.Add(tmp);
                Debug.Log("randomNoDuplicates[" + i + "] = " + randomNoDuplicates[i]);
            }
        }

        Debug.Log("RealQuestionIndex: " + realQuestionIndex);

        //두글자일때
        /*if (excelLevel == 1)
        {
            questionId = (stage - 1) * 10 + randomNoDuplicates[realQuestionIndex];
        }*/

        //한글자, 세글자, 네글자, 이제 두글자도!
        questionId = stage * 10 + randomNoDuplicates[realQuestionIndex];
        //questionId = randomNoDuplicates[realQuestionIndex];
        QuizInit();
    }

    void Update()
    {
        ref_total_tried = total_tried;

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
            director.GetComponent<SynthesisGameDirector>().SetTime(watch.ElapsedMilliseconds / 1000.0f);
        }

        // 타임오버 되었을 떄
        if (!run_once && watch.ElapsedMilliseconds > 60000f)
        {
            responseTime = watch.ElapsedMilliseconds;
            Debug.Log("Stage Over");
            run_once = true;
            this.GoNext();
        }
    }

    public void QuizInit()
    {
        crab.transform.position = crabStartTransform.position;
        if (excelLevel == 0)
        {
            crab.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        }

        if (excelLevel == 1)
        {
            crab.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        }

        if (excelLevel == 2)
        {
            crab.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        }

        if (excelLevel == 3)
        {
            crab.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        }

        //StartCoroutine(EnableCoroutine());
        StartCoroutine(EnableCoroutine());
    }

    IEnumerator EnableCoroutine()
    {
        run_once = false;
        // 감독의 시간을 60초로 초기화한다.
        director.GetComponent<SynthesisGameDirector>().InitTime();
        ;
        // UI 설정
        director.GetComponent<SynthesisGameDirector>().setStage(realQuestionIndex);
        director.GetComponent<SynthesisGameDirector>().setLevel(excelLevel);


        //정답의 index (corrAnsCnt: 초급-1,2개 중급-3개 고급-4개)
        //8개의 자리중 랜덤한 위치
        for (int i = 0; i < (corrAnsCnt + wrongAnsCnt); i++)
        {
            int tmp;
            do
            {
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
                Debug.Log("level " + excelLevel);
                Debug.Log("stageIndex " + questionNumber);

                Debug.Log(data.sheets[excelLevel].list[questionId].정답1);
                PickedAnswer[0].GetComponent<TextMeshPro>().text
                    = choiceTexts[corrAnsPosIndex[0]].text
                        = data.sheets[excelLevel].list[questionId].정답1;
            }

            if (i == 1)
            {
                PickedAnswer[1].GetComponent<TextMeshPro>().text
                    = choiceTexts[corrAnsPosIndex[1]].text
                        = data.sheets[excelLevel].list[questionId].정답2;
            }

            if (i == 2)
            {
                PickedAnswer[2].GetComponent<TextMeshPro>().text
                    = choiceTexts[corrAnsPosIndex[2]].text
                        = data.sheets[excelLevel].list[questionId].정답3;
            }

            /*if (i == 3)
            {
                PickedAnswer[3].GetComponent<TextMeshPro>().text
                    = choiceTexts[corrAnsPosIndex[3]].text
                        = data.sheets[excelLevel].list[questionId].정답4;
            }*/
        }

        //정답이 아닌 글자들 처리
        for (int i = corrAnsCnt; i < (corrAnsCnt + wrongAnsCnt); i++)
        {
            choices[corrAnsPosIndex[i]].tag = "WrongAns";
            if (i == corrAnsCnt)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[excelLevel].list[questionId].보기1;
            }

            if (i == corrAnsCnt + 1)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[excelLevel].list[questionId].보기2;
            }

            if (i == corrAnsCnt + 2)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[excelLevel].list[questionId].보기3;
            }

            if (i == corrAnsCnt + 3)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[excelLevel].list[questionId].보기4;
            }

            if (i == corrAnsCnt + 4)
            {
                choiceTexts[corrAnsPosIndex[i]].text
                    = data.sheets[excelLevel].list[questionId].보기5;
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

        yield return new WaitForSeconds(1.3f);

        string levelToString = "";
        switch (excelLevel)
        {
            case 0:
                levelToString = "A1_Easy";
                break;
            case 1:
                levelToString = "A2_Normal";
                break;
            case 2:
                levelToString = "A3_Hard";
                break;
            default:
                Debug.Assert(false,"Weird LevelToString");
                break;
        }
        
        string wordFileLink = $"Sounds/Synthesis/{levelToString}/{data.sheets[excelLevel].list[questionId].filename}";
        Debug.Log(data.sheets[excelLevel].list[questionId].filename);
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
        int cnt = 0;
        int jellyfish_index;
        while (cnt < corrAnsCnt + wrongAnsCnt)
        {
            yield return new WaitForSeconds(0.0f);
            //choices[maxChoiceNumber].SetActive(true);
            jellyfish_index = corrAnsPosIndex[cnt];

            //1. move to each choicePosition
            StartCoroutine(InitialJellyfish(jellyfish_index));

            cnt++;
        }
    }

    IEnumerator InitialJellyfish(int i)
    {
        // 얘는 OneShot이 아니고 백그라운드 뮤직
        if (!SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.Play_JellyFishShowedUp();
            // SoundManager.Instance.Play_SynthesisMusic();
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
                MoveJellyfish(i, choiceTransforms[i].position);
            }
            else
            {
                JfArrived[i] = true;

                //Debug.Log("InitialJellyfish JF "+ i +" became true");
            }
        }

        //DragAndDrop script에서 해파리가 제자리로 돌아가기위한 코드
        choices[i].GetComponent<DragAndDrop>().initialPosition
            = choiceTransforms[i].position;
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
        if (!initialDone && CheckAllArrived())
        {
            Debug.Log("DONE");
            // 해파리가 모두 제 자리에 왔을 때 시간을 시작.
            initialDone = true;
            // 음악도 시작

            StartCoroutine(MusicStart());
            Invoke("StartTime", 2.0f);
        }
    }

    IEnumerator MusicStart()
    {
        if (SoundManager.Instance.IsMusicPlaying())
        {
            SoundManager.Instance.StopMusic();
        }
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.Play_SynthesisMusic();
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
        eachQuestionStorageScript.SC = this;
        eachQuestionStorageScript.SaveSynthesisDataForEachQuestion();

        watch.Stop();
        /*if (stageIndex == 29)
        {
            //level++;
            stageIndex = 0;
        }
        else*/
        {
            questionNumber++;
            realQuestionIndex++;
        }

        watch.Reset();
        Debug.Log("In GoNext Invoke HideAnswer");
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


        // stage가 끝났을 경우에는 Result창을 보여주어야 한다.
        if (realQuestionIndex + 1 > questionMaxIndex)
        {
            Debug.Log("Game Is Over");
            _totalStorageScript.tmpLevel[1]++;


            _totalStorageScript.Save(EGameName.Synthesis, realLevel, stage);

            yield return DecideResult(total_tried, total_correct, total_correct_stage);
        }

        if (realQuestionIndex + 1 <= questionMaxIndex)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    IEnumerator DecideResult(int totalClicked, int totalCorrect, int totalCorrectStage)
    {
        yield return new WaitForSeconds(1.0f);
        _resultHandler.OpenResult();
        SoundManager.Instance.Play_StarShowedUp();
        string result_text = $"{totalCorrect / (float) (excelLevel + 1) / 10.0f * 100} 도달!";
        descriptionText.text = result_text;
        string[] sentences = {"다시! 다시! 아~좀!", "와~ 고마워!"};


        /////////임시코드///////////////////////////////////////////
        /*if (totalCorrectStage == questionMaxIndex)
        {
            // 별 1개 채워짐
            StarMiddle.fillAmount = 1f;
            onesentenceText.text = sentences[1];
            levelStorageScript.obtainedStarCnt[realLevel, stage] = 4;
        }
        else
        {
            StarMiddle.fillAmount = 0.25f;
            onesentenceText.text = sentences[0];
            levelStorageScript.obtainedStarCnt[realLevel, stage] = 1;
        }*/
        ////////////////////////////////////////////////////////////////

        if (totalCorrectStage <= 3)
        {
            onesentenceText.text = sentences[0];
            // 별 1/4
            StarMiddle.fillAmount = 0.25f;
            if (levelStorageScript.obtainedStarCnt[realLevel, stage] != 4)
            {
                levelStorageScript.obtainedStarCnt[realLevel, stage] = 1;
            }
        }
        else if (totalCorrectStage <= 6)
        {
            // 별 2/4
            onesentenceText.text = sentences[0];
            StarMiddle.fillAmount = 0.5f;
            if (levelStorageScript.obtainedStarCnt[realLevel, stage] != 4)
            {
                levelStorageScript.obtainedStarCnt[realLevel, stage] = 2;
            }
        }
        else if (totalCorrectStage <= 9)
        {
            // 별 3/4
            onesentenceText.text = sentences[0];
            StarMiddle.fillAmount = 0.75f;
            if (levelStorageScript.obtainedStarCnt[realLevel, stage] != 4)
            {
                levelStorageScript.obtainedStarCnt[realLevel, stage] = 3;
            }
        }
        else if (totalCorrectStage == questionMaxIndex)
        {
            // 별 1개 채워짐
            onesentenceText.text = sentences[1];
            StarMiddle.fillAmount = 1f;

            levelStorageScript.obtainedStarCnt[realLevel, stage] = 4;
        }
        else
        {
            Debug.Assert(false, "문제가 10개 초과");
        }
        
        SoundManager.Instance.StopMusic();
        
        yield return new WaitForSeconds(1.0f);

        if (totalCorrectStage == questionMaxIndex)
        {
            SoundManager.Instance.Play_Narration("Synthesis", 5);
        }
        else
        {
            SoundManager.Instance.Play_Narration("Synthesis", 7);
        }

        stageStorageScript.LoadGameStageData(EGameName.Synthesis, _totalStorageScript.currId, realLevel, stage);
        stageStorageScript.playCnt++;
        eachQuestionStorageScript.SaveGameOver(EGameName.Synthesis, realLevel, stage, questionId,
            stageStorageScript.playCnt);


        
        stageStorageScript.SaveGameStageData(EGameName.Synthesis, _totalStorageScript.currId, realLevel, stage,
            totalCorrectStage);

        //levelData 계산
        levelStorageScript.avgPerfection[realLevel] =
            stageStorageScript.GetAvgCorrectAnswerCountForLevel(_totalStorageScript.currId, realLevel,
                EGameName.Synthesis);
        levelStorageScript.avgResponseTime[realLevel] =
            stageStorageScript.GetAvgResponseTimeForLevel(_totalStorageScript.currId, realLevel, EGameName.Synthesis);
        levelStorageScript.SaveLevelData(EGameName.Synthesis, _totalStorageScript.currId, realLevel);

        _totalStorageScript.Save(EGameName.Synthesis, realLevel, stage);

        eachQuestionStorageScript.initializeQuestionData();
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
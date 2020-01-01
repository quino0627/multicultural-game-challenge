using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class QuizManager : MonoBehaviour
{
    /*
     * forelink - this is main routine of quiz contents
     */
    private PostScript ps = new PostScript();
    private string[] page = { "Quiz1"};

    private int last_page_index = 10;
    public Text[] StudyText;
    public Image[] StudyImage;
    public Image[] Image;
    private string val_str;

    [HideInInspector]
    public static int startIndex = 0;
    public GameObject UIpage;
    public GameObject CantTouchPage;
    public GameObject StartBtn;
    public Text Quiz5_Answer_txt;

    [HideInInspector]
    public static int word_length;
    [HideInInspector]
    public static int quiz_word_length;
    [HideInInspector]
    public static int day;
    [HideInInspector]
    public static int word_number;
    [HideInInspector]
    public static int word_count;
    [HideInInspector]
    public static int sentence_count = 0;
    [HideInInspector]
    public static bool[] trueorfalse; //= new bool[7];//5개의 퀴즈 사이즈
    [HideInInspector]
    public static string[] StudyText1_Spilt;

    static bool isfirst = true;

    public static int sentence_size = 5;//데이당 몇문제로 할지
    public static int sentence_lastnumber;//학습 재학습시 마지막 문제의 index를 정하는 변수
    public static bool isReStudy = false;//학습중인지 재학습 중인지 여부 -> index가 달라짐
    public static ArrayList ic_number = new ArrayList();
    private ArrayList temp_icnumber = new ArrayList();

    public Image[] Quiz1Toggle;

    //excel data
    public Entity_sort list;

    public GameObject[] LastPopup;
    private int rotate_number = 1;
    public static bool prev_or_next = true;

    public static string parameter_id = "";

    public static int[] page_ModifyCount; // count 초기화 되기 전
    public static bool[] page_isAnswer; // check 초기화 되기 전
    public static int[] page_currentScore; // current Score 초기화 되기 전
    

    public static int pageNum = 0;

    void Awake()
    {
        //Debug.Log(list.sheets[0].list[1].ex1);

        if (ps == null)
            ps = GameObject.FindObjectOfType<PostScript>();

        /* 
         * forelink replace below code - previous code truncate user_id after position 6
         */
        string urlParameter = Application.absoluteURL;
        string userId = null;
        string classId = "";
        int day = 0;

        string mediaUrl = null; // parameter added by forelink
        string gotoUrl = null; // parameter added by forelink

        if (urlParameter == null || urlParameter == "") // forelink - for pc standalone running
        {
            urlParameter = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/unity1.1/quiz/index.html?id=kukpadmin&class_id=elem2&day=1&media=https://kukp.forelink-cloud.co.kr/moodle3/kukp/media_elem2";
            
            Debug.Log("QuizManager: temporary url = " + urlParameter);
        }

        int index = urlParameter.IndexOf("?");

        if (index != -1)
        {
            string[] queryParameters = urlParameter.Substring(index + 1).Split('&');

            for (int i = 0; i < queryParameters.Length; i++)
            {
                if (queryParameters[i].StartsWith("id="))
                {
                    userId = queryParameters[i].Substring(3);
                }
                else if (queryParameters[i].StartsWith("class_id="))
                {
                    classId = queryParameters[i].Substring(9);
                }
                else if (queryParameters[i].StartsWith("day="))
                {
                    day = Int32.Parse(queryParameters[i].Substring(4));
                }
                else if (queryParameters[i].StartsWith("media="))
                {
                    mediaUrl = queryParameters[i].Substring(6);
                }
                else if (queryParameters[i].StartsWith("goto="))
                {
                    gotoUrl = queryParameters[i].Substring(5);
                }
            }

            if (mediaUrl != null) // '!', '|' is used to specify url query part
            {
                mediaUrl = mediaUrl.Replace("!", "?");
                mediaUrl = mediaUrl.Replace("|", "&");
            }
            if (gotoUrl != null) // '!', '|' is used to specify url query part
            {
                gotoUrl = gotoUrl.Replace("!", "?");
                gotoUrl = gotoUrl.Replace("|", "&");
            }
        }
        if (mediaUrl == null)
        {
            mediaUrl = "";
        }
        if (gotoUrl == null)
        {
            gotoUrl = "";
        }
        // Debug.Log("QuizManager: id = " + (userId!=null?userId:"null") + ", day = " + day + " [quiz, "+mediaUrl+", "+gotoUrl+"]");

        if (userId != null && userId != "" && day != 0)
        {
            PlayerPrefs.SetString("user_id", userId);
            Global.userId = userId;
            Global.classId = classId;
            Global.nRound = day;
            Global.type = "quiz";
            Global.mediaUrl = mediaUrl;
            Global.gotoUrl = gotoUrl;

            //sentence_count = 1;
            startIndex = 8;
            //DayBtnClicked(classId, day, userId);
        }
    }

    static void Init()     
    {
        PostScript ps = GameObject.Find("Post").GetComponent<PostScript>();
        Debug.Log("hello");
        Debug.Log(ps.parse_WordID.Count);
        page_isAnswer = new bool[ps.parse_WordID.Count];
        page_ModifyCount = new int[ps.parse_WordID.Count];
        page_currentScore = new int[ps.parse_WordID.Count];
        trueorfalse = new bool[ps.parse_WordID.Count];
        for (int i = 0; i < page_isAnswer.Length; i++)
        {
            page_isAnswer[i] = false;
            page_ModifyCount[i] = 0;
            page_currentScore[i] = 10;
            Debug.Log(page_isAnswer[i]);
            Debug.Log(page_ModifyCount[i]);
            Debug.Log(page_currentScore[i]);
        }
        Debug.Log("In QUIZMANAGERR, INIT()");
        
    }

    public void BtnClickSoundPlay()
    {
        GetComponent<AudioSource>().Play();
    }

    public void DayBtnClicked(string classId, int day_value, string m_Id)
    {
        pageNum = 0;

        Debug.Log("DayBtnClicked Start");
        ps = GameObject.FindObjectOfType<PostScript>();
        parameter_id = m_Id;
        //ps.UserInfoParse(parameter_id);

        if (ps.parse_WordID.Count <= 0) // 0 <- 4, forelink change condition
        {
            ps.WordIDParse(classId, day_value, parameter_id);
        }

        gameObject.transform.Find("Day_Choice").Find("Paper").gameObject.SetActive(false);
        gameObject.transform.Find("Day_Choice").Find("StudyStart").gameObject.SetActive(true);

        StartCoroutine(ParseTimer(0.3f, classId, day_value));

        Debug.Log("DayBtnClicked End");
    }

    public void NextBtnClicked()
    {
        string classId = Global.classId;

        pageNum++;

        Debug.Log("NextBtnClicked");
        CantTouchPage.SetActive(true);

        Transform tempTransform = gameObject.transform;

        StopCoroutine(TimerScript.StopWatchStart());
        int clickTime = (int)TimerScript.sw.ElapsedMilliseconds;
        TimerScript.sw.Stop();

        ps = GameObject.FindObjectOfType<PostScript>();
        int maxPageNum = ps.quizJson["data"][0].Count - 1;

        if (pageNum < maxPageNum) // maxPageNum is empty
        {
            string[] tempString_AA = new string[] { };
            tempString_AA = ps.quizJson["data"][0][pageNum]["quiz_id"].ToString().Split(';');
            templistTimer[Int32.Parse(tempString_AA[1].Remove(1)) - 1] += clickTime;
            AnswerCheck(2); // check sentence changed and send result. set next sentence answer to default value
        }

        TimerScript.sw.Reset();

        if (pageNum == maxPageNum)
        {
            CantTouchPage.SetActive(false);

            tempTransform.parent.Find("PopUpPage").Find("panel").Find("Text").gameObject.GetComponent<Text>().text = "퀴즈 종료";
            tempTransform.parent.Find("PopUpPage").gameObject.SetActive(true);

            ps.QuizInfoInit(templistTimer, templistAnswer, classId, Int32.Parse(tempWordID), day); // send reset to server
        }
        else
        {
            for (int i = 0; i < page.Length; i++)
            {
                if (tempTransform.Find(page[i]) != null) {
                    tempTransform.Find(page[i]).gameObject.SetActive(false);
                }
            }
            //tempTransform.Find(page[int.Parse(ps.quizJson["data"][0][pageNum]["quiz_type"].Value) + 7]).gameObject.SetActive(true);
            //tempTransform.Find(page[int.Parse(ps.quizJson["data"][0][pageNum]["quiz_type"].Value) + 7]).GetComponent<RandomQuizScript>().QuizInit();
            tempTransform.Find(page[pageNum%5]).gameObject.SetActive(true);
            tempTransform.Find(page[pageNum%5]).GetComponent<RandomQuizScript>().QuizInit();
        }


        SlotScript.moveCount_last = 0;
        SlotScript.moveCount = 0;

        CantTouchPage.SetActive(false);
    }

    public void PrevBtnClicked()
    {
        string classId = Global.classId;

        pageNum--;

        CantTouchPage.SetActive(true);

        Transform tempTransform = gameObject.transform;

        StopCoroutine(TimerScript.StopWatchStart());
        int clickTime = (int)TimerScript.sw.ElapsedMilliseconds;
        TimerScript.sw.Stop();

        string[] tempString_AA = new string[] { };
        tempString_AA = ps.quizJson["data"][0][pageNum]["quiz_id"].ToString().Split(';');
        if ((pageNum - wordChangeCount) >= 0)
        {
            templistTimer[Int32.Parse(tempString_AA[1].Remove(1)) - 1] += clickTime;
        }
        else
        {
            pageNum++;
            TimerScript.sw.Reset();

            SlotScript.moveCount_last = 0;
            SlotScript.moveCount = 0;

            CantTouchPage.SetActive(false);
            return;
        }

        ps = GameObject.FindObjectOfType<PostScript>();
        switch (startIndex)//인덱스에 해당하는 DB에 가서 그 값을 저장한다
        {
            case 8:
                StartCoroutine(AddAnsweTimer(0));
                val_str = sentence_count.ToString() + ",DayTimerTable,Quiz1," + clickTime.ToString() + "," + rotate_number.ToString();
                ps.RenumPostInit(val_str);
                break;
            case 9:
                StartCoroutine(AddAnsweTimer(1));
                val_str = sentence_count.ToString() + ",DayTimerTable,Quiz2," + clickTime.ToString() + "," + rotate_number.ToString();
                ps.RenumPostInit(val_str);
                break;
            case 10:
                StartCoroutine(AddAnsweTimer(2));
                val_str = sentence_count.ToString() + ",DayTimerTable,Quiz3," + clickTime.ToString() + "," + rotate_number.ToString();
                ps.RenumPostInit(val_str);
                break;
            case 11:
                val_str = sentence_count.ToString() + ",DayTimerTable,Quiz4," + clickTime.ToString() + "," + rotate_number.ToString();
                ps.RenumPostInit(val_str);
                break;
            default:
                break;
        }
        TimerScript.sw.Reset();

        if (pageNum >= 0)
        {
            for (int i = 0; i < page.Length; i++)
            {
                // Debug.Log(page[i]);
                if (tempTransform.Find(page[i]) != null)
                    tempTransform.Find(page[i]).gameObject.SetActive(false);
            }

            // Debug.Log(ps.quizJson["data"][0][pageNum]["quiz_type"].Value);

            //tempTransform.Find(page[int.Parse(ps.quizJson["data"][0][pageNum]["quiz_type"].Value) + 7]).gameObject.SetActive(true);
            //tempTransform.Find(page[int.Parse(ps.quizJson["data"][0][pageNum]["quiz_type"].Value) + 7]).GetComponent<RandomQuizScript>().QuizInit();
            tempTransform.Find(page[pageNum % 5]).gameObject.SetActive(true);
            tempTransform.Find(page[pageNum % 5]).GetComponent<RandomQuizScript>().QuizInit();
        }
        else
            pageNum = 0;

        SlotScript.moveCount_last = 0;
        SlotScript.moveCount = 0;

        CantTouchPage.SetActive(false);
    }

    public void HomeBtnClicked()
    {
        startIndex = 0;
        word_count = 1;
        rotate_number = 1;
        sentence_lastnumber = sentence_size;
        isReStudy = false;
        RandomQuizScript.toggleList.Clear();
        RandomQuizScript.cp_last_index = 21;//초기화
        RandomQuizScript.cp_count = 1;
        SceneManager.LoadScene("00.Home");
    }

    IEnumerator ParseTimer(float m_Timer, string classId, int day_value)
    {
        yield return new WaitForSeconds(m_Timer);

        while (!ps.isParseDone)
        {
            if (ps.isParseDone)
                break;
            else
                yield return null;
        }

        sentence_size = ps.parse_WordID.Count;
        sentence_lastnumber = sentence_size;

        day = day_value;

        //sentence_count = Int32.Parse(ps.parse_WordID[word_count]);
        sentence_count = Int32.Parse(ps.parse_WordID[word_count]);


        Debug.Log("ParseTimer QuizInit(" + classId + ", " + day + ", " + sentence_count.ToString() + ", " + parameter_id + ")");
        //ps.ParseInit(classId, day.ToString(), sentence_count.ToString(), parameter_id);
        ps.QuizInit(classId, day.ToString(), sentence_count.ToString(), parameter_id);
        

        //NextBtnClicked();
    }

    public void QuitButton()
    {
        string userId = PlayerPrefs.GetString("user_id");
        string classId = Global.classId;
        int nRound = Global.nRound;
        string type = Global.type; // added parameter by forelink
        string gotoUrl = Global.gotoUrl; // added parameter by forelink

        WordSentenceQuizScript.isInit = false;

        if (Application.platform != RuntimePlatform.WebGLPlayer) { // forelink insert
            Application.Quit();
        }
        else if (gotoUrl != null && gotoUrl != "") // added parameter by forelink
        {
            Application.OpenURL(gotoUrl);
        }
        else
        {
            Application.OpenURL("https://kukp.forelink-cloud.co.kr/moodle3/day.php?ver=1.1&id="+userId+"&class_id="+classId+"&day="+nRound+"&type="+type);
        }
    }

    IEnumerator AddAnsweTimer(int m_Count)
    {
        yield return new WaitForSecondsRealtime(0.1f);
    }

    public void GoStageSelectBtn()
    {
        StartCoroutine(PlayStartBtn());
        
    }

    IEnumerator PlayStartBtn()
    {
        StartBtn.GetComponent<AudioSource>().loop = false;
        StartBtn.GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(StartBtn.GetComponent<AudioSource>().clip.length);
        transform.GetChild(0).gameObject.SetActive(false);

        Transform tempTransform = GameObject.Find("BackgroundPage").transform;
        tempTransform.Find("Stage_Choice").gameObject.SetActive(true);
    }

    public void QuizStartBtn()
    {
        // forelink remove condition // if (ps.tempAudioClipList.Count > 5)
        {
            transform.GetChild(0).gameObject.SetActive(false);

            Transform tempTransform = GameObject.Find("BackgroundPage").transform;

            //String quizType = "Quiz" + ps.quizJson["data"][0][pageNum]["quiz_type"].Value;
            String quizType = page[pageNum];
            tempTransform.Find(quizType).gameObject.SetActive(true);
            //tempTransform.Find(quizType).GetComponent<RandomQuizScript>().QuizInit();
            //Debug.Log(GameObject.Find("Image1"));
            //tempTransform.Find(page[int.Parse(ps.quizJson["data"][0][pageNum]["quiz_type"].Value) + 7]).gameObject.SetActive(true);
            //tempTransform.Find(page[int.Parse(ps.quizJson["data"][0][pageNum]["quiz_type"].Value) + 7]).GetComponent<RandomQuizScript>().QuizInit();

            WordSentenceQuizScript.Init();
            Init();

            InitAnswer(); // forelink insert for fixed size

            UIpage.SetActive(true);
        }
    }

    public string tempWordID = ""; // current sentence
    public List<float> templistTimer = new List<float>();
    public List<int> templistAnswer = new List<int>();
    int wordChangeCount = 0;       // currcnt sentence start pagenum

    public void InitAnswer()  // forelink insert
    {
        templistAnswer.Clear();
        templistTimer.Clear();
        for (int i = 0; i < 7; i++)
        {
            templistAnswer.Add(3); // 0:NOK, 1:OK, 2:SKIP, 3:NT(not tested)
            templistTimer.Add(0);
        }
    }

    public void AnswerCheck(int m_Answer)
    {
        string classId = Global.classId;

        if (tempWordID != ps.parse_WordID[pageNum]) // forelink, pageNum is current quiz, wordid.count is total quiz count
        {
            if (tempWordID.Length > 0)
            {
                Debug.Log("AnswerCheck, Sentence Changed '" + tempWordID + "' -> '" + ps.parse_WordID[pageNum] + "'");
                ps.QuizInfoInit(templistTimer, templistAnswer, classId, Int32.Parse(tempWordID), day); // send reset to server
            }
            tempWordID = ps.parse_WordID[pageNum];
            wordChangeCount = pageNum;
            InitAnswer();
        }
        string[] tempString_AA = new string[] { };
        tempString_AA = ps.quizJson["data"][0][pageNum]["quiz_id"].ToString().Split(';');
        int n = Int32.Parse(tempString_AA[1].Remove(1));
        Debug.Log("quiz_id = " + tempWordID + ", " + n + " (" + ps.quizJson["data"][0][pageNum]["quiz_id"] + ")");
        templistAnswer[n - 1] = m_Answer;
        templistTimer[n - 1] = (int)TimerScript.sw.ElapsedMilliseconds;
    }
}
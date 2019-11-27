using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class NextBtnClickedScript : MonoBehaviour
{
    /*
     * forelink - this is main routine of study contents
     */
    private PostScript ps = new PostScript();
    private string[] page = { "Day_Choice", "Study1", "Study2", "Study3", "Study4_1", "Study4_2", "Study4_3", "Study5",
                             "Quiz1", "Quiz2", "Quiz3", "Quiz4", "SentenceLast"};

    private int last_page_index = 10;
    public Text[] StudyText;
    public Image[] StudyImage;
    private string val_str;

    [HideInInspector]
    public static int startIndex = 0;
    public GameObject UIpage;
    public GameObject CantTouchPage;
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
    public static int word_count = 1;
    [HideInInspector]
    public static int sentence_count;
    [HideInInspector]
    public static bool[] trueorfalse = new bool[4];//5개의 퀴즈 사이즈
    [HideInInspector]
    public static string[] StudyText1_Spilt;

    public static int sentence_size = 5;//데이당 몇문제로 할지
    public static int sentence_lastnumber;//학습 재학습시 마지막 문제의 index를 정하는 변수
    public static bool isReStudy = false;//학습중인지 재학습 중인지 여부 -> index가 달라짐
    public static ArrayList ic_number = new ArrayList();
    private ArrayList temp_icnumber = new ArrayList();

    public Image[] Quiz1Toggle;
    public Image[] Quiz2Toggle;
    public Image[] Quiz4Toggle;

    public GameObject[] LastPopup;
    private int rotate_number = 1;
    public static bool prev_or_next = true;

    public static string parameter_id = "";

    List<float> sentenceTimer = new List<float>();
    

    void Awake()
    {
        if (ps == null)
            ps = GameObject.FindObjectOfType<PostScript>();

        /* 
         * forelink replace below code - previous code truncate user_id after position 6
         */
        string urlParameter = Application.absoluteURL;
        string userId = null;
        string classId = "";
        int day = 0;
        string type = null;
        string mediaUrl = null; // parameter added by forelink
        string gotoUrl = null; // parameter added by forelink
        if (SceneManager.GetActiveScene().name == "01_1.Study")
        {
            type = "study";
        }
        else
        {
            type = "quiz";
        }
        if (urlParameter == null || urlParameter == "") // forelink - for pc standalone running
        {
            //lParameter = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/unity1.1/" + type + "/index.html?id=kukpadmin&class_id=math2p&day=1&media=https://kukp.forelink-cloud.co.kr/moodle3/kukp/media_math2p";
            urlParameter = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/unity1.1/" + type + "/index.html?id=kukpadmin&class_id=elem2p&day=1&media=https://kukp.forelink-cloud.co.kr/moodle3/kukp/media_elem2";
            // "&media=url1!q1=A|q2=a|q3=1&goto=url2!q4=B|q5=b|q6=2"
            Debug.Log("NextBtnClicked: temporary url = " + urlParameter);
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
        // Debug.Log("NextBtnClicked: id = " + (userId!=null?userId:"null") + ", day = " + day + " ["+type+", "+mediaUrl+", "+gotoUrl+"]");

        if (userId != null && userId != "" && day != 0)
        {
            PlayerPrefs.SetString("user_id", userId);
            Global.userId = userId;
            Global.classId = classId;
            Global.nRound = day;
            Global.type = type;
            Global.mediaUrl = mediaUrl;
            Global.gotoUrl = gotoUrl;
            if (SceneManager.GetActiveScene().name == "01_1.Study")
            {
                DayBtnClicked(classId, day, userId);
            }
            if (SceneManager.GetActiveScene().name == "01_2.Quiz")
            {
                sentence_count = 1;
                startIndex = 8;
                DayBtnClicked(classId, day, userId);
            }
        }
    }

    public void BtnClickSoundPlay()
    {
        GetComponent<AudioSource>().Play();
    }

    public void DayBtnClicked(string classId, int day_value, string m_Id)
    {
        Debug.Log("DayBtnClicked()");
        ps = GameObject.FindObjectOfType<PostScript>();
        parameter_id = m_Id;
        //ps.UserInfoParse(parameter_id);

        ps.WordIDParse(classId, day_value, parameter_id);

        gameObject.transform.Find("Day_Choice").Find("Paper").gameObject.SetActive(false);
        gameObject.transform.Find("Day_Choice").Find("StudyStart").gameObject.SetActive(true);

        StartCoroutine(ParseTimer(0.3f, classId, day_value));
    }

    public void StudyStartBtnClicked()
    {
        Debug.Log("StudyStartBtnClicked = " + ps.parse_Attribute[0] + " [" + ps.parse_Attribute[1] + "] " + ps.parse_Attribute[2]+", "+ps.parse_Attribute[3]);
        word_length = ps.parse_Attribute[2].Length;
        if (word_length > 0)
        {
            NextBtnClicked();
        }
    }

    public void PopUpOKbtnClicked()
    {
        Debug.Log("PopUpOKbtnClicked()");
        gameObject.transform.parent.Find("PopUpPage").gameObject.SetActive(false);
        if (startIndex == 1)
        {
            gameObject.transform.Find(page[startIndex]).gameObject.SetActive(false);
            startIndex += 2;
            gameObject.transform.Find(page[startIndex - 1]).gameObject.SetActive(true);
            gameObject.transform.Find(page[startIndex]).gameObject.SetActive(true);

            if (gameObject.transform.Find(page[startIndex]).Find("CurrentText") != null)
                gameObject.transform.Find(page[startIndex]).Find("CurrentText").GetComponent<Text>().text = StudyText[1].text;
        }
        else
        {
            if (startIndex != 7)
            {
                gameObject.transform.Find(page[startIndex]).gameObject.SetActive(false);
                gameObject.transform.Find(page[startIndex + 1]).gameObject.SetActive(true);
                startIndex++;
            }
            else
            {
                string userId = PlayerPrefs.GetString("user_id");
                string classId = Global.classId;
                int nRound = Global.nRound;
                string type = Global.type; // added parameter by forelink
                string gotoUrl = Global.gotoUrl; // added parameter by forelink
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
        }
    }

    public void NextBtnClicked()
    {
        Debug.Log("NextBtnClicked()");

        string classId = Global.classId;

        if (word_length <= 0)
        {
            Debug.Log("NextBtnClicked() word_length is zero, return");
            return;
        }

        CantTouchPage.SetActive(true);

        if (prev_or_next == false && startIndex == 2) startIndex++;
        prev_or_next = true;

        Transform tempTransform = gameObject.transform;
        StopCoroutine(TimerScript.StopWatchStart());
        int clickTime = (int)TimerScript.sw.ElapsedMilliseconds;
        TimerScript.sw.Stop();

        sentenceTimer.Add(clickTime);

        ps = GameObject.FindObjectOfType<PostScript>();

        /*
         * forelink - startIndex transition on study contents
         *  (1 -> Popup -> 3 -> Popup -> 4 -> 5 -> 6) x 2 -> 7
         *  1: study1 - sentence intro 
         *  3: study3 - word1 meaning (Study3*.wav)
         *  4: study4 - word1 usage 1 (Study4*.wav)
         *  5: study5 - word1 usage 2
         *  6: study6 - word1 usage 3
         * forelink - startIndex transition on quiz contents
         *  8 -> 9 -> 10 -> 11 -> 12 -> 13 -> 14 -> 8 -> 9 -> 10 -> 11 -> 12 -> 13 -> 14
         *  8: quiz1 - meaning of word1
         *  9: quiz2 - sound of word1
         * 10: quiz3 - select correct word1 for blank in sentence
         * 11: quiz4 - meaning of word2
         * 12: quiz5 - sound of word2
         * 13: quiz6 - select correct word2 for blank in sentence
         * 14: quiz7 - ordering words for sentence 
         */
        switch (startIndex)//인덱스에 해당하는 DB에 가서 그 값을 저장한다
        {
            case 1:
                if (word_count == 1)//단어가 여러개일때 다시 여기로 오면서 타임 값이 0이 되므로 word_count가 1일때 한번만 저장시키기로 한다
                {
                    val_str = sentence_count.ToString() + ",DayTimerTable,Study1," + clickTime.ToString() + "," + rotate_number.ToString();
                    //Debug.Log("val_str = " + val_str);
                    ps.RenumPostInit(val_str);
                }
                break;
            case 3:
                val_str = sentence_count.ToString() + ",DayTimerTable,Study3_" + word_count.ToString() + "," + clickTime.ToString() + "," + rotate_number.ToString();
                ps.RenumPostInit(val_str);
                break;
            case 4:
                val_str = sentence_count.ToString() + ",DayTimerTable,Study4_" + word_count.ToString() + "," + clickTime.ToString() + "," + rotate_number.ToString();
                ps.RenumPostInit(val_str);
                break;
            case 7:
                val_str = sentence_count.ToString() + ",DayTimerTable,Study5," + clickTime.ToString() + "," + rotate_number.ToString();
                ps.RenumPostInit(val_str);
                break;
            default:
                break;
        }

        TimerScript.sw.Reset();

        if ((startIndex == (ps.parse_Attribute[7 + 2 * (word_count - 1)].Split(';').Length + 2)) && (word_count < word_number))
        {
            tempTransform.Find(page[startIndex]).gameObject.SetActive(false);
            startIndex = 1;
            word_count++;
            StudyText[1].text = ps.parse_Attribute[2 + word_count - 1];
            StudyText[2].text = ps.parse_Attribute[6 + 2 * (word_count - 1)];
            StudyText[3].text = ps.parse_Attribute[7 + 2 * (word_count - 1)].Split(';')[0];
            StudyText[4].text = ps.parse_Attribute[7 + 2 * (word_count - 1)].Split(';')[1];
            StudyText[5].text = ps.parse_Attribute[7 + 2 * (word_count - 1)].Split(';')[2];
            for (int i = 0; i < ps.tempSpriteList.Count; i++)
            {
                if (sentence_count + "_2_" + word_count.ToString() == ps.tempSpriteList[i].name)
                {
                    Debug.Log("sentence_count + _2_ + word_count.ToString() = " + sentence_count + "_2_" + word_count.ToString());
                    Debug.Log("ps.tempSpriteList[i].name = " + ps.tempSpriteList[i].name);
                    StudyImage[1].sprite = ps.tempSpriteList[i];
                    StudyImage[2].sprite = ps.tempSpriteList[i];
                    StudyImage[3].sprite = ps.tempSpriteList[i];
                    StudyImage[4].sprite = ps.tempSpriteList[i];
                }
            }
            tempTransform.Find(page[startIndex]).gameObject.SetActive(true);
            word_length = StudyText[1].text.Length;
            NextBtnClicked();//Study1이 화면에 보이지 않고 바로 넘어가게 끔
        }
        else if (startIndex == 2)
        {
            tempTransform.Find(page[startIndex - 1]).gameObject.SetActive(false);
            tempTransform.Find(page[startIndex]).gameObject.SetActive(true);
            tempTransform.Find(page[startIndex + 1]).gameObject.SetActive(true);
            startIndex++;
        }
        else if (startIndex == 0 || startIndex == 4 || startIndex == 5 || startIndex == 6 || startIndex == 8 || startIndex == 9)
        {
            while (!ps.isParseDone)
            {
                if (ps.isParseDone)
                    break;
            }
            if (tempTransform.Find(page[startIndex + 1]).Find("CurrentText") != null)
                tempTransform.Find(page[startIndex + 1]).Find("CurrentText").GetComponent<Text>().text = StudyText[1].text;
            tempTransform.Find(page[startIndex]).gameObject.SetActive(false);
            tempTransform.Find(page[startIndex + 1]).gameObject.SetActive(true);
            startIndex++;
        }
        else if (startIndex == 1 || startIndex == 3 || startIndex == 7)
        {
            if (startIndex == 1)
            {
                tempTransform.Find(page[startIndex]).gameObject.GetComponent<AudioSource>().clip = null;
                tempTransform.parent.Find("PopUpPage").Find("panel").Find("Text").gameObject.GetComponent<Text>().text = "'" + StudyText[1].text + "'" + " 단어를 배워봐요!";
                tempTransform.parent.Find("PopUpPage").gameObject.GetComponent<AudioSource>().clip = Resources.Load("Sound/PopUpSound/word_study_start") as AudioClip;
                if (tempTransform.Find(page[startIndex + 1]).Find("CurrentText") != null)
                {
                    tempTransform.Find(page[startIndex + 1]).Find("CurrentText").GetComponent<Text>().text = StudyText[1].text;
                }
            }
            else if (startIndex == 3)
            {
                tempTransform.Find(page[startIndex]).gameObject.GetComponent<AudioSource>().clip = null;
                tempTransform.parent.Find("PopUpPage").Find("panel").Find("Text").gameObject.GetComponent<Text>().text = "'" + StudyText[1].text + "'" + " 단어는 어떻게 사용될까요?";
                tempTransform.parent.Find("PopUpPage").gameObject.GetComponent<AudioSource>().clip = Resources.Load("Sound/PopUpSound/word_use") as AudioClip;
                if (tempTransform.Find(page[startIndex + 1]).Find("CurrentText") != null)
                {
                    tempTransform.Find(page[startIndex + 1]).Find("CurrentText").GetComponent<Text>().text = StudyText[1].text;
                }
            }
            else if (startIndex == 7)
            {
                CantTouchPage.SetActive(false);
                if (sentence_count % sentence_lastnumber != 0)
                {
                    NextSentenceBtnClicked();
                }
                else
                {
                    ps.StudyInfoInit(sentenceTimer, classId, sentence_count);
                    tempTransform.parent.Find("PopUpPage").Find("panel").Find("Text").gameObject.GetComponent<Text>().text = "학습 종료";
                    tempTransform.parent.Find("PopUpPage").gameObject.SetActive(true);
                    tempTransform.parent.Find("PopUpPage").GetComponent<AudioSource>().Stop();
                }
                return;
            }
            tempTransform.parent.Find("PopUpPage").gameObject.SetActive(true);
        }
        if (startIndex == 0 || startIndex == 12)
        {
            if (startIndex == 12) tempTransform.Find(page[11]).gameObject.SetActive(false);

            UIpage.SetActive(false);
            for (int i = 0; i < 6; i++)
            {
                if (i == 0 || i == 1 || i == 2) Quiz1Toggle[i].gameObject.transform.parent.parent.Find("Label").GetComponent<Text>().color = new Color(0.24f, 0.22f, 0.22f);
                Quiz1Toggle[i].color = new Color(Quiz1Toggle[i].color.r, Quiz1Toggle[i].color.g, Quiz1Toggle[i].color.b, 0f);
            }
            tempTransform.Find("Quiz1").Find("WordQuiz").gameObject.SetActive(false);
            tempTransform.Find("Quiz1").Find("SentenceQuiz").gameObject.SetActive(false);

            for (int i = 0; i < 6; i++) Quiz2Toggle[i].color = new Color(Quiz2Toggle[i].color.r, Quiz2Toggle[i].color.g, Quiz2Toggle[i].color.b, 0f);
            tempTransform.Find("Quiz2").Find("WordQuiz").gameObject.SetActive(false);
            tempTransform.Find("Quiz2").Find("SoundQuiz").gameObject.SetActive(false);

            tempTransform.Find("Quiz3").Find("Size3").gameObject.SetActive(false);
            tempTransform.Find("Quiz3").Find("Size4").gameObject.SetActive(false);
            tempTransform.Find("Quiz3").Find("Size5").gameObject.SetActive(false);

            for (int i = 0; i < 3; i++) Quiz4Toggle[i].color = new Color(Quiz4Toggle[i].color.r, Quiz4Toggle[i].color.g, Quiz4Toggle[i].color.b, 0f);

            RandomQuizScript.toggleList.Clear();
            RandomQuizScript.cp_last_index = 21;//초기화
            RandomQuizScript.cp_count = 1;
        }
        else
        {
            UIpage.SetActive(true);
        }

        SlotScript.moveCount = 0;
        CantTouchPage.SetActive(false);
    }

    public void NextSentenceBtnClicked()
    {
        string classId = Global.classId;

        CantTouchPage.SetActive(true);
        val_str = sentence_count.ToString() + ",PerfectLearningTable,RotNumber," + rotate_number.ToString();
        ps.PostInit(val_str);

        startIndex = 0;
        word_count = 1;

        if (sentence_count % sentence_lastnumber != 0)//마지막 센텐스가 아닐떄
        {
            StartCoroutine(TextParseTimer2(0.3f, classId));
        }
        else//마지막 센텐스 일때 여기로 온 것은 틀린 문항이 있을 경우
        {
            StartCoroutine(TextParseTimer3(0.3f, classId));
        }

        CantTouchPage.SetActive(false);
    }

    public void NextDayBtnClicked()
    {
        CantTouchPage.SetActive(true);

        val_str = sentence_count.ToString() + ",PerfectLearningTable,RotNumber," + rotate_number.ToString();
        ps.PostInit(val_str);

        for (int i = 0; i < LastPopup.Length; i++)
        {
            LastPopup[i].SetActive(false);
        }

        gameObject.transform.Find(page[startIndex]).gameObject.SetActive(false);
        gameObject.transform.Find(page[startIndex - 1]).gameObject.SetActive(false);

        startIndex = 0;
        word_count = 1;
        rotate_number = 1;
        gameObject.transform.Find(page[startIndex]).gameObject.SetActive(true);

        CantTouchPage.SetActive(false);
    }

    public void PrevBtnClicked()
    {
        if (startIndex != 1 && startIndex != 8)
        {
            CantTouchPage.SetActive(true);
            prev_or_next = false;

            Transform tempTransform = gameObject.transform;
            StopCoroutine(TimerScript.StopWatchStart());

            startIndex--;
            if (((startIndex == 2) && (word_count > 1)) || ((startIndex == 1) && (word_count > 1) && gameObject.transform.Find("Study3").gameObject.activeSelf))
            {
                tempTransform.Find(page[startIndex + 1]).gameObject.SetActive(false);
                startIndex = 6;
                word_count--;
                StudyText[1].text = ps.parse_Attribute[2 + word_count - 1];
                StudyText[2].text = ps.parse_Attribute[6 + 2 * (word_count - 1)];
                StudyText[3].text = ps.parse_Attribute[7 + 2 * (word_count - 1)].Split(';')[0];
                StudyText[4].text = ps.parse_Attribute[7 + 2 * (word_count - 1)].Split(';')[1];
                StudyText[5].text = ps.parse_Attribute[7 + 2 * (word_count - 1)].Split(';')[2];


                for (int i = 0; i < ps.tempSpriteList.Count; i++)
                {
                    if (sentence_count + "_1" == ps.tempSpriteList[i].name)
                    {
                        StudyImage[0].sprite = ps.tempSpriteList[i];
                        StudyImage[5].sprite = ps.tempSpriteList[i];
                        StudyImage[1].sprite = ps.tempSpriteList[i];
                        StudyImage[2].sprite = ps.tempSpriteList[i];
                        StudyImage[3].sprite = ps.tempSpriteList[i];
                        StudyImage[4].sprite = ps.tempSpriteList[i];
                    }
                }
                tempTransform.Find(page[startIndex]).gameObject.SetActive(true);
                word_length = StudyText[1].text.Length;
            }

            else if (startIndex == 1)
            {
                tempTransform.Find(page[startIndex + 2]).gameObject.SetActive(false);
                tempTransform.Find(page[startIndex]).gameObject.SetActive(true);
            }
            else if (startIndex == 2)
            {
                tempTransform.Find(page[startIndex + 1]).gameObject.SetActive(false);
                tempTransform.Find(page[startIndex - 1]).gameObject.SetActive(true);
                startIndex--;
            }
            else if (startIndex == 3)
            {
                tempTransform.Find(page[startIndex + 1]).gameObject.SetActive(false);
                startIndex--;
                tempTransform.Find(page[startIndex + 1]).gameObject.SetActive(true);
                tempTransform.Find(page[startIndex]).gameObject.SetActive(true);
            }
            else
            {
                tempTransform.Find(page[startIndex + 1]).gameObject.SetActive(false);
                tempTransform.Find(page[startIndex]).gameObject.SetActive(true);
            }
            SlotScript.moveCount = 0;
        }
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

    IEnumerator ParseTimer(float m_Timer, string m_classId, int day_value)
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
        sentence_count = Int32.Parse(ps.parse_WordID[0]);
        ps.ParseInit(m_classId, day.ToString(), sentence_count.ToString(), parameter_id);
        // forelink block - QuizManager have it's own ParseTimer()
        // Debug.Log("ParseTimer QuizInit(" + m_classId + ", " + day + ", " + sentence_count.ToString() + ", " + parameter_id + ")");
        // ps.QuizInit(m_classId, day.ToString(), sentence_count.ToString(), parameter_id);

        StartCoroutine(TextParseTimer(0.1f));
    }

    IEnumerator TextParseTimer(float timer)
    {
        yield return new WaitForSeconds(timer);

        while (!ps.isParseDone)
        {
            if (ps.isParseDone)
                break;
            else
                yield return null;
        }

        word_number = Int32.Parse(ps.parse_Attribute[1]);
        //sentence_lastnumber = Int32.Parse(ps.parse_Attribute[1]);
        StudyText[0].text = ps.parse_Attribute[5];
        AudioPlayerScript.cutSoundLastIndex = StudyText[0].text.Split(' ').Length;
        StudyText1_Spilt = new string[StudyText[0].text.Split(' ').Length];
        StudyText1_Spilt = StudyText[0].text.Split(' ');
        Debug.Log(StudyText[0].text);
        StudyText[1].text = ps.parse_Attribute[2];
        StudyText[2].text = ps.parse_Attribute[6];

        StudyText[3].text = ps.parse_Attribute[7].Split(';')[0];
        StudyText[4].text = ps.parse_Attribute[7].Split(';')[1];
        StudyText[5].text = ps.parse_Attribute[7].Split(';')[2];

        StudyText[6].text = ps.parse_Attribute[5];

        while (!ps.isDownloadDone)
        {
            if (ps.isDownloadDone)
                break;
            else
                yield return null;
        }

        for (int i = 0; i < ps.tempSpriteList.Count; i++)
        {
            if (sentence_count + "_1" == ps.tempSpriteList[i].name)
            {
                StudyImage[0].sprite = ps.tempSpriteList[i];
                StudyImage[5].sprite = ps.tempSpriteList[i];
                StudyImage[1].sprite = ps.tempSpriteList[i];
                StudyImage[2].sprite = ps.tempSpriteList[i];
                StudyImage[3].sprite = ps.tempSpriteList[i];
                StudyImage[4].sprite = ps.tempSpriteList[i];
            }

            //if (sentence_count + "_2_" + word_count.ToString() == ps.tempSpriteList[i].name)
            //{
            //    StudyImage[1].sprite = ps.tempSpriteList[i];
            //    StudyImage[2].sprite = ps.tempSpriteList[i];
            //    StudyImage[3].sprite = ps.tempSpriteList[i];
            //    StudyImage[4].sprite = ps.tempSpriteList[i];
            //}
        }
        word_length = StudyText[2].text.Length;//글자수 길이를 받아와서 그에 맞는 배경을 사용하려고
    }

    IEnumerator TextParseTimer2(float timer, string m_classId)
    {
        ps.StudyInfoInit(sentenceTimer,m_classId,sentence_count);

        if (isReStudy == false) sentence_count++;
        else sentence_count = Convert.ToInt32(temp_icnumber[0]);

        ps.ParseInit(m_classId, day.ToString(), sentence_count.ToString(), parameter_id);
        // Debug.Log("TextParseTimer2 QuizInit(" + m_classId + ", " + day + ", " + sentence_count.ToString() + ", " + parameter_id + ")");
        // ps.QuizInit(m_classId, day.ToString(), sentence_count.ToString(),parameter_id);

        yield return new WaitForSeconds(timer);

        //for (int i = 0; i < LastPopup.Length; i++)//팝업창 3개로 된거 끄기
        //{
        //    LastPopup[i].SetActive(false);
        //}

        while (!ps.isParseDone)
        {
            if (ps.isParseDone)
                break;
            else
                yield return null;
        }

        gameObject.transform.Find(page[7]).gameObject.SetActive(false);

        word_number = Int32.Parse(ps.parse_Attribute[1]);
        StudyText[0].text = ps.parse_Attribute[5];
        AudioPlayerScript.cutSoundLastIndex = StudyText[0].text.Split(' ').Length;
        StudyText1_Spilt = new string[StudyText[0].text.Split(' ').Length];
        StudyText1_Spilt = StudyText[0].text.Split(' ');
        Debug.Log(StudyText[0].text);
        StudyText[1].text = ps.parse_Attribute[2];
        StudyText[2].text = ps.parse_Attribute[6];

        StudyText[3].text = ps.parse_Attribute[7].Split(';')[0];
        StudyText[4].text = ps.parse_Attribute[7].Split(';')[1];
        StudyText[5].text = ps.parse_Attribute[7].Split(';')[2];

        StudyText[6].text = ps.parse_Attribute[5];
        word_length = StudyText[1].text.Length;

        if (isReStudy == true) temp_icnumber.RemoveAt(0);

        for (int i = 0; i < ps.tempSpriteList.Count; i++)
        {
            if (sentence_count + "_1" == ps.tempSpriteList[i].name)
            {
                StudyImage[0].sprite = ps.tempSpriteList[i];
                StudyImage[5].sprite = ps.tempSpriteList[i];
                StudyImage[1].sprite = ps.tempSpriteList[i];
                StudyImage[2].sprite = ps.tempSpriteList[i];
                StudyImage[3].sprite = ps.tempSpriteList[i];
                StudyImage[4].sprite = ps.tempSpriteList[i];
            }
        }

        gameObject.transform.Find(page[0]).gameObject.SetActive(true);
        Debug.Log("gameObject.transform.Find(page[startIndex]) = " + gameObject.transform.Find(page[startIndex]).name);
        UIpage.SetActive(true);
    }

    IEnumerator TextParseTimer3(float timer, string m_classId)
    {
        Debug.Log("end");
        yield return null;
        #region 마지막 문장 끝내기는 가만히 있는걸로
        //isReStudy = true;
        //for (int i = 0; i < ic_number.Count; i++)//temp에 그동안 저장된 ic_number를 복사한다
        //{
        //    temp_icnumber.Add(ic_number[i]);
        //}
        //ic_number.Clear();//그리고 ic_number는 클리어 하고 새로 틀린 문항을 Add할 준비를 한다

        //sentence_count = Convert.ToInt32(temp_icnumber[0]);//temp에 담은 첫번째 부터 재학습을 시작한다
        //temp_icnumber.RemoveAt(0);
        //if (temp_icnumber.Count == 0)
        //    sentence_lastnumber = sentence_count;
        //else
        //    sentence_lastnumber = Convert.ToInt32(temp_icnumber[temp_icnumber.Count - 1]);//temp_icnumber 마지막 요소의 값을 lastnumber로 지정
        //ps.ParseInit(day.ToString(), sentence_count.ToString(), "word_number");
        //ps.QuizInit(day.ToString(), sentence_count.ToString());

        //yield return new WaitForSeconds(timer);
        //for (int i = 0; i < LastPopup.Length; i++)//팝업창 3개로 된거 끄기
        //{
        //    LastPopup[i].SetActive(false);
        //}

        ////for (int i = 0; i < RandomQuizScript.word_space.Length; i++)//Quiz5 클론들 삭제하고 정답 null로 초기화 하기
        ////{
        ////    Destroy(RandomQuizScript.word_space[i]);
        ////}
        ////Quiz5_Answer_txt.text = null;
        //gameObject.transform.Find(page[12]).gameObject.SetActive(false);

        //word_number = Int32.Parse(ps.parse_Attribute[1]);
        //StudyText[0].text = ps.[5];
        //AudioPlayerScript.cutSoundLastIndex = StudyText[0].text.Split(' ').Length;
        //StudyText1_Spilt = new string[StudyText[0].text.Split(' ').Length];
        //StudyText1_Spilt = StudyText[0].text.Split(' ');
        //Debug.Log(StudyText[0].text);
        //StudyText[1].text = ps.parse_Attribute[2];
        //StudyText[2].text = ps.parse_Attribute[6];

        //StudyText[3].text = ps.parse_Attribute[7].Split(';')[0];
        //StudyText[4].text = ps.parse_Attribute[7].Split(';')[1];
        //StudyText[5].text = ps.parse_Attribute[7].Split(';')[2];

        //StudyText[6].text = ps.parse_Attribute[5];
        //word_length = StudyText[1].text.Length;

        //rotate_number++;//반복 횟수를 저장하기 위한 변수를 증가 시킨다
        //                //여기다가 Rotation에 저장할 count를 하나씩 증가 시켜준다 그리고 DB에 저장시킴

        //for (int i = 0; i < tp.tempSpriteList.Count; i++)
        //{
        //    if (sentence_count.ToString() + "_1" == tp.tempSpriteList[i].name)
        //    {
        //        StudyImage[0].sprite = tp.tempSpriteList[i];
        //        StudyImage[5].sprite = tp.tempSpriteList[i];
        //    }

        //    if (sentence_count.ToString() + "_2_" + word_count.ToString() == tp.tempSpriteList[i].name)
        //    {
        //        StudyImage[1].sprite = tp.tempSpriteList[i];
        //        StudyImage[2].sprite = tp.tempSpriteList[i];
        //        StudyImage[3].sprite = tp.tempSpriteList[i];
        //        StudyImage[4].sprite = tp.tempSpriteList[i];
        //    }
        //}

        //gameObject.transform.Find(page[startIndex]).gameObject.SetActive(true);
        //UIpage.SetActive(true);
        #endregion
    }
}
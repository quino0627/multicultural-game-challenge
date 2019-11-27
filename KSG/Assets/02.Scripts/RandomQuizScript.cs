using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomQuizScript : MonoBehaviour
{
    private PostScript ps = new PostScript();

    private int quiz1_question_choice;
    private int quiz2_question_choice;

    //excel data
    public Entity_sort list;
    public int level;
    private int Quiz_No = 0;

    public GameObject[] Answers;
    public GameObject Octo;
    public Text Quiz_Level;
    public Text Quiz_Step;
    public GameObject CantTouchPage;

    [HideInInspector]
    public static int quiz1_answer_choice;
    [HideInInspector]
    public static int quiz2_answer_choice;

    [HideInInspector]
    public static int quiz1_1_answer_choice;
    [HideInInspector]
    public static int quiz2_1_answer_choice;

    [HideInInspector]
    public static string[] words;
    [HideInInspector]
    public static string[] quiz3_arr;
    [HideInInspector]
    public static string[] quiz5_answer;
    [HideInInspector]
    public static string[] quiz3_answer;
    [HideInInspector]
    public static string[] quiz4_answer;
    [HideInInspector]
    public static string quiz4_temp_answer;
    [HideInInspector]
    public static string[] quiz3_temp_answer;
    [HideInInspector]
    public static ArrayList toggleList = new ArrayList();

    public GameObject quiz1; //wordQuiz
    //public GameObject quiz2; //sentenceQuiz
    public Text[] Quiz1_1text;
    public Vector3[] Quiz1_1text_vector3 = new Vector3[] { };

    public GameObject word_clone;
    [HideInInspector]
    public static GameObject[] word_space;

    public static Transform[] study_size3_init = new Transform[3];
    public static Transform[] study_size4_init = new Transform[4];
    public static Transform[] study_size5_init = new Transform[5];

    private string val_str;
    public static int cp_count = 1;
    public static int cp_last_index = 21;
    private int platform;
    public static int[] pageOpen = new int[7];
    public bool isQuiz1Ready = false;

    /// <summary>
    /// Fix
    /// </summary>
    Color32 textcolor = new Color32(62, 58, 57, 255);
    public ToggleGroup CheckToggleGroup;
    public Text QuizCount;

    void Awake()
    {
        ps = GameObject.FindObjectOfType<PostScript>();
    }

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android) platform = 1;
        else platform = 2;

        if (gameObject.name == "Quiz1")
            QuizInit();
    }

    /*
    public void Quiz5_AnswerCheckBtnClicked()
    {
        ps = GameObject.FindObjectOfType<PostScript>();
        Quiz5text.text = quiz5_answer[0];
        if (toggleList.Count != quiz5_answer.Length - 1)//처음에 정답 누른 것과 정답의 길이가 다르면 틀린 것이므로 바로 틀림
        {
            NextBtnClickedScript.trueorfalse[4] = false;
            gameObject.transform.parent.parent.Find("UIpage").Find("Ximage").gameObject.SetActive(true);
            //val_str = NextBtnClickedScript.sentence_count.ToString() + ",TestResultTable,q5_result,0";
            ps.PostInit(val_str);
            StartCoroutine(DeleteOXimage());

            for (int i = 0; i > Quiz1_1text.Length; i++) Quiz1_1text[i].transform.localPosition = Quiz1_1text_vector3[i];
        }
        else
        {
            int quiz5_answer_count = 0;
            for (int i = 1; i < quiz5_answer.Length; i++)//정답에서 누른 값들을 빼면서
            {
                for (int j = 0; j < toggleList.Count; j++)
                {
                    if (quiz5_answer[i] == toggleList[j].ToString())
                        quiz5_answer_count++;
                }
            }
            if (quiz5_answer_count == toggleList.Count)//남은게 없으면 둘이 같은 것이므로 정답이고
            {
                NextBtnClickedScript.trueorfalse[4] = true;
                gameObject.transform.parent.parent.Find("UIpage").Find("Oimage").gameObject.SetActive(true);
                //val_str = NextBtnClickedScript.sentence_count.ToString() + ",TestResultTable,q5_result,1";
                ps.PostInit(val_str);
                StartCoroutine(DeleteOXimage());
            }
            else//아니면 틀린 것이다
            {
                gameObject.transform.parent.parent.Find("UIpage").Find("Ximage").gameObject.SetActive(true);
                //val_str = NextBtnClickedScript.sentence_count.ToString() + ",TestResultTable,q5_result,0";
                ps.PostInit(val_str);
                StartCoroutine(DeleteOXimage());
            }
            for (int i = 0; i > Quiz1_1text.Length; i++) Quiz1_1text[i].transform.localPosition = Quiz1_1text_vector3[i];
        }
        gameObject.transform.Find("AnswerBar").gameObject.SetActive(true);
        gameObject.transform.Find("JoystickBar").gameObject.SetActive(false);
    }
    */

    IEnumerator DeleteOXimage()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        gameObject.transform.parent.parent.Find("UIpage").Find("Oimage").gameObject.SetActive(false);
        gameObject.transform.parent.parent.Find("UIpage").Find("Ximage").gameObject.SetActive(false);
    }

    public void LeftCPBtnClicked()
    {
        if (cp_count != 1)
        {
            word_clone.transform.Find("Label" + cp_count.ToString()).Find("CheckPoint").SetParent(word_clone.transform.Find("Label" + (cp_count - 1).ToString()));
            cp_count--;
            word_clone.transform.Find("Label" + cp_count.ToString()).Find("CheckPoint").transform.localPosition = new Vector3(19.6f, 48.2f, 0);
        }
    }

    public void RightCPBtnClicekd()
    {
        if (cp_count != words.Length - 1)
        {
            word_clone.transform.Find("Label" + cp_count.ToString()).Find("CheckPoint").SetParent(word_clone.transform.Find("Label" + (cp_count + 1).ToString()));
            cp_count++;
            word_clone.transform.Find("Label" + cp_count.ToString()).Find("CheckPoint").transform.localPosition = new Vector3(19.6f, 48.2f, 0);
        }
    }

    public void SelectCPBtnClicked()
    {
        if (toggleList.Contains(cp_count.ToString()) == true)//이미 넣으려고 하는 index가 들어있다
        {
            toggleList.Remove(cp_count.ToString());
            if (cp_count < cp_last_index)
            {
                for (int i = cp_count - 1; i < cp_last_index - 1; i++)
                {
                    word_space[i].transform.localPosition = new Vector3(word_space[i].transform.localPosition.x - 15, word_space[i].transform.localPosition.y);
                }
                if (word_space[cp_last_index - 2].transform.localPosition.x <= 375.5f)
                {
                    if (toggleList.Contains(cp_last_index.ToString()))//첫번째쭐 마지막 글자가 띄어쓰기가 되어있으면 바로 옆에 붙이면 안되므로
                    {
                        word_space[cp_last_index - 1].transform.localPosition = new Vector3(word_space[cp_last_index - 2].transform.localPosition.x + 38 + 15, -24f, 0.0f);
                    }
                    else
                    {
                        word_space[cp_last_index - 1].transform.localPosition = new Vector3(word_space[cp_last_index - 2].transform.localPosition.x + 38, -24f, 0.0f);
                    }
                    for (int j = cp_last_index; j < words.Length - 1; j++)
                    {
                        if (toggleList.Contains((cp_last_index + 1).ToString()))
                        {
                            word_space[j].transform.localPosition = new Vector3(word_space[j].transform.localPosition.x - 38 - 15, word_space[j].transform.localPosition.y);
                        }
                        else
                        {
                            word_space[j].transform.localPosition = new Vector3(word_space[j].transform.localPosition.x - 38, word_space[j].transform.localPosition.y);
                        }
                    }
                    cp_last_index++;
                }
            }
            else
            {
                if (cp_count != cp_last_index)
                {
                    for (int i = cp_count - 1; i < words.Length - 1; i++)
                    {
                        word_space[i].transform.localPosition = new Vector3(word_space[i].transform.localPosition.x - 15, word_space[i].transform.localPosition.y);
                    }
                }
            }
        }
        else//index를 처음 넣는거다
        {
            toggleList.Add(cp_count.ToString());
            if (cp_count < cp_last_index)//첫줄만 뒤로 밀림
            {
                for (int i = cp_count - 1; i < cp_last_index - 1; i++)
                {
                    word_space[i].transform.localPosition = new Vector3(word_space[i].transform.localPosition.x + 15, word_space[i].transform.localPosition.y);
                }
                if (word_space[cp_last_index - 2].transform.localPosition.x > 419.5f)//첫번째 줄에서 뒤로 밀었을때 범위를 벗어나면 마지막 clone을 밑으로 내림
                {
                    word_space[cp_last_index - 2].transform.localPosition = new Vector3(-371.5f, -114.5f, 0.0f);
                    for (int j = cp_last_index - 1; j < words.Length - 1; j++)
                    {
                        if (toggleList.Contains(cp_last_index.ToString()))
                        {
                            word_space[j].transform.localPosition = new Vector3(word_space[j].transform.localPosition.x + 38 + 15, word_space[j].transform.localPosition.y);
                        }
                        else
                        {
                            word_space[j].transform.localPosition = new Vector3(word_space[j].transform.localPosition.x + 38, word_space[j].transform.localPosition.y);
                        }
                    }
                    cp_last_index--;
                }
            }
            else//두번째 줄만 뒤로 밀림
            {
                if (cp_count != cp_last_index)
                {
                    for (int i = cp_count - 1; i < words.Length - 1; i++)
                    {
                        word_space[i].transform.localPosition = new Vector3(word_space[i].transform.localPosition.x + 15, word_space[i].transform.localPosition.y);
                    }
                }
            }
        }
    }

    public void QuizInit()
    {
        CantTouchPage.SetActive(false);
        StartCoroutine(EnableCoroutine());
    }

    IEnumerator EnableCoroutine()
    {
        Quiz_Level.text = "" + (level + 1);
        Quiz_Step.text = ""+(Quiz_No + 1);

        //yield return null;
        //yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(GetComponent<AudioSource>().clip.length);

        /*while (!ps.isQuizDone)
        {
            if (ps.isQuizDone)
                break;
            else
                yield return null;
        }*/

        if (gameObject.name == "Quiz1")
        {
            CheckToggleGroup.SetAllTogglesOff();
            //pageOpen[0]++;
            quiz1_answer_choice = Random.Range(0, 5);

            //if (quiz1_question_choice == 0)
            //{
            //words = list.sheets[0].list[1];
            //words = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_examples"].Value.Split(';');//ps.pars_QuizAttribute[3].Split(';');

            //QuizManager.quiz_word_length = words[0].Length;
            //Quiz1_1text[0].text = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_data"].Value;//ps.pars_QuizAttribute[2];//퀴즈 제목을 먼저 넣고


            Debug.Log("quiz1_answer_choice: " + quiz1_answer_choice);
            //Quiz1_1text[quiz1_answer_choice].color = textcolor;
            Quiz1_1text[quiz1_answer_choice].text = list.sheets[level].list[Quiz_No].cor;//정답을 랜덤위치에 넣고

            int j = 1;
            for (int i = 0; i < 5; i++)
            {
                if (i != quiz1_answer_choice) //정답 인덱스와 같지 않으면
                {
                    //Quiz1_1text[i].color = textcolor;
                    if (j == 1)
                    {
                        Quiz1_1text[i].text = list.sheets[level].list[Quiz_No].ex1;
                    }
                    else if (j == 2)
                    {
                        Quiz1_1text[i].text = list.sheets[level].list[Quiz_No].ex2;
                    }
                    else if (j == 3)
                    {
                        Quiz1_1text[i].text = list.sheets[level].list[Quiz_No].ex3;
                    }
                    else if (j == 4)
                    {
                        Quiz1_1text[i].text = list.sheets[level].list[Quiz_No].ex4;
                    }
                    j++;
                }
            }

            // show quiz
            StartCoroutine(ShowAnswers());

            // play audio
            

            isQuiz1Ready = true;
            GetComponent<TimerScript>().StartTimerAfterParsing();
        }
        /*else if (gameObject.name == "Quiz2")
        {
            CheckToggleGroup.SetAllTogglesOff();
            pageOpen[1]++;
            quiz2_question_choice = Random.Range(0, 2);
            quiz2_answer_choice = Random.Range(1, 4);

            //words = ps.pars_QuizAttribute[9].Split(';');
            words = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_examples"].Value.Split('#')[1].Split(';');
            if (quiz2_question_choice == 0)
            {
                QuizManager.quiz_word_length = words[0].Length;
                Quiz2text[0].text = words[0];
                if (quiz2.activeSelf == false)
                    quiz1.SetActive(true);
            }
            else
            {
                Quiz2text[0].text = words[0];

                int j = 1;
                for (int i = 1; i < 4; i++)
                {
                    if (i != quiz2_answer_choice)
                    {
                        Quiz2text[i].text = words[j];
                        j++;
                    }
                }
                if (quiz1.activeSelf == false)
                    quiz2.SetActive(true);
            }
        }
        else if (gameObject.name == "Quiz3")
        {
            CheckToggleGroup.SetAllTogglesOff();
            pageOpen[2]++;
            //words = ps.pars_QuizAttribute[14].Split(';');
            words = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_data"].Value.Split(';');
            //quiz4_answer = ps.pars_QuizAttribute[15].Split(';');
            quiz4_answer = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_examples"].Value.Split(';');

            if (words[0] == "1") Quiz4text[0].text = words[1] + " ____";
            else if (words[0] == "2") Quiz4text[0].text = "____ " + words[1];
            else Quiz4text[0].text = words[0] + " ____ " + words[1];

            Quiz4text[1].text = quiz4_answer[0];
            Quiz4text[2].text = quiz4_answer[1];
            Quiz4text[3].text = quiz4_answer[2];
            quiz4_temp_answer = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_answer"].Value;//pars_QuizAttribute[16];
        }
        else if (gameObject.name == "Quiz4")
        {
            pageOpen[6]++;
            for (int i = 0; i < 3; i++) study_size3_init[i] = Quiz3_3text[i].gameObject.transform.parent.parent;
            for (int i = 0; i < 4; i++) study_size4_init[i] = Quiz3_4text[i].gameObject.transform.parent.parent;
            if (study_size5_init[0] == null)
            {
                for (int i = 0; i < 5; i++) study_size5_init[i] = Quiz3_5text[i].gameObject.transform.parent.parent;
            }

            for (int i = 3; i < 6; i++)
                gameObject.transform.GetChild(i).gameObject.SetActive(false);


            //만들어;던져서;세 자리 수를;콩 주머니를;봅시다.;
            quiz3_arr = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_examples"].Value.Split(';');//pars_QuizAttribute[39].Split(';');
            quiz3_answer = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_answer"].Value.Split(';');//pars_QuizAttribute[40].Split(';');
            quiz3_temp_answer = new string[quiz3_arr.Length];

            if (quiz3_arr.Length == 3)
            {
                gameObject.transform.Find("Size3").gameObject.SetActive(true);
                DragHandlerScript.quizsize = quiz3_arr.Length;
                for (int i = 0; i < 3; i++)
                {
                    Quiz3_3text[i].text = quiz3_arr[i];
                }
            }
            else if (quiz3_arr.Length == 4)
            {
                gameObject.transform.Find("Size4").gameObject.SetActive(true);
                DragHandlerScript.quizsize = quiz3_arr.Length;
                for (int i = 0; i < 4; i++)
                {
                    Quiz3_4text[i].text = quiz3_arr[i];
                }
            }
            else if (quiz3_arr.Length == 5)
            {
                gameObject.transform.Find("Size5").gameObject.SetActive(true);
                DragHandlerScript.quizsize = quiz3_arr.Length;
                for (int i = 0; i < 5; i++)
                {
                    Quiz3_5text[i].text = quiz3_arr[i];
                    Quiz3_5text[i].gameObject.transform.parent.SetParent(study_size5_init[i]);
                }
            }
        }
        else if (gameObject.name == "Quiz5") // by sh.jo
        {
            CheckToggleGroup.SetAllTogglesOff();
            pageOpen[0]++;
            quiz1_question_choice = Random.Range(0, 2);
            quiz1_answer_choice = Random.Range(1, 3);

            //if (quiz1_question_choice == 0)
            //{
            words = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_examples"].Value.Split(';');// type 5 images
            //ps.pars_QuizAttribute[3].Split(';');

            //QuizManager.quiz_word_length = words[0].Length;
            Quiz1_1text[0].text = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_data"].Value;//ps.pars_QuizAttribute[2];//퀴즈 제목을 먼저 넣고

            //insert images
            int currentNum = QuizManager.pageNum * 6 / 5;
            Quiz_images[quiz1_answer_choice-1].sprite = ps.tempSpriteList[currentNum];

            int j = currentNum+1;
            for (int i = 0; i < 3; i++)
            {
                if (i != quiz1_answer_choice-1) //정답 인덱스와 같지 않으면
                {
                    Quiz_images[i].sprite = ps.tempSpriteList[j];
                    j++;
                }
            }
            if (quiz2.activeSelf == false)
                quiz1.SetActive(true);

            isQuiz1Ready = true;
            GetComponent<TimerScript>().StartTimerAfterParsing();
        }
        else if (gameObject.name == "Quiz6") // by sh.jo
        {
            CheckToggleGroup.SetAllTogglesOff();
            pageOpen[0]++;
            quiz1_question_choice = Random.Range(0, 2);
            quiz1_answer_choice = Random.Range(1, 3);

            //if (quiz1_question_choice == 0)
            //{
            words = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_examples"].Value.Split(';');// type 5 images
            //ps.pars_QuizAttribute[3].Split(';');

            //QuizManager.quiz_word_length = words[0].Length;
            Quiz1_1text[0].text = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_data"].Value;//ps.pars_QuizAttribute[2];//퀴즈 제목을 먼저 넣고

            //insert images
            int currentNum = Mathf.FloorToInt(QuizManager.pageNum / 5) * 6 + 3;
            Debug.Log("currentNum: " + currentNum);
            Quiz_images[quiz1_answer_choice - 1].sprite = ps.tempSpriteList[currentNum];

            int j = currentNum + 1;
            for (int i = 0; i < 3; i++)
            {
                if (i != quiz1_answer_choice - 1) //정답 인덱스와 같지 않으면
                {
                    Quiz_images[i].sprite = ps.tempSpriteList[j];
                    j++;
                }
            }
            if (quiz2.activeSelf == false)
                quiz1.SetActive(true);

            isQuiz1Ready = true;
            GetComponent<TimerScript>().StartTimerAfterParsing();
        }*/
        //else if (gameObject.name == "Quiz4")
        //{
        //    pageOpen[3]++;
        //    quiz1_1_question_choice = Random.Range(0, 2);
        //    quiz1_1_answer_choice = Random.Range(1, 4);

        //    //if (quiz1_1_question_choice == 0)
        //    //{
        //    words = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_examples"].Value.Split(';');//.pars_QuizAttribute[21].Split(';');
        //    QuizManager.quiz_word_length = words[0].Length;
        //    Quiz1_2text[0].text = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_data"].Value;//.pars_QuizAttribute[20];//퀴즈 제목을 먼저 넣고
        //    Quiz1_2text[quiz1_1_answer_choice].text = words[0];//정답을 랜덤위치에 넣고


        //    int j = 1;
        //    for (int i = 1; i < 4; i++)
        //    {
        //        if (i != quiz1_1_answer_choice)
        //        {
        //            Quiz1_2text[i].text = words[j];
        //            j++;
        //        }
        //    }
        //    if (quiz2.activeSelf == false)
        //        quiz1.SetActive(true);

        //    isQuiz1Ready = true;
        //    GetComponent<TimerScript>().StartTimerAfterParsing();

        //    Debug.Log("quiz set || quiz1_1_answer_choice = " + quiz1_1_answer_choice);
        //}

        gameObject.SetActive(true);
    }

    IEnumerator ShowAnswers()
    {
        int i = 0;
        while (i < 5)
        {
            yield return new WaitForSeconds(0.2f);
            Answers[i].SetActive(true);
            i++;
        }
        yield return new WaitForSeconds(1.0f);
        string p = "02.Sounds/Stimulus/" + list.sheets[level].list[Quiz_No].filename;
        Octo.GetComponent<AudioSource>().loop = false;
        Octo.GetComponent<AudioSource>().clip = Resources.Load(p) as AudioClip;
        Octo.GetComponent<AudioSource>().Play();
    }


    public void GoNext()
    {
        Quiz_No++;
        StartCoroutine(HideAnswers());
    }

    IEnumerator HideAnswers()
    {
        int i = 0;
        while (i < 5)
        {
            yield return new WaitForSeconds(0.1f);
            Answers[i].SetActive(false);
            i++;
        }
        yield return new WaitForSeconds(1.0f);
        QuizInit();
    }
}
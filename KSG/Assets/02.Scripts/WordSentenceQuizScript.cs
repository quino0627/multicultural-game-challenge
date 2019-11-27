using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WordSentenceQuizScript : MonoBehaviour
{
    public static int resultSc;
    public static int currentSc = 10;
    public Text currentText;
    public Text resultText;
    public GameObject minusAni;
    public GameObject plusAni;
    public GameObject Character;
    public GameObject ScoreGage;

    private PostScript ps = new PostScript();
    private QuizManager qm = new QuizManager();
    public GameObject CantTouchPage;

    private string val_str;
    private Animator _CharFace = null;
    private Animator _FlagState = null;

    public static bool[] page_isAnswer;
    public static int[] page_currentScore;

    public static bool isInit = false;

    public void Update()
    {
        /*if (isInit && QuizManager.pageNum < page_currentScore.Length)
        {
            currentText.text = page_currentScore[QuizManager.pageNum].ToString();
            resultText.text = resultSc.ToString();
            plusAni.transform.GetChild(0).GetComponent<Text>().text = "+" + page_currentScore[QuizManager.pageNum].ToString();
        }*/
    }

    private void Start()
    {
        _CharFace = Character.GetComponent<Animator>();
        _FlagState = currentText.transform.parent.gameObject.GetComponent<Animator>();

        ps = GameObject.FindObjectOfType<PostScript>();
        qm = GameObject.FindObjectOfType<QuizManager>();
    }

    public static void Init()
    {
        PostScript ps = GameObject.Find("Post").GetComponent<PostScript>();

        //page_isAnswer = new bool[ps.parse_WordID.Count];
        //page_currentScore = new int[ps.parse_WordID.Count];

        /*for (int i = 0; i < page_isAnswer.Length; i++)
        {
            page_isAnswer[i] = false;
            page_currentScore[i] = 10;
        }*/
        //Debug.Log("ps.parse_WordID.Count = " + ps.parse_WordID.Count);

        isInit = true;
    }

    public void AnswerBtnClicked()
    {
        if (ps == null)
            ps = GameObject.FindObjectOfType<PostScript>();
        if (qm == null)
            qm = GameObject.FindObjectOfType<QuizManager>();

        if (gameObject.GetComponent<Toggle>().isOn == true)
        {
            //gameObject.transform.Find("Background").Find("Checkmark").GetComponent<Image>().color = new Color(gameObject.transform.Find("Background").Find("Checkmark").GetComponent<Image>().color.r, gameObject.transform.Find("Background").Find("Checkmark").GetComponent<Image>().color.g, gameObject.transform.Find("Background").Find("Checkmark").GetComponent<Image>().color.b, 1.0f);
            

            Debug.Log(Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1))-1);
            if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1))-1 == RandomQuizScript.quiz1_answer_choice)
            {
                ResultScore();
                //StartCoroutine(Plus());
                //QuizManager.page_currentScore[QuizManager.pageNum] = page_currentScore[QuizManager.pageNum];
                //gameObject.transform.Find("Label").GetComponent<Text>().color = new Color(0.113f, 0.859f, 0.086f);

                Debug.Log("Correct");
                currentText.transform.position = new Vector3(this.transform.position.x+30.0f, this.transform.position.y+20.0f, this.transform.position.z);
                currentText.gameObject.SetActive(true);

                
                //gameObject.transform.parent.parent.parent.parent.parent.Find("UIpage").Find("Oimage").gameObject.SetActive(true);

                //qm.AnswerCheck(1);

                //val_str = QuizManager.sentence_count.ToString() + ",TestResultTable,q1_result" + ",1";
                //ps.PostInit(val_str); 
            }
            else
            {
                //WrongScore();
                //StartCoroutine(Minus());
                //QuizManager.page_currentScore[QuizManager.pageNum] = page_currentScore[QuizManager.pageNum];

                Debug.Log("fault");

                
                //qm.AnswerCheck(0);

                //val_str = QuizManager.sentence_count.ToString() + ",TestResultTable,q1_result" + ",0";
                //ps.PostInit(val_str);
            }
            CantTouchPage.SetActive(true);
            Transform tempTransform = GameObject.Find("Quiz1").transform;
            tempTransform.GetComponent<RandomQuizScript>().GoNext();
        }
    }

    IEnumerator DeleteOXimage()
    {
        CantTouchPage.SetActive(true);
        Transform tempTransform = GameObject.Find("Quiz1").transform;
        tempTransform.GetComponent<RandomQuizScript>().GoNext();
        yield return new WaitForSecondsRealtime(2.0f);
        CantTouchPage.SetActive(false);
    }

    public IEnumerator Minus()
    {
        if (!minusAni.activeSelf)
        {
            _CharFace.SetBool("Cry", true);
            _FlagState.SetBool("flagBlow", true);
            minusAni.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _FlagState.SetBool("flagBlow", false);
            _CharFace.SetBool("Cry", false);
            minusAni.SetActive(false);
        }
    }

    public IEnumerator Plus()
    {
        _CharFace.SetBool("Smile", true);
        _FlagState.SetBool("flagUp", true);
        plusAni.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _FlagState.SetBool("flagUp", false);
        _CharFace.SetBool("Smile", false);
        plusAni.SetActive(false);
    }

    public void WrongScore()
    {
        if (page_currentScore[QuizManager.pageNum] > 0)
        {
            Debug.Log("hey? || " + currentSc);

            page_currentScore[QuizManager.pageNum] = page_currentScore[QuizManager.pageNum] - 2;
            Debug.Log("page_currentScore[QuizManager.pageNum] = " + page_currentScore[QuizManager.pageNum]);
        }
    }

    public void ResultScore()
    {
        resultSc += 100;
        resultText.text = ""+resultSc;
        ScoreGage.GetComponent<MovingGage>().AnimateGage(ScoreGage.GetComponent<Slider>().value + 0.2f, 1.0f);
    }
    
    IEnumerator GageAni(float limit, float fadeOutTime, System.Action nextEvent = null)
    {
        Slider sl = ScoreGage.GetComponent<Slider>();
        int i = 0;
        while (i < 20)
        {
            yield return new WaitForSeconds(0.05f);
            sl.value += 0.01f;
            Debug.Log(i);
            Debug.Log(sl.value);
            i++;
        }
    }

}
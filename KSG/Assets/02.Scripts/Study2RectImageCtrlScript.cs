using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Study2RectImageCtrlScript : MonoBehaviour
{
    void OnEnable()
    {
        if (gameObject.name == "WordImage")//퀴즈2_1에서
        {
            Debug.Log("QuizManager.quiz_word_length.ToString() = " + QuizManager.quiz_word_length.ToString());

            if(QuizManager.quiz_word_length == 0)
            StartCoroutine(timer());
        }
        else//Study2에서
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Textbox/panel" + NextBtnClickedScript.word_length.ToString());
            if (NextBtnClickedScript.word_length == 1)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(438, 438);
                gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(438, 438);
                gameObject.transform.GetChild(0).GetComponent<Text>().fontSize = 335;
            }
            else if (NextBtnClickedScript.word_length == 2)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(641, 375);
                gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(641, 375);
                gameObject.transform.GetChild(0).GetComponent<Text>().fontSize = 290;
            }
            else if (NextBtnClickedScript.word_length == 3)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(922, 375);
                gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(922, 375);
                gameObject.transform.GetChild(0).GetComponent<Text>().fontSize = 290;
            }
            else if (NextBtnClickedScript.word_length == 4)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(922, 278);
                gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(922, 278);
                gameObject.transform.GetChild(0).GetComponent<Text>().fontSize = 220;
            }
        }
    }

    void Quiz1Init()
    {
        int length = PostScript.Getinstance.quizJson["data"][0][QuizManager.pageNum]["quiz_data"].Value.Length;
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Textbox/size" + length.ToString());
        if (length == 1)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 122);
        }
        else if (length == 2)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(212, 122);
        }
        else if (length == 3)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(305, 122);
        }
        else if (length == 4)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(383, 122);
        }
    }

    IEnumerator timer()
    {
        yield return new WaitForEndOfFrame();

        if (QuizManager.quiz_word_length == 0)
            StartCoroutine(timer());
        else
            Quiz1Init();
    }
}
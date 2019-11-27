using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IDropHandler
{
    private PostScript ps = new PostScript();
    private WordSentenceQuizScript ws = new WordSentenceQuizScript();
    private QuizManager qm = new QuizManager();

    public GameObject EmptyObject;

    private string slot_puzzleName;
    private int slot_puzzleNumber;
    private string image_puzzleName;
    private int image_puzzleNumber;
    private bool checkAnswer;
    public static int moveCount;
    public static int moveCount_last;
    private string val_str;

    int adjustment = 0;
    int previousAdd = 0;

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            //Debug.Log("here come");
            DragHandlerScript.itemBeingDragged.transform.SetParent(transform);
            DragHandlerScript.slotName = DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1);

            if (DragHandlerScript.itemBeingDragged.name.Remove(5, 1) == "Image")//Study
            {
                RandomQuizScript.quiz3_temp_answer[Int32.Parse(DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(0, DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1)) - 1]
                    = DragHandlerScript.itemBeingDragged.name.Remove(0, DragHandlerScript.itemBeingDragged.name.Length - 1);
                Debug.Log("Int32Parse? = " + (Int32.Parse(DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(0, DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1)) - 1));
                Debug.Log("RandomQuizScript.quiz3_temp_answer = " + RandomQuizScript.quiz3_temp_answer[Int32.Parse(DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(0, DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1)) - 1]);
            }
            else//Test
            {
                //WeeklyTestCtrlScript.Test3_temp_answer[Int32.Parse(DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(0, DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1)) - 1]
                //    = DragHandlerScript.itemBeingDragged.name.Remove(0, DragHandlerScript.itemBeingDragged.name.Length - 1);
            }

            if (DragHandlerScript.questionName == "Puzzle" && DragHandlerScript.slotName == "Slot")
            {
                if (DragHandlerScript.itemBeingDragged.name.Remove(5, 1) == "Testi")
                {
                    //if (WeeklyTestCtrlScript.Test3_temp_answer[Int32.Parse(DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(0, DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1)) - 1] != WeeklyTestCtrlScript.test_ex[Int32.Parse(DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(0, DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1)) - 1])
                    //{
                    //    WeeklyTestCtrlScript.wrong_answer_check[WeeklyTestCtrlScript.testCount - 1] += (WeeklyTestCtrlScript.Test3_temp_answer[Int32.Parse(DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(0, DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1)) - 1]);//틀린거 넣을 곳
                    //}
                }
                moveCount_last++;
                Debug.Log("moveCount_last = " + moveCount_last);
            }
            else if (DragHandlerScript.questionName == "Slot" && DragHandlerScript.slotName == "Puzzle")
            {
                if (DragHandlerScript.itemBeingDragged.name.Remove(5, 1) == "Testi")
                {
                    //if (WeeklyTestCtrlScript.wrong_answer_check[WeeklyTestCtrlScript.testCount - 1] != null)
                    //{
                    //    WeeklyTestCtrlScript.wrong_answer_check[WeeklyTestCtrlScript.testCount - 1] = WeeklyTestCtrlScript.wrong_answer_check[WeeklyTestCtrlScript.testCount - 1].Replace((WeeklyTestCtrlScript.Test3_temp_answer[Int32.Parse(DragHandlerScript.itemBeingDragged.transform.parent.name.Remove(0, DragHandlerScript.itemBeingDragged.transform.parent.name.Length - 1)) - 1]), "");
                    //}
                }
                moveCount--;
                moveCount_last--;
                Debug.Log("moveCount_last minus = " + moveCount_last);
            }
            ps = GameObject.FindObjectOfType<PostScript>();
            if (moveCount == DragHandlerScript.quizsize || moveCount_last == DragHandlerScript.quizsize)//모든 단어를 다 위로 올렸을때
            {
                checkAnswer = true;
                adjustment = 0;
                for (int i = 1; i <= DragHandlerScript.quizsize; i++)
                {
                    image_puzzleName = gameObject.transform.parent.Find("Slot" + i.ToString()).GetChild(0).name.Remove(0, gameObject.transform.parent.Find("Slot" + i.ToString()).GetChild(0).name.Length - 1);
                    image_puzzleNumber = Int32.Parse(image_puzzleName);

                    if (DragHandlerScript.itemBeingDragged.name.Remove(5, 1) == "Image")//Study퀴즈에서
                    {
                        if ((RandomQuizScript.quiz3_temp_answer[i - 1] != RandomQuizScript.quiz3_answer[i - 1]))//틀린 단어가 있으면
                        {
                            Debug.Log("startIndex = " + QuizManager.startIndex);
                            adjustment++;
                            Debug.Log("!!!!!QuizSize = " + DragHandlerScript.quizsize);
                            Debug.Log("RandomQuizScript.quiz3_temp_answer[" + (i - 1) + "] = " + RandomQuizScript.quiz3_temp_answer[i - 1]);
                            Debug.Log("RandomQuizScript.quiz3_answer[" + (i - 1) + "] = " + RandomQuizScript.quiz3_answer[i - 1]);
                            //원래 있던 자리로 내린다 size 별로
                            if (DragHandlerScript.quizsize == 3) gameObject.transform.parent.Find("Slot" + i.ToString()).GetChild(0).SetParent(RandomQuizScript.study_size3_init[image_puzzleNumber - 1]);
                            else if (DragHandlerScript.quizsize == 4) gameObject.transform.parent.Find("Slot" + i.ToString()).GetChild(0).SetParent(RandomQuizScript.study_size4_init[image_puzzleNumber - 1]);
                            else if (DragHandlerScript.quizsize == 5)
                            {
                                string temp_objName = gameObject.transform.parent.Find("Slot" + i.ToString()).GetChild(0).name;
                                gameObject.transform.parent.Find("Slot" + i.ToString()).GetChild(0).SetParent(RandomQuizScript.study_size5_init[image_puzzleNumber - 1]);
                                Debug.Log(RandomQuizScript.study_size5_init[image_puzzleNumber - 1]);

                                if (RandomQuizScript.study_size5_init[image_puzzleNumber - 1].name.Contains("Slot"))
                                {
                                    Debug.Log("--------------" + RandomQuizScript.study_size5_init[image_puzzleNumber - 1].name.Remove(0, 4));
                                    Debug.Log("-temp_objName-" + temp_objName);

                                    int tempint = Int32.Parse(temp_objName.Remove(0, 5));

                                    Debug.Log("tempInt = " + tempint);
                                    Debug.Log("i.Tostring() = " + i.ToString());
                                    RandomQuizScript.quiz3_temp_answer[image_puzzleNumber - 1] = "" + tempint;

                                    moveCount_last++;
                                }
                            }
                            moveCount--;
                            moveCount_last--;

                            Debug.Log("Slot" + i.ToString());
                            Debug.Log("Back to = " + RandomQuizScript.study_size5_init[image_puzzleNumber - 1]);
                            checkAnswer = false;
                        }
                        if (adjustment == 1)
                        {
                           if (!WordSentenceQuizScript.page_isAnswer[QuizManager.pageNum])
                           {
                                Debug.Log("CountTemp[6] = " + QuizManager.page_ModifyCount[QuizManager.pageNum]);
                           }
                        }
                    }
                    else//Test에서는 밑으로 내리지 않으므로 안내림
                    {
                        //if ((WeeklyTestCtrlScript.Test3_temp_answer[i - 1] != WeeklyTestCtrlScript.test_ex[i - 1]))
                        //{
                        //    checkAnswer = false;
                        //}moveCount_last
                    }
                }
                if (checkAnswer == true)
                {
                    if (DragHandlerScript.itemBeingDragged.name.Remove(5, 1) == "Image")
                    {
                        if (!WordSentenceQuizScript.page_isAnswer[QuizManager.pageNum])
                        {
                            //WordSentenceQuizScript.ResultScore();
                            StartCoroutine(EmptyObject.GetComponent<WordSentenceQuizScript>().Plus());

                            //WordSentenceQuizScript.page_currentScore[QuizManager.pageNum] = WordSentenceQuizScript.currentSc;
                            QuizManager.page_currentScore[QuizManager.pageNum] = WordSentenceQuizScript.page_currentScore[QuizManager.pageNum];
                        }
                        WordSentenceQuizScript.page_isAnswer[QuizManager.pageNum] = true;
                        QuizManager.page_isAnswer[QuizManager.pageNum] = WordSentenceQuizScript.page_isAnswer[QuizManager.pageNum];


                        gameObject.transform.parent.parent.parent.parent.parent.Find("UIpage").Find("Oimage").gameObject.SetActive(true);
                        QuizManager.trueorfalse[QuizManager.pageNum] = true;
                        GameObject.FindObjectOfType<QuizManager>().AnswerCheck(1);
                        val_str = QuizManager.sentence_count.ToString() + ",TestResultTable,q3_result" + ",1";
                        ps.PostInit(val_str);
                        StartCoroutine(DeleteOXimage());
                    }
                    else
                    {
                        //if (WeeklyTestCtrlScript.next_or_prev == true) WeeklyTestCtrlScript.percent[WeeklyTestCtrlScript.testCount - 1] = true;
                        //else WeeklyTestCtrlScript.percent[WeeklyTestCtrlScript.testCount] = true;

                        //val_str = WeeklyTestCtrlScript.count_choice.ToString() + ",TestResultTable,t3_result" + ",1";
                        //ps.PostInit(val_str);
                    }
                }
                else
                {
                    if (DragHandlerScript.itemBeingDragged.name.Remove(5, 1) == "Image")
                    {
                        //WordSentenceQuizScript.WrongScore();
                        StartCoroutine(EmptyObject.GetComponent<WordSentenceQuizScript>().Minus());

                        //WordSentenceQuizScript.page_currentScore[QuizManager.pageNum] = WordSentenceQuizScript.currentSc;
                        QuizManager.page_currentScore[QuizManager.pageNum] = WordSentenceQuizScript.page_currentScore[QuizManager.pageNum];

                        gameObject.transform.parent.parent.parent.parent.parent.Find("UIpage").Find("Ximage").gameObject.SetActive(true);
                        QuizManager.trueorfalse[QuizManager.pageNum] = false;

                        GameObject.FindObjectOfType<QuizManager>().AnswerCheck(0);
                        val_str = QuizManager.sentence_count.ToString() + ",TestResultTable,q3_result" + ",0";
                        ps.PostInit(val_str);
                        StartCoroutine(DeleteOXimage());
                    }
                    else
                    {
                        //if (WeeklyTestCtrlScript.next_or_prev == true) WeeklyTestCtrlScript.percent[WeeklyTestCtrlScript.testCount - 1] = false;
                        //else WeeklyTestCtrlScript.percent[WeeklyTestCtrlScript.testCount] = false;

                        //val_str = WeeklyTestCtrlScript.count_choice.ToString() + ",TestResultTable,t3_result" + ",0";
                        //ps.PostInit(val_str);
                    }
                }
            }
        }
        else
        {
            Debug.Log("ewqkljeqw");
        }
    }


    IEnumerator DeleteOXimage()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        gameObject.transform.parent.parent.parent.parent.parent.Find("UIpage").Find("Oimage").gameObject.SetActive(false);
        gameObject.transform.parent.parent.parent.parent.parent.Find("UIpage").Find("Ximage").gameObject.SetActive(false);
    }


}
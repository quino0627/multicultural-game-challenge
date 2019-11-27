using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCtrlScript : MonoBehaviour
{

    private string[] tutorialpage = { "quiz1_1_1", "quiz1_1_2", "quiz1_2_1", "quiz1_2_2", "quiz2_1_1", "quiz2_1_2", "quiz2_1_3", "quiz2_2_1", "quiz2_2_2", "quiz3_1", "quiz3_2", "quiz3_3", "quiz4_1", "quiz4_2" };
    private int tutorialPageIndex;

    private void OnEnable()
    {
        tutorialPageIndex = 0;
        gameObject.transform.Find(tutorialpage[tutorialPageIndex]).gameObject.SetActive(true);
    }

    public void TutorialNextBtnClicked()
    {
        if (tutorialPageIndex != 13)
        {
            gameObject.transform.Find(tutorialpage[tutorialPageIndex]).gameObject.SetActive(false);
            tutorialPageIndex++;
            gameObject.transform.Find(tutorialpage[tutorialPageIndex]).gameObject.SetActive(true);
        }
    }

    public void TutorialPrevBtnClicked()
    {
        if (tutorialPageIndex != 0)
        {
            gameObject.transform.Find(tutorialpage[tutorialPageIndex]).gameObject.SetActive(false);
            tutorialPageIndex--;
            gameObject.transform.Find(tutorialpage[tutorialPageIndex]).gameObject.SetActive(true);
        }
    }

    public void TutorialExitBtnClicked()
    {
        gameObject.transform.Find(tutorialpage[tutorialPageIndex]).gameObject.SetActive(false);
        if(NextBtnClickedScript.startIndex == 7 && NextBtnClickedScript.sentence_count == 1)
        {
            gameObject.transform.parent.Find("BackgroundPage").Find("Study5").gameObject.SetActive(false);
            gameObject.transform.parent.Find("BackgroundPage").Find("Quiz1").gameObject.SetActive(true);
            NextBtnClickedScript.startIndex++;
        }
        gameObject.SetActive(false);
    }
}

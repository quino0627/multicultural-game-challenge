using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpreadChoices : MonoBehaviour
{
    public GameData data;
    public int level;
    public int Quiz_No = 0;

    public GameObject[] Answers;
    public GameObject Crab;
    public Text Quiz_Level;
    public Text Quiz_step;
    public Text[] choice_texts;
    public static int corr_ans_pos;

    public static string[] words;
    
    public static ArrayList toggleList = new ArrayList();
    
    // Start is called before the first frame update
    void Start()
    {
        QuizInit();
    }

    public void QuizInit()
    {
        //StartCoroutine(EnableCoroutine());
    }

    /*IEnumerator EnableCoroutine()
    {
        Quiz_Level.text = "" + (level + 1);
        Quiz_step.text = "" + (Quiz_No + 1);
        //yield return new WaitForSecondsRealtime(GetComponent<AudioSource>().clip.length);
        //CheckToggleGroup.SetAllTogglesOff();
        corr_ans_pos = Random.Range(0,5);

        choice_texts[corr_ans_pos].text = data.data[level].corrAns1;

    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
}

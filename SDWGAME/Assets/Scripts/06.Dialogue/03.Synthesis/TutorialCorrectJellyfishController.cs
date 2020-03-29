using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialCorrectJellyfishController : MonoBehaviour
{
    
    public float speed = 2f;
    public bool onCircle;
    public bool sparked;
    public bool ischecked;
    public Transform InitialTransform;
    public GameObject Crab = null;
    public SpreadChoices tmp;
    public GameObject Carrier;
    
    public TextMeshPro child;

    // Start is called before the first frame update
    void Start()
    {
        child = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; //움직이는 속도
        float fly = 2 * speed * Time.deltaTime; //빠른 속도...
        
        if (!onCircle) //onCircle은 DragAndDrop.cs에서 설정해줌
        {
            return; //해파리가 circle에 놓여져 있지 않으면 return
        }
        
        
//            Crab.transform.Find("DescriptionBubble").GetComponent<SynthesisDescriptionController>().CorrectAnswer();
        //QuizManager.GetComponent<SpreadChoices>().PlusTotalTry();
        Carrier.SetActive(true);

        GameObject.FindObjectOfType<TutorialSynthesisManager>().isCorrected = true;
        
        
        
        // level 올라가면 그저 정답간에 순서 맞춰서 배정하면됨
//        for (int i = 0; i < tmp.corrAnsCnt+tmp.wrongAnsCnt; i++)
//        {
//            if (child.text == tmp.choiceTexts[tmp.corrAnsPosIndex[i]].text)
//            {
//                transform.SetParent(tmp.PickedJfPos[i]);
//                transform.localPosition= Vector2.zero;
//            }
//        }
            
        //quizManager에서 맞춘 글자수 올려줌.
//        JfSuccess script = QuizManager.GetComponent<JfSuccess>();
//        script.currCntCorrAns++;
        ischecked = true;

        //json을 위해 chosenAns;
        //tmp.chosenAns.Add(child.text); 
            
        //정답란에 글자를 띄워줌
//        for (int i = 0; i < tmp.corrAnsCnt+tmp.wrongAnsCnt; i++)
//        {
//            if (child.text == tmp.choiceTexts[tmp.corrAnsPosIndex[i]].text)
//            {
//                tmp.PickedAnswer[i].SetActive(true);
//            }
//        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class TutorialDetectionManager : MonoBehaviour
{
   
    
    private DialogueManagerV2 theDM;
    
    // 5개의 통 중 가운데 놓여있는 통
    public GameObject CorrectBarrel;
    // 정답이 아닌 통들
    public GameObject WrongBarrel_1;
    public GameObject WrongBarrel_2;
    public GameObject WrongBarrel_3;
    public GameObject WrongBarrel_4;
    
    public GameObject CoinInCorrectBarrel;
    
    public GameObject Octo;

    private GameObject description;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI oneSentenceText;
    
    [HideInInspector] private string[] answerStrings = new string[5];

    public Boolean clickedCorrectAnswer;
    


    // Start is called before the first frame update
    void Start()
    {

        theDM = FindObjectOfType<DialogueManagerV2>();
        
        this.description = GameObject.Find("Octopus").transform.Find("DescriptionBubble").gameObject;
        // 배럴들은 outlet으로 연결

        clickedCorrectAnswer = false;
        
        QuizInit();
    }

    public void QuizInit()
    {
        answerStrings[0] = "개";
        answerStrings[1] = "가";
        answerStrings[2] = "글";
        answerStrings[3] = "고";
        answerStrings[4] = "강";
        StartCoroutine(DetectionTutorialStage());
    }

    IEnumerator DetectionTutorialStage()
    {
        yield return Octo.GetComponent<OctoSinusodialMove>().TutorialMoveOctopus();
        yield return new WaitForSeconds(1f);
        
//        Octo.transform.Find("DescriptionBubble").gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        // 통에 동전을 넣어두었는데 찾을 수가 없다.
        theDM.ShowDialogue();
        var tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            Debug.Log(theDM.GetCurrentSentenceNumber());
            yield return new WaitForFixedUpdate();
        }
        // 내가 단어를 소리내서 말할건데, 그 단어가 적힌 통에 동전이 있다.
        Debug.Log("NEXT DIALOGUE");
        // 안내 음성
        yield return new WaitForSeconds(3.0f);
        // 샘플 사운드
        SoundManager.Instance.Play_EliminationTutorialSampleSound();
        yield return new WaitForSeconds(1.0f);
        
        // check whether is next dialogue? 
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while(tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        
        // 어떤 통인지 선택해 주겠어?
        // 안내 음성
        yield return new WaitForSeconds(3.0f);
        
        CoinInCorrectBarrel.SetActive(true);
        CoinInCorrectBarrel.GetComponent<Animator>().SetBool("Appear", false);

        SoundManager.Instance.Play_BarrelCreated();
        WrongBarrel_2.SetActive(true);
        WrongBarrel_2.GetComponent<Animator>().Play("Entry");
        yield return new WaitForSeconds(0.4f);
        
        SoundManager.Instance.Play_BarrelCreated();
        WrongBarrel_4.SetActive(true);
        WrongBarrel_4.GetComponent<Animator>().Play("Entry");
        yield return new WaitForSeconds(0.4f);
        
        SoundManager.Instance.Play_BarrelCreated();
        WrongBarrel_3.SetActive(true);
        WrongBarrel_3.GetComponent<Animator>().Play("Entry");
        yield return new WaitForSeconds(0.4f);
        
        SoundManager.Instance.Play_BarrelCreated();
        CorrectBarrel.SetActive(true);
        CorrectBarrel.GetComponent<Animator>().Play("Entry");
        yield return new WaitForSeconds(0.4f);
        
        SoundManager.Instance.Play_BarrelCreated();
        WrongBarrel_1.SetActive(true);
        WrongBarrel_1.GetComponent<Animator>().Play("Entry");
        yield return new WaitForSeconds(0.4f);


        while (!clickedCorrectAnswer)
        {
            yield return new WaitForEndOfFrame();
        }
        
        theDM.StartNextScript();
        
        yield return new WaitForSeconds(3.0f);
        CoinInCorrectBarrel.GetComponent<Animator>().SetBool("Appear", false);
        
        Debug.Log(theDM.GetCurrentSentenceNumber());
        // check whether is next dialogue? 
        tmpCount = theDM.GetCurrentSentenceNumber();
        theDM.AllowNextStep();
        while(tmpCount == theDM.GetCurrentSentenceNumber())
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1.0f);
        theDM.AllowNextStep();
        
        
        
        
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

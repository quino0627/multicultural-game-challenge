using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


// 모든 Tutorial[gameType]Manager가 상속받을 클래스
public class DialogueManagerV2 : MonoBehaviour
{
    public string type;
    public GameObject TotalStorage;
    public KeepTrackController TotalStorageScript;
    public int currentLevel;

    public GameObject DisplayClickable;
    
    // Dialogue를 조작하는데 공통적으로 필요한 항목들을 여기에다가 작성합니다.
    private List<string> listSentences = new List<string>();
//    private Sprite window = null;

    public int count;
    public bool talking;

    public TextMeshProUGUI text;
//    public SpriteRenderer window;
    // Start is called before the first frame update

    public DialogueV2 dialogue;


    public Animator aniWindow;
//    public SpriteRenderer srSprite;
    public SpriteRenderer srWindow;
    
    
   
    private void Start()
    {
//        TotalStorage = GameObject.Find("TotalStorage");
//        TotalStorageScript = TotalStorage.GetComponent<KeepTrackController>();
//        currentLevel = TotalStorageScript.chosenLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogue.flags[count] && !DisplayClickable.activeSelf)
        {
            DisplayClickable.SetActive(true);
        }
        else if(!dialogue.flags[count] && DisplayClickable.activeSelf)
        {
            DisplayClickable.SetActive(false);
        }
        // 만약 말하고 있는 상태일 때?
        if (talking)
        {
            // 만약 마우스 클릭을 한다면?
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("I Clicked And");
                // flags[count]가 true일때만 넘어간다?
                if (!dialogue.flags[count])
                {
                    Debug.Log("DIALOGUE's FLAG IS TRUE");
                    return;
                    
                }

                ++count;

                if (count == listSentences.Count)
                {
                    StopAllCoroutines();
                    ExitDialogue();
                    Invoke(nameof(LoadNextScene), 1.0f);
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartDialogueCoroutine());
                }
            }
        }
    }

    // 다이어로그 시작
    public void ShowDialogue()
    {
        Debug.Log("IN SHOWDIALOGUE V2");
        
        aniWindow.SetBool("Appear", true);
        
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
        }

        StartCoroutine(StartDialogueCoroutine());

    }

    public int GetCurrentSentenceNumber()
    {
        return count;
    }

    public void AllowNextStep()
    {
        dialogue.flags[count] = true;
    }
    
    public void StartNextScript()
    {
        dialogue.flags[count] = true;
        count++;
        StopAllCoroutines();
        StartCoroutine(StartDialogueCoroutine());
    }

    IEnumerator StartDialogueCoroutine()
    {
        SoundManager.Instance.Play_SpeechBubblePop();
        talking = true;
        text.text = "";
        if (count>0)
        {
            yield return new WaitForSeconds(0.05f);
        }
        // count 가 0인 경우, 즉 첫이미지인경우
        else
        {
            srWindow.sprite = dialogue.dialogueWindow;
            Debug.Log("HI?");
        }
        
//        Debug.Log(listSentences[count].Length);
        // 텍스트 출력 
        SoundManager.Instance.Play_SoundYes();
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];
            yield return new WaitForSeconds(0.02f);
        }
//        for (int i = 0; i < dialogue.sentences[count].Length; i++)
//        {
//            text.text += dialogue.sentences[count][i];
//            yield return new WaitForSeconds(0.02f);
//        }
    }
    
    public void ExitDialogue()
    {
        talking = false;
        ///// 초기화 //////
        count = 0;
        listSentences.Clear();
        // 애니메이터 초기화
    }
    
    public void LoadNextScene()
    {
        if (type == "Detection")
        {
            SceneManager.LoadScene("DetectionGame");
        }

        if (type == "Synthesis")
        {
            //매직넘버
            if (TotalStorageScript.tmpStage[1] < 14)
            {
                SceneManager.LoadScene("CrabLevel1");
            }
            else
            {
                SceneManager.LoadScene("CrabLevel2");
            }
        }

        if (type == "Elimination")
        {
            SceneManager.LoadScene("EliminationEscape");
        }

        if (type == "Alternative")
        {
            SceneManager.LoadScene("AlternativeGame");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public string type;
    public static DialogueManager instance;
    // 일반적인 raycast는 UI 에서 먹히지 않기 때문에 GraphicRaycaster가 필요하당!
    private GraphicRaycaster raycaster;
    
    public GameObject TotalStorage;
    public TotalDataManager totalStorageScript;
    public int currentLevel;
    
    #region Singleton
    private void Awake()
    {
        this.raycaster = GetComponent<GraphicRaycaster>();
        Debug.Log("IN AWAKE");
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion Singleton


    private List<string>  listSentences = new List<string>();
    private List<Sprite> listSprites = new List<Sprite>();
    private List<Sprite> listWindows = new List<Sprite>();
    private List<GameObject> listClickObject = new List<GameObject>();

    public Animator aniSprite;
    public Animator aniWindow;
    private bool talking;
    
    // 대화 진행 상황 카운트 
    private int count;
    
    public TextMeshProUGUI text;
    public SpriteRenderer srSprite;
    public SpriteRenderer srWindow;


    private void Start()
    {
        TotalStorage = GameObject.Find("TotalStorage");
        totalStorageScript = TotalStorage.GetComponent<TotalDataManager>();
        currentLevel = totalStorageScript.chosenLevel;
    }


    // Update is called once per frame
    void Update()
    {
        if (talking)
        {
            // 대화를 읽음
//            if (Input.GetKeyDown(KeyCode.Space))
            if(Input.GetMouseButtonDown(0))
            {
                if (listClickObject[count] != null)
                {
                    //Set up the new Pointer Event
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    List<RaycastResult> results = new List<RaycastResult>();
 
                    //Raycast using the Graphics Raycaster and mouse click position
                    pointerData.position = Input.mousePosition;
                    this.raycaster.Raycast(pointerData, results);
 
                    //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                    foreach (RaycastResult result in results)
                    {
                        Debug.Log("Hit " + result.gameObject.name);
                    }

                    if (results.Count <= 0 )
                    {
                        return;
                    }
                    
                    if (listClickObject[count].name != results[0].gameObject.name)
                    {
                        return;
                    }

                }
                ++count;
                // 모든 대화를 읽은 경우
                if (count == listSentences.Count)
                {
                    // 모든 코루틴 종료 - 해당 스크립트에 있는? 코루틴만 종료시킨다. 
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

    // 다이얼로그 시작
    public void ShowDialogue(Dialogue dialogue)
    {
        Debug.Log("IN SHOWDIALOGUE");
        // 리스트 채우기 
        Debug.Log(dialogue.sentences.Length);
        Debug.Log(listSentences);
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listWindows.Add(dialogue.dialogueWindow[i]);
            listClickObject.Add(dialogue.clickNeedObject[i]);
        }
        
        // 캐릭터 및 대화창이 나오도록 함 
        aniSprite.SetBool("Appear", true);
        aniWindow.SetBool("Appear", true);

        StartCoroutine(StartDialogueCoroutine());
    }

    // 현재 다이어로그의 문장들에서 몇 번째 단어일까욧?
    public int GetCurrentSentenceNumber()
    {
        return count;
    }

    IEnumerator StartDialogueCoroutine()
    {
        talking = true;
        text.text = "";
        if (count > 0)
        {
            // 윈도우가 다른 경우 
            if (listWindows[count - 1] != listWindows[count])
            {
                aniWindow.SetBool("Change", true);
                aniWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.01f);
                // 출력할 스프라이트, 윈도우 저장
                srSprite.sprite = listSprites[count];
                srWindow.sprite = listWindows[count];
                aniWindow.SetBool("Appear", true);
                aniWindow.SetBool("Change", false);
            }
            // 윈도우가 같은 경우
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        // count가 0인 경우(첫이미지인경우)
        else
        {
            srWindow.sprite = listWindows[count];
            srSprite.sprite = listSprites[count];
            

        }
        
        // 텍스트 출력 
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];
            yield return new WaitForSeconds(0.02f);
        }

    }

    // 다이어로그가 모두 종료되었을 때 실행되는 함수 
    public void ExitDialogue()
    {
        talking = false;
        ///// 초기화 //////
        count = 0;
        listSentences.Clear();
        listWindows.Clear();
        listSprites.Clear();
        // 애니메이터 초기화
        aniSprite.SetBool("Appear", false);
        aniWindow.SetBool("Appear", false);
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
            //if (totalStorageScript.tmpStage[1] < 14)
            //{
                SceneManager.LoadScene("CrabLevel1");
            //}
            //else
            //{
            //    SceneManager.LoadScene("CrabLevel2");
            //}
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

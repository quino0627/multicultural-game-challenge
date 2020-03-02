using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlternativeDialogue : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;

    private DialogueManager theDM;

    public Animator aniBubbles;
    public Animator aniWords;
    public Animator aniPointer;
    public Animator aniOriginWord;
    public Animator aniExpectWord;
    public GameObject objSpeechBubble;

    private int? tmpCount = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        theDM = FindObjectOfType<DialogueManager>();
        Debug.Log(theDM);
        theDM.ShowDialogue(dialogue);
    }

    // Update is called once per frame
    void Update()
    {
        // 한번만 실행되도록 
        if (tmpCount == theDM.GetCurrentSentenceNumber())
        {
            return;
        }
        else
        {
            tmpCount = theDM.GetCurrentSentenceNumber();
            // 아기 해마들이 ~
            if (theDM.GetCurrentSentenceNumber() == 0)
            {

                aniBubbles.SetBool("Appear", true);
            }
            // 왼쪽과 오른쪽 글자를~
            if (theDM.GetCurrentSentenceNumber() == 1)
            {
                
                
                aniWords.SetBool("Appear", true);
            }
            // 왼쪽에서 어떤~ 
            if (theDM.GetCurrentSentenceNumber() == 2)
            {
                aniOriginWord.SetBool("Center", true);
                aniExpectWord.SetBool("Center", true);
//                aniPointer.SetBool("Appear", true);
                objSpeechBubble.SetActive(true);
            }
            // 바꾸면 되는 음소가~
            if (theDM.GetCurrentSentenceNumber() == 3)
            {
            }
            
            if (theDM.GetCurrentSentenceNumber() == 4)
            {
            }
        }
        
        //
        
        
        
    }

    private void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
    }
}
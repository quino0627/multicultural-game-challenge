using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EliminationDialogue : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;

    private DialogueManager theDM;

//    public Animator aniBubbles;
//    public Animator aniWords;
//    public Animator aniPointer;
//    public Animator aniPointer;
    public Animator aniSpeechBubble;
    public Animator aniFishes;
    
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
            // 다리가 없
            if (theDM.GetCurrentSentenceNumber() == 0)
            {

//                aniBubbles.SetBool("Appear", true);
            }
            // 해파리들이~
            if (theDM.GetCurrentSentenceNumber() == 1)
            {
                
                aniFishes.SetBool("Appear", true);
//                aniWords.SetBool("Appear", true);
            }
            // 해당하는~
            if (theDM.GetCurrentSentenceNumber() == 2)
            {
                aniSpeechBubble.SetBool("Appear", true);
//                aniPointer.SetBool("Appear", true);
            }
            
            if (theDM.GetCurrentSentenceNumber() == 3)
            {
//                aniPointer.SetBool("Drag", true);
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
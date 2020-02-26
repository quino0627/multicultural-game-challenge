using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectionDialogue : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;

    private DialogueManager theDM;

    public Animator aniCoin;
    public Animator aniPointer;

    private int? tmpCount = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        theDM = FindObjectOfType<DialogueManager>();
        
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
            if (theDM.GetCurrentSentenceNumber() == 0)
            {

                
            }
            if (theDM.GetCurrentSentenceNumber() == 1)
            {
                
                aniPointer.SetBool("Appear", true);
                
            }

            if (theDM.GetCurrentSentenceNumber() == 2)
            {
                // 코인 나타남
                aniCoin.SetBool("Appear", true);
            }
            
            if (theDM.GetCurrentSentenceNumber() == 3)
            {
            }
            
            if (theDM.GetCurrentSentenceNumber() == 4)
            {
                // 코인 사라짐
                aniCoin.SetBool("Appear", false);
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

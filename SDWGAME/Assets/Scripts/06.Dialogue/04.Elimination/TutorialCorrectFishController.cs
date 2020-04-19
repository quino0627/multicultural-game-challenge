using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCorrectFishController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!GameObject.FindObjectOfType<TutorialEliminationManager>().enableFish) return;
        SoundManager.Instance.Play_ClickedCorrectAnswer();
        GameObject.FindObjectOfType<TutorialEliminationManager>().isClickedCorrectFish = true;
    }
}

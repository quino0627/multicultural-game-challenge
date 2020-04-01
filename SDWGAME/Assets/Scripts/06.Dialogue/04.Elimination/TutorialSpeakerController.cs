using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpeakerController : MonoBehaviour
{
    public int choiceNumber;
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
        SoundManager.Instance.Play_TutorialEliminationChoices(choiceNumber);
        GameObject.FindObjectOfType<TutorialEliminationManager>().isClickedSpeaker = true;
    }
}

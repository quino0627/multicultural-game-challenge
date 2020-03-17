using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatSoundBubble : MonoBehaviour
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
        if (Time.timeScale == 0) return;
        transform.parent.GetComponent<SeahorseRightController>().ClickBubble();
        GameObject.FindObjectOfType<TutorialAlternativeManager>().clickedSpeechBubble = true;
        
    }
}

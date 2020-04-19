using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRepeatSoundBubble : MonoBehaviour
{
    private Boolean isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnMouseUp()
    {
        if (!GameObject.FindObjectOfType<TutorialAlternativeManager>().enableSpeechBubbleClick) return;
        if (isPlaying) return;
        
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        isPlaying = true;
        yield return new WaitForSeconds(SoundManager.Instance.Play_AlternativeTargetSound()+1f);
        GameObject.FindObjectOfType<TutorialAlternativeManager>().clickedSpeechBubble = true;
        isPlaying = false;
    }
}

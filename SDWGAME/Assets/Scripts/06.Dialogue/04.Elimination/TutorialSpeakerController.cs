using System;
using System.Collections;
using UnityEngine;

// 탈락과제 튜토리얼의 물고기 보기 위에 있는 하트모양 스피터 
public class TutorialSpeakerController : MonoBehaviour
{
    
    public int choiceNumber;
    
    
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
        if (isPlaying) return;
        
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        isPlaying = true;
        yield return new WaitForSeconds(SoundManager.Instance.Play_TutorialEliminationChoices(choiceNumber) + 1f);
        GameObject.FindObjectOfType<TutorialEliminationManager>().isClickedSpeaker = true;
        isPlaying = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCorrectBubble : MonoBehaviour
{
    // 버블이 여러번 클릭되는 것을 방지한다.
    // 기본값은 false, 정답일 경우 한번 클릭된 이후에는 true로
    private bool preventSeveralTouch = false;
    
    // Start is called before the first frame update
    void Start()
    {
        preventSeveralTouch = false;
    }
    
    private void OnEnable()
    {
        preventSeveralTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!GameObject.FindObjectOfType<TutorialAlternativeManager>().enableBubbleClick) return;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(!preventSeveralTouch)
        {
            BubbleClicked();
        }
    }
    
    public void BubbleClicked()
    {
        SoundManager.Instance.Play_ClickedCorrectAnswer();
        preventSeveralTouch = true;
        GameObject.FindObjectOfType<TutorialAlternativeManager>().clickedCorrectAnswer = true;
    }
}

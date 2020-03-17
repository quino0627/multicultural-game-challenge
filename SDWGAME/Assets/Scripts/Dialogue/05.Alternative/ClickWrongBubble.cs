using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickWrongBubble : MonoBehaviour
{
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
//        Debug.Log(!IsPointerOverUIObject()); //true
//        if (IsPointerOverUIObject()) //false
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
        SoundManager.Instance.Play_ClickedWrongAnswer();
        

    }
}

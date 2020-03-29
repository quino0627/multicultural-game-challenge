using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCorrectBarrel : MonoBehaviour
{
    
    // 배럴이 여러번 클릭되는 것을 방지한다.
    // 기본값은 false, 정답일 경우 한번 클릭된 이후에는 true로 
    private bool preventSeveralTouch = false;
    // Start is called before the first frame update
    void Start()
    {
        preventSeveralTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnEnable()
    {
        preventSeveralTouch = false;
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
            BarrelClicked();
        }
        
    }

    public void BarrelClicked()
    {
        SoundManager.Instance.Play_ClickedCorrectAnswer();
        preventSeveralTouch = true;
        GameObject.FindObjectOfType<TutorialDetectionManager>().clickedCorrectAnswer = true;
        Animator aniCoin = transform.Find("Coin").gameObject.GetComponent<Animator>();
        aniCoin.SetBool("Appear", true);
    }
}

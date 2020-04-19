using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickWrongBarrel : MonoBehaviour
{
    // 배럴이 여러번 클릭되는 것을 방지한다.
    // 기본값은 false, 정답일 경우 한번 클릭된 이후에는 true로 
    private bool preventSeveralTouch = false;

    private Animator aniTrash;
    // Start is called before the first frame update
    void Start()
    {
        preventSeveralTouch = false;
        aniTrash = transform.Find("Trash").gameObject.GetComponent<Animator>();
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
        if (!GameObject.FindObjectOfType<TutorialDetectionManager>().enableBarrelClick)
        {
            return;
        }
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
        SoundManager.Instance.Play_ClickedWrongAnswer();
        preventSeveralTouch = true;
        aniTrash.SetTrigger("Appear");
        Invoke(nameof(DisappearTrashAfterSeconds), 1.0f);

    }
    
    private void DisappearTrashAfterSeconds()
    {
        aniTrash.SetTrigger("Disappear");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** 대치과제 튜토리얼에서 원제시어가 담긴 박스에 장착됨.
 * 클릭시 샘플 사운드 발사 및 플래그를 true로 변경하여 다음 다이얼로그를 표시
 *
 * @author: 송동욱
 */
public class TutorialOriginalWordBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log("Clicked TutorialOriginWordBox");
        GameObject.FindObjectOfType<TutorialAlternativeManager>().clickedOriginalWordBox = true;
        SoundManager.Instance.Play_EliminationTutorialSampleSound();
    }
}

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
        yield return new WaitForSeconds(SoundManager.Instance.Play_AlternativeOriginSound()+1f);
        GameObject.FindObjectOfType<TutorialAlternativeManager>().clickedOriginalWordBox = true;
        isPlaying = false;
    }
}

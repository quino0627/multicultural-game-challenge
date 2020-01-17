using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float FadeTime = 2f; //Fade효과 재생시간
    
    private Image fadeImg;
    
    private float start;
    
    private float end;
    
    private float time = 0f;
    
    private bool isPlaying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        fadeImg = GetComponent<Image>();
        InStartFadeAnim();
    }

    /*public void OutStartFadeAnim()
    {
        if (isPlaying) //중복재생방지
        {
            return;
        }

        start = 1f;
        end = 0f;
        StartCoroutine("FadeOutPlay"); //코루틴 실행
    }*/
    

    public void InStartFadeAnim()
    {
        if (isPlaying)
        {
            return;
        }

        start = 1f;
        end = 0f;
        StartCoroutine("FadeInPlay");

    }

    IEnumerator FadeInPlay()
    {
        isPlaying = true;

        Color fadeColor = fadeImg.color;
        time = 0f;
        fadeColor.a = Mathf.Lerp(start, end, time);

        while (fadeColor.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            fadeColor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadeColor;
            yield return null;
        }

        isPlaying = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

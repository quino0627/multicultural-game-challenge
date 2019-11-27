using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingScore : MonoBehaviour {

    public float speed = 0.05f, time = 1.0f;
    Text this_text;
    private void Awake()
    {
        this_text = this.gameObject.GetComponent<Text>();
    }

    private void Start()
    {
        
    }

    void OnDisable()
    {
        
    }

    void OnEnable()
    {
        this_text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        FadeOut(time);
    }

    // Update is called once per frame 
    void Update()
    {
        
    }

    public void FadeIn(float fadeOutTime, System.Action nextEvent = null)
    {
        StartCoroutine(CoFadeIn(fadeOutTime, nextEvent));
    }

    public void FadeOut(float fadeOutTime, System.Action nextEvent = null)
    {
        StartCoroutine(CoFadeOut(fadeOutTime, nextEvent));
    }

    // 투명 -> 불투명
    IEnumerator CoFadeIn(float fadeOutTime, System.Action nextEvent = null)
    {
        Text sr = this.gameObject.GetComponent<Text>();
        Color tempColor = sr.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            sr.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }

        sr.color = tempColor;
        if (nextEvent != null) nextEvent();
    }

    // 불투명 -> 투명
    IEnumerator CoFadeOut(float fadeOutTime, System.Action nextEvent = null)
    {
        Text sr = this.gameObject.GetComponent<Text>();
        Color tempColor = sr.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeOutTime;
            sr.color = tempColor;
            
            this.transform.Translate(new Vector3(0, speed, 0));  //이동

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }
        sr.color = tempColor;
        this.gameObject.SetActive(false);
        if (nextEvent != null) nextEvent();
    }

}

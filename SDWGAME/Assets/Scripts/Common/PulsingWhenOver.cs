using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingWhenOver : MonoBehaviour
{

    private bool coroutineAllowed;
    
    // Start is called before the first frame update
    void Start()
    {
        coroutineAllowed = true;

    }

    private void OnMouseOver()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(onMousePulsing());
        }
    }

    private IEnumerator onMousePulsing()
    {
        coroutineAllowed = false;
        
        for (float i = 0f; i <=1f; i+=0.1f)
        {
            transform.localScale = new Vector2((Mathf.Lerp(transform.localScale.x, transform.localScale.x+0.005f, Mathf.SmoothStep(0f,1f,i))),(Mathf.Lerp(transform.localScale.y, transform.localScale.y+0.005f, Mathf.SmoothStep(0f,1f,i))));
            yield return new WaitForSeconds(0.02f);
        }
        
        for (float i = 0f; i <=1f; i+=0.1f)
        {
            transform.localScale = new Vector2((Mathf.Lerp(transform.localScale.x, transform.localScale.x-0.005f, Mathf.SmoothStep(0f,1f,i))),(Mathf.Lerp(transform.localScale.y, transform.localScale.y-0.005f, Mathf.SmoothStep(0f,1f,i))));
            yield return new WaitForSeconds(0.02f);
        }

        coroutineAllowed = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

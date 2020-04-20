using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;

public class shinyEffectAnimationPlay : MonoBehaviour
{
    public bool bTriggerShine;

    public bool bShine;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    IEnumerator ShinePlayLoop(ShinyEffectForUGUI shinyEffectForUgui)
    {
        while (bShine)
        {
            shinyEffectForUgui.Play(2, AnimatorUpdateMode.Normal);
            yield return new WaitForSeconds(2.5f);

        }
            
         
    }

    private void Update()
    {
        if (bTriggerShine)
        {
            bShine = true;
            StartCoroutine(ShinePlayLoop(GetComponent<ShinyEffectForUGUI>()));
            bTriggerShine = false;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverResponse : MonoBehaviour
{
    private Vector3 originalScale;

    public Vector3 newScale;

    public bool bLockHoverEffect;

    public bool bElimination;

    public GameObject childFishGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
        if (bElimination)
        {
            originalScale = new Vector3(1,1,1);
        }
        else
        {
            originalScale = transform.localScale;
        }
    }

    private void OnMouseEnter()
    {
        if (bElimination)
        {
            childFishGameObject.transform.localScale = newScale;
            return;
        }
        if (!bLockHoverEffect)
        {
            transform.localScale = newScale;
        }

        
    }

    private void OnMouseDown()
    {
        if (bElimination)
        {
            childFishGameObject.transform.localScale = originalScale;
            return;
        }
        if (!bLockHoverEffect)
        {
            transform.localScale = originalScale;
        }
        
    }

    private void OnMouseExit()
    {
        if (bElimination)
        {
            childFishGameObject.transform.localScale = originalScale;
            return;
        }
        if (!bLockHoverEffect)
        {
            //Debug.Log("OnMouseExit");
            transform.localScale = originalScale;
        }
        
    }

    
}

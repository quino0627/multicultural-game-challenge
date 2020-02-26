using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    private Vector3 originalScale = new Vector3(0.59293f, 0.59293f, 1);
    private Vector3 clickedScale = new Vector3(0.52854f, 0.52854f, 1);

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        gameObject.GetComponent<RectTransform>().localScale = originalScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Input.mousePosition;
        gameObject.GetComponent<RectTransform>().transform.position = cursorPos;

        if (Input.GetMouseButton(0))
        {
            gameObject.GetComponent<RectTransform>().localScale = clickedScale;
        }

        if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<RectTransform>().localScale = originalScale;
        }
        
        
    }

}

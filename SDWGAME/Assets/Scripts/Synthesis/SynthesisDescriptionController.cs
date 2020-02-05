using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SynthesisDescriptionController : MonoBehaviour
{
    private GameObject DescriptionText;
    private GameObject ParentCrab;
    private string description_text;

    private Vector3 OriginCrabScale;
    private Vector3 AfterCrabScale;
    private Vector3 OriginMsgScale;
    private float AfterMsgScaleXValue;

    private bool isFirstUpdate;
    
    private void Awake()
    {
        isFirstUpdate = true;
        ParentCrab = this.transform.parent.gameObject;
        Debug.Log(ParentCrab.transform.localScale);
        OriginCrabScale = ParentCrab.transform.localScale;
        OriginMsgScale = this.transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get Parent Object which is Crab\
        DescriptionText = transform.Find("DescriptionText").gameObject;
        DefaultDescription();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstUpdate)
        {
            AfterCrabScale = ParentCrab.transform.localScale;
            AfterMsgScaleXValue = (OriginCrabScale.x * OriginMsgScale.x / AfterCrabScale.x);
            Debug.Log($"OriginCrabScale.x: {OriginCrabScale.x}");
            Debug.Log($"AfterCrabScale.x: {AfterCrabScale.x}");
            Debug.Log($"OriginMsgScale.x: {OriginMsgScale.x}");
            Debug.Log($"AfterMsgScaleXValue: {AfterMsgScaleXValue}");
            this.transform.localScale = new Vector3(AfterMsgScaleXValue, AfterMsgScaleXValue, 1);
            
            isFirstUpdate = false;
            
        }
        var tmp = 0;
        this.DescriptionText.GetComponent<TextMeshPro>().text = description_text;
    }

    public void CorrectOneWord()
    {
        description_text = "맞아!";
        Invoke("DefaultDescription", 2.0f);
    }

    public void CorrectAnswer()
    {
        description_text = "정답이야. \n 고마워!";
        
    }

    public void WrongAnswer()
    {
        description_text = "틀렸어. \n 다시 골라볼까?";
        Invoke("DefaultDescription", 2.0f);
    }

    public void DefaultDescription()
    {
        description_text = "해파리를 옮겨 \n 소리나는 단어를\n 만들어줘!";
    }
}

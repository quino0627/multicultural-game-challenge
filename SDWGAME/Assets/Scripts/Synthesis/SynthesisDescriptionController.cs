using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SynthesisDescriptionController : MonoBehaviour
{
    private GameObject DescriptionText;

    private string description_text;
    
    // Start is called before the first frame update
    void Start()
    {
        DescriptionText = transform.Find("DescriptionText").gameObject;
        DefaultDescription();
    }

    // Update is called once per frame
    void Update()
    {
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

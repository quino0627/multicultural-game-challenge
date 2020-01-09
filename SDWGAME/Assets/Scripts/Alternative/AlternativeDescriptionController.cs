using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlternativeDescriptionController : MonoBehaviour
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

    public void CorrectAnswer()
    {
        description_text = "정답이야. \n 축하해!";
    }

    public void WrongAnswer()
    {
        description_text = "틀렸어. \n 다시 골라보자.";
    }

    public void DefaultDescription()
    {
        description_text = "왼쪽 글자를 \n 소리나는 글자로 \n 바꿔보자!";
    }
}

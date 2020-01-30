using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetectionDescriptionController : MonoBehaviour
{
    private GameObject DescriptionText;

    private string description_text;
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
        Invoke("DefaultDescription", 2.0f);
    }

    public void WrongAnswer()
    {
        description_text = "틀렸어. \n 다시 골라보자.";
        Invoke("DefaultDescription", 2.0f);
    }

    public void DefaultDescription()
    {
        description_text = "소리나는\n글자를\n찾아보자!";
    }
}

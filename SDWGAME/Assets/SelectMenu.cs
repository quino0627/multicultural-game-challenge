using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    public GraphicRaycaster GR;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickTest()
    {
        Debug.Log("Clicked");
    }

    public void Detection()
    {
        Debug.Log("Clicked");
        GR.enabled = false;
        SceneManager.LoadScene("DetectionGame");
        
    }

    public void Synthesis()
    { Debug.Log("Clicked");
        GR.enabled = false;
        SceneManager.LoadScene("CrabLevel1");
        
    }

    public void Elimination()
    { Debug.Log("Clicked");
        GR.enabled = false;
        SceneManager.LoadScene("EliminationEscape");
        

    }

    public void Alternative()
    { Debug.Log("Clicked");
        GR.enabled = false;
        SceneManager.LoadScene("AlternativeGame");
        
    }

    
}

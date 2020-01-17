using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button Easy;

    public Button Normal;

    public Button Hard;
    // Start is called before the first frame update
    void Start()
    {
        Normal.interactable = false;
        Hard.interactable = false;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

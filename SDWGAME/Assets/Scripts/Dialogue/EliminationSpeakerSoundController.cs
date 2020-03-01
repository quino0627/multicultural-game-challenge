using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationSpeakerSoundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        SpeakerClicked();
    }

    public void SpeakerClicked()
    {
        //string s = $"Sounds/Detection/{script.data.sheets[script.level].list[script.stageIndex].오답음성1}"
        SoundManager.Instance.Play_EliminationTutorialSampleSound();
        
    }
}

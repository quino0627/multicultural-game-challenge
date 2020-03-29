using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueV2
{
    [TextArea(1, 3)] public string[] sentences;
    public Boolean[] flags;
    public Sprite dialogueWindow;
    

}
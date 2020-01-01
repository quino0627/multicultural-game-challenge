using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : ScriptableObject
{
    public List<Data> data = new List<Data> ();
    
    [System.SerializableAttribute]
    public class Data
    {
        public string target;
        public string corrAns1;
        public string corrAns2;
        public string corrAns3;
        public string corrAns4;
        public string choice1;
        public string choice2;
        public string choice3;
        public string choice4;
    }
}

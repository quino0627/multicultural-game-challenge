using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;
class people
{
    public string name;
    public int age;
   // public Dictionary<string,int> userMaxLevel;
    
    public people(string name, int age)
    {
        this.name = name;
        this.age = age;
        
    }
}

public class PracticeManager : MonoBehaviour
{
    //1. 한개
    people data = new people("철수", 15);

//2. List
    //private List<people> data = new List<people>();
    
    //3.Dictionary
    //Dictionary<string,people> data = new Dictionary<string, people>();

    public Text tx;
    //List<people> data = new List<people>();
    
    private void Start()
    {
        //string jdata = JsonConvert.SerializeObject(p1);
        
        /*data.Add(new people("철수", 20));
        data.Add(new people("유리", 18));
        data.Add(new people("맹구", 23));
        string jdata = JsonConvert.SerializeObject(data);*/
        
        /*data["p1"] = new people("철수",15);
        data["p2"] = new people("유리", 23);
        data["p3"] = new people("맹구", 45);*/
        //string jdata = JsonConvert.SerializeObject(data);
        
        //print(jdata);
    }

    public void Save()
    {
       // string jdata = JsonConvert.SerializeObject(data);
      //  File.WriteAllText(Application.dataPath + "/Practice.json", jdata);
    }

    public void Load()
    {
      //  string jdata = File.ReadAllText(Application.dataPath + "/Practice.json");
     //   tx.text = jdata;
        //data = JsonConvert.DeserializeObject<List<people>>(jdata);
        
        //print(data[1].name);
    }
}

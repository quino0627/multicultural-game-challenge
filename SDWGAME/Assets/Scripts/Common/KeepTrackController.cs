using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.SceneManagement;

class ConclusionData
{
    public string id;
    public DateTime startTime;
    public DateTime endTime;
    public Dictionary<string,int> userMaxLevel; // 피험자가 과반 이상 정답을 맞춘 최고단계
    public int perfection;//전체 평균 정답률
    public int minorPerfection;//세부 자극별 평균 정답률
    public int avgResponseDuration; // 평균 반응 시간
    public int maxResponseDuration; // 최고 반응 시간
    public int IES; //평균 반응시간/정확도

    
}

public class KeepTrackController : MonoBehaviour
{
    
    ConclusionData data = new ConclusionData();

    public int[] tmpLevel;
    /*public DetectionQuizManager DQM;
    public SpreadChoices SC;
    public FishShowAnswer FSA;
    public AlternativeQuizManager AQM;*/
    public bool[] isLevelOpen;
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        isLevelOpen=new bool[3];
        Load();
        //Debug.Log("Loaded");
       Debug.Log(isLevelOpen.Length);
        //Debug.Log(data);
        //string jdata = JsonConvert.SerializeObject(data);
        //File.WriteAllText(Application.dataPath + "/Conclusion.json", jdata);

        if (data.userMaxLevel["Alternative"] > 1 &&
            data.userMaxLevel["Synthesis"] > 1 &&
            data.userMaxLevel["Elimination"] > 1 &&
            data.userMaxLevel["Detection"] > 1)
        {
            isLevelOpen[0] = isLevelOpen[1] = isLevelOpen[2] = true;
        }
        else if (data.userMaxLevel["Alternative"] > 0 &&
                   data.userMaxLevel["Synthesis"] > 0 &&
                   data.userMaxLevel["Elimination"] > 0 &&
                   data.userMaxLevel["Detection"] > 0)
        {
            isLevelOpen[0] = isLevelOpen[1] = true;
            isLevelOpen[2] = false;
        }
        else
        {
            isLevelOpen[0] = true;
        }
    }

    public void Save()
    {
        if (data.userMaxLevel["Alternative"] < tmpLevel[3])
        {
            data.userMaxLevel["Alternative"] = tmpLevel[3];
        }
        
        if (data.userMaxLevel["Synthesis"] < tmpLevel[1])
        {
            data.userMaxLevel["Synthesis"] = tmpLevel[1];
        }
        if (data.userMaxLevel["Elimination"] < tmpLevel[2])
        {
            data.userMaxLevel["Elimination"] = tmpLevel[2];
        }
        if (data.userMaxLevel["Detection"] < tmpLevel[0])
        {
            data.userMaxLevel["Detection"] =tmpLevel[0];
        }
        
        string jdata = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath+"/Conclusion.json",jdata);
        
        Debug.Log("Save"+"\n"+jdata);
    
    }

    public void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Conclusion.json");
        data = JsonConvert.DeserializeObject<ConclusionData>(jdata);
        print("data");
    }

}

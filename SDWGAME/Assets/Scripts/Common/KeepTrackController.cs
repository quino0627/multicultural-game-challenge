using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.SceneManagement;

class ConclusionData
{
    public string id;
    public DateTime startTime;
    public DateTime endTime;
    public Dictionary<string, int> userMaxLevel; // 피험자가 과반 이상 정답을 맞춘 최고단계
    public Dictionary<string, int> userMaxStage;
    public int perfection;//전체 평균 정답률
    public int minorPerfection;//세부 자극별 평균 정답률
    public int avgResponseDuration; // 평균 반응 시간
    public int maxResponseDuration; // 최고 반응 시간
    public int IES; //평균 반응시간/정확도

    
}

public class KeepTrackController : MonoBehaviour
{
    
    ConclusionData data = new ConclusionData();
    public int chosenLevel;
    public int[] tmpLevel;

    public int[] tmpStage;
    /*public DetectionQuizManager DQM;
    public SpreadChoices SC;
    public FishShowAnswer FSA;
    public AlternativeQuizManager AQM;*/
    public bool[] isLevelOpen;
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        isLevelOpen=new bool[3];
        tmpLevel = new int[4];
        tmpStage = new int[4];
        Load();
        
        tmpStage[0] = data.userMaxStage["Detection"];
        tmpStage[1] = data.userMaxStage["Synthesis"];
        tmpStage[2] = data.userMaxStage["Elimination"];
        tmpStage[3] = data.userMaxStage["Alternative"];

        tmpLevel[0] = data.userMaxLevel["Detection"];
        tmpLevel[1] = data.userMaxLevel["Synthesis"];
        tmpLevel[2] = data.userMaxLevel["Elimination"];
        tmpLevel[3] = data.userMaxLevel["Alternative"];
    }

    public void Update()
    {
       
        
        if (tmpLevel[0] > 1 &&
            tmpLevel[1] > 1 &&
            tmpLevel[2] > 1 &&
            tmpLevel[3] > 1)
        {
            isLevelOpen[0] = isLevelOpen[1] = isLevelOpen[2] = true;
        }
        else if (tmpLevel[0] > 0 &&
                 tmpLevel[1] > 0 &&
                 tmpLevel[2] > 0 &&
                 tmpLevel[3] > 0)
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
            data.userMaxLevel["Detection"] = tmpLevel[0];
        }

        if (data.userMaxStage["Detection"] < tmpStage[0])
        {
            data.userMaxStage["Detection"] = tmpStage[0];
        }
        if (data.userMaxStage["Synthesis"] < tmpStage[1])
        {
            data.userMaxStage["Synthesis"] = tmpStage[1];
        }
        if (data.userMaxStage["Elimination"] < tmpStage[2])
        {
            data.userMaxStage["Elimination"] = tmpStage[2];
        }
        if (data.userMaxStage["Alternative"] < tmpStage[3])
        {
            data.userMaxStage["Alternative"] = tmpStage[3];
        }
        
        string jdata = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath+"/Conclusion.json",jdata);
        
        //Debug.Log("Save"+"\n"+jdata);
    
    }

    public void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Conclusion.json");
        data = JsonConvert.DeserializeObject<ConclusionData>(jdata);
        //print("data");
    }

    public void LoadSelectMenu()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Conclusion.json");
        data = JsonConvert.DeserializeObject<ConclusionData>(jdata);
        
    }
}

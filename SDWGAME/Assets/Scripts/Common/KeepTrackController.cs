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

public class ConclusionData
{
    //public string id;
    public DateTime startTime;
    public DateTime endTime;
    public Dictionary<string, int> userMaxLevel; // 피험자가 과반 이상 정답을 맞춘 최고단계
    public Dictionary<string, int> userMaxStage;
    public int perfection; //전체 평균 정답률이라기보다 rate로 해야할듯
    public Dictionary<string, int> minorPerfection;//세부 자극별 평균 정답률
    public int avgResponseDuration; // 평균 반응 시간
    public int maxResponseDuration; // 최고 반응 시간
    public int IES; //평균 반응시간/정확도
    //public Dictionary<string,Dictionary<int,int>> totalClickCount; //"Detection":{"level":totalCilckedCountNumber},{}
    //public Dictionary<string,Dictionary<int,int>> totalCorrectCount;
    public Dictionary<string,Dictionary<int,int>> totalRate;
    public int[] triedCnt;

}

public class KeepTrackController : MonoBehaviour
{
    public string currId;
    public Dictionary<string, ConclusionData> allData;
    public ConclusionData data = new ConclusionData();
    public int chosenLevel;
    public int[] tmpLevel;
    public int[] tmpStage;
    public int[,] tmpStars;

    public int[] tmpTriedCnt;
    /*public DetectionQuizManager DQM;
    public SpreadChoices SC;
    public FishShowAnswer FSA;
    public AlternativeQuizManager AQM;*/
    public bool[] isLevelOpen;
    public bool isIdLoaded;
    private static KeepTrackController instance;

    // Public static reference that can be accesd from anywhere
    public static KeepTrackController Instance
    {
        get
        {
            // Check if instance has not been set yet and set it it is not set already
            // This takes place only on the first time usage of this reference
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<KeepTrackController>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
    
    void Awake()
    {
        Debug.Log("AWAKE");
        if (instance == null)
        {
            // Make the current instance as the singleton
            instance = this;

            // Make it persistent  
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("IN FIRST ELSE");
            // If more than one singleton exists in the scene find the existing reference from the scene and destroy it
            if (this != instance)
            {
                Debug.Log("IN SECONDE ELSE");
                Destroy(this.gameObject);
            }
        }
    }

    public void Start()
    {
        isLevelOpen = new bool[3];
        tmpLevel = new int[4];
        tmpStage = new int[4];
        tmpStars = new int[4, 3];
    }
    
    public void Update()
    {
        //To open next Level
        if (isIdLoaded)
        {
            if (tmpStars[0,0] < 3 ||
                tmpStars[1,0] < 3 ||
                tmpStars[2,0] < 3 ||
                tmpStars[3,0] < 3)
            {
                //초급
                isLevelOpen[0] = true;
            }
            else if (tmpStars[0,1] < 3 ||
                     tmpStars[1,1] < 3 ||
                     tmpStars[2,1] < 3 ||
                     tmpStars[3,1] < 3)
            {
                //중급
                isLevelOpen[0] = isLevelOpen[1] = true;
                isLevelOpen[2] = false;
            }
            else
            {
                //고급
                isLevelOpen[0] = isLevelOpen[1] = isLevelOpen[2] = true;
            }
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
        
        for (int i = 0; i < 3; i++)
        {
            data.totalRate["Detection"][i] = tmpStars[0, i];
            data.totalRate["Synthesis"][i] = tmpStars[1, i];
            data.totalRate["Elimination"][i] = tmpStars[2, i] ;
            data.totalRate["Alternative"][i] = tmpStars[3, i] ;
        }

        for (int i = 0; i < 4; i++)
        {
            data.triedCnt[i] = tmpTriedCnt[i];
        }
        
        allData[currId] = data; 
        string jdata = JsonConvert.SerializeObject(allData);
        File.WriteAllText(Application.dataPath+"/Conclusion.json",jdata);
        
        //Debug.Log("Save"+"\n"+jdata);
        
    }
    

    public void LoadTmpData()
    {
        tmpStage[0] = data.userMaxStage["Detection"];
        tmpStage[1] = data.userMaxStage["Synthesis"];
        tmpStage[2] = data.userMaxStage["Elimination"];
        tmpStage[3] = data.userMaxStage["Alternative"];

        tmpLevel[0] = data.userMaxLevel["Detection"];
        tmpLevel[1] = data.userMaxLevel["Synthesis"];
        tmpLevel[2] = data.userMaxLevel["Elimination"];
        tmpLevel[3] = data.userMaxLevel["Alternative"];

        for (int i = 0; i < 3; i++)
        {
            tmpStars[0, i] = data.totalRate["Detection"][i];
            tmpStars[1, i] = data.totalRate["Synthesis"][i];
            tmpStars[2, i] = data.totalRate["Elimination"][i];
            tmpStars[3, i] = data.totalRate["Alternative"][i];
        }
    }
    

    public void InitStageData()
    {
        tmpStage = new int[4];
    }

    public void GetDataWithID(string id)
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Conclusion.json");
        allData = JsonConvert.DeserializeObject<Dictionary<string,ConclusionData>>(jdata);

        if (allData[id] == null)
        {
            //창 띄우기
            Debug.Log("유효한 id가 아닙니다");
        }
        else
        {
            currId = id;
            data = allData[id];
            isIdLoaded = true;
        }
    }
}

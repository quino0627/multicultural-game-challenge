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
    //public string id;
    public DateTime startTime;
    public DateTime endTime;
    public Dictionary<string, int> userMaxLevel; // 피험자가 과반 이상 정답을 맞춘 최고단계
    public Dictionary<string, int> userMaxStage;
    public int perfection; //전체 평균 정답률이라기보다 rate로 해야할듯
    public Dictionary<string, int> minorPerfection; //세부 자극별 평균 정답률
    public int avgResponseDuration; // 평균 반응 시간
    public int maxResponseDuration; // 최고 반응 시간
    public int IES; //평균 반응시간/정확도


    //public Dictionary<string,Dictionary<int,int>> totalClickCount; //"Detection":{"level":totalCilckedCountNumber},{}
    //public Dictionary<string,Dictionary<int,int>> totalCorrectCount;
    public Dictionary<string, Dictionary<int, int>> totalRate;

    //public int[,] triedCnt;
    public Dictionary<string, int[,]> triedCnt; // 게임이름, level
    //public Dictionary<string, int[,]> clickCnt; 

    public ConclusionData()
    {
        startTime = new DateTime();
        endTime = new DateTime();
        userMaxLevel = new Dictionary<string, int>()
        {
            {"Alternative", 0},
            {"Elimination", 0},
            {"Synthesis", 0},
            {"Detection", 0}
        };
        userMaxStage = new Dictionary<string, int>()
        {
            {"Alternative", 0},
            {"Elimination", 0},
            {"Synthesis", 0},
            {"Detection", 0}
        };
        perfection = 0;
        minorPerfection = new Dictionary<string, int>()
        {
            {"Alternative", 0},
            {"Elimination", 0},
            {"Synthesis", 0},
            {"Detection", 0}
        };
        avgResponseDuration = 0;
        maxResponseDuration = 0;
        IES = 0;
        totalRate = new Dictionary<string, Dictionary<int, int>>()
        {
            {"Detection", new Dictionary<int, int> {{0, 0}, {1, 0}, {2, 0}}},
            {"Synthesis", new Dictionary<int, int> {{0, 0}, {1, 0}, {2, 0}}},
            {"Elimination", new Dictionary<int, int> {{0, 0}, {1, 0}, {2, 0}}},
            {"Alternative", new Dictionary<int, int> {{0, 0}, {1, 0}, {2, 0}}}
        };
        //triedCnt = new int[4, 30];
        triedCnt = new Dictionary<string, int[,]>()
        {
            {"Detection", new int[3, 30]},
            {"Synthesis", new int[3, 30]},
            {"Elimination", new int[3, 30]},
            {"Alternative", new int[3, 30]}
        };
    }
}

public class KeepTrackController : MonoBehaviour
{
    private GameObject StageStorage;
    private DataController StageStorageScript;

    public string currId;
    private Dictionary<string, ConclusionData> allData;
    private ConclusionData data;
    public int chosenLevel;
    public int[] tmpLevel;
    public int[] tmpMaxLevel;
    public int[] tmpStage;
    public int[,] tmpStars;

    public Dictionary<string, int[,]> tmpTriedCnt;
    //public Dictionary<string, int[,]> tmpTriedCount;

    /*public DetectionQuizManager DQM;
    public SpreadChoices SC;
    public FishShowAnswer FSA;
    public AlternativeQuizManager AQM;*/
    public bool[] isLevelOpen;
    public bool bLogin;
    private static KeepTrackController instance;
    private string conclusionPath;

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
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
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
        StageStorage = GameObject.Find("StageStorage");
        StageStorageScript = StageStorage.GetComponent<DataController>();
        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.streamingAssetsPath);
        conclusionPath = Path.Combine(Application.streamingAssetsPath + "/Conclusion.json");
        Debug.Log("ConclusionPath: " + conclusionPath);

        bLogin = false;
        isLevelOpen = new bool[3];
        tmpLevel = new int[4];
        tmpMaxLevel = new int[4];
        tmpStage = new int[4];
        tmpStars = new int[4, 3];
        tmpTriedCnt = new Dictionary<string, int[,]>()
        {
            {"Detection", new int[3, 30]},
            {"Synthesis", new int[3, 30]},
            {"Elimination", new int[3, 30]},
            {"Alternative", new int[3, 30]}
        };


        //tmp data test 생성용
        /*allData = new Dictionary<string, ConclusionData>();
        allData["please"] = new ConclusionData(); 
        string jdata = JsonConvert.SerializeObject(allData, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Conclusion.json", jdata);
        */


        //진짜 코드
        //var jdataTextAsset = (TextAsset) Resources.Load("Conclusion.json");


        if (!File.Exists(conclusionPath))
        {
            //Directory.CreateDirectory(Application.persistentDataPath + "/Conclusion.json");
            //File.Create(conclusionPath);
            allData = new Dictionary<string, ConclusionData>();
            allData.Add("initial", new ConclusionData());
            string tmpJdata = JsonConvert.SerializeObject(allData, Formatting.Indented);
            //File.WriteAllText(conclusionPath, tmpJdata);
            WriteFile(conclusionPath, tmpJdata);
        }

        string jdata = "";
        jdata = File.ReadAllText(conclusionPath);
        //ReadFile(conclusionPath, jdata);
        allData = JsonConvert.DeserializeObject<Dictionary<string, ConclusionData>>(jdata);
        Debug.Log("alldata: "+JsonConvert.SerializeObject(allData,Formatting.Indented));
        


        //data = new ConclusionData();
        //allData = new Dictionary<string, ConclusionData>();
    }

    private void WriteFile(string filePath, string json)
    {
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        }

        //AssetDataBase.Refresh();
    }

    private string ReadFile(string filePath, string json)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            json = reader.ReadToEnd();
        }

        return json;
    }

    public bool makeNewId(string id)
    {
        if (!allData.ContainsKey(id))
        {
            ConclusionData tmp = new ConclusionData();
            //allData.Add(id,tmp);
            allData[id] = tmp;
            string jdata = JsonConvert.SerializeObject(allData, Formatting.Indented);
            //File.WriteAllText(conclusionPath, jdata);
            WriteFile(conclusionPath, jdata);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void deleteTotalInfoOfCurrentId(string id)
    {
        if (allData.Remove(id))
        {
            Debug.Log("removed");
        }
        else
        {
            Debug.Log("No such Id");
        }

        string jdata = JsonConvert.SerializeObject(allData, Formatting.Indented);
        // File.WriteAllText(conclusionPath, jdata);
        WriteFile(conclusionPath, jdata);
    }

    public void Save()
    {
        data.userMaxLevel["Alternative"] = tmpMaxLevel[3];
        data.userMaxLevel["Synthesis"] = tmpMaxLevel[1];
        data.userMaxLevel["Elimination"] = tmpMaxLevel[2];
        data.userMaxLevel["Detection"] = tmpMaxLevel[0];


        /*if (data.userMaxStage["Detection"] < tmpStage[0])
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
        }*/

        //perfection
        //minorPerfection 4게임 다
        //??stage실패인지 정담을 잘못 고를때마다 올라가는건지
        //??몇번째 시도를 써야하는지

        //avgResponseDuration
        // 모든 게임의?

        //maxResonponseDuration
        // 가장 오래 걸린시간?

        //IES = avgResponseTime/Perfection??


        for (int i = 0; i < 3; i++)
        {
            data.totalRate["Detection"][i] = tmpStars[0, i];
            data.totalRate["Synthesis"][i] = tmpStars[1, i];
            data.totalRate["Elimination"][i] = tmpStars[2, i];
            data.totalRate["Alternative"][i] = tmpStars[3, i];
        }

        data.triedCnt["Detection"] = tmpTriedCnt["Detection"];
        data.triedCnt["Synthesis"] = tmpTriedCnt["Synthesis"];
        data.triedCnt["Elimination"] = tmpTriedCnt["Elimination"];
        data.triedCnt["Alternative"] = tmpTriedCnt["Alternative"];

        allData[currId] = data;
        string jdata = JsonConvert.SerializeObject(allData, Formatting.Indented);
        //File.WriteAllText(conclusionPath, jdata);
        WriteFile(conclusionPath, jdata);

        //Debug.Log("Save" + "\n" + jdata);
    }


    public void LoadTmpData()
    {
        tmpStage[0] = data.userMaxStage["Detection"];
        tmpStage[1] = data.userMaxStage["Synthesis"];
        tmpStage[2] = data.userMaxStage["Elimination"];
        tmpStage[3] = data.userMaxStage["Alternative"];

        tmpMaxLevel[0] = tmpLevel[0] = data.userMaxLevel["Detection"];
        tmpMaxLevel[1] = tmpLevel[1] = data.userMaxLevel["Synthesis"];
        tmpMaxLevel[2] = tmpLevel[2] = data.userMaxLevel["Elimination"];
        tmpMaxLevel[3] = tmpLevel[3] = data.userMaxLevel["Alternative"];

        for (int i = 0; i < 3; i++)
        {
            tmpStars[0, i] = data.totalRate["Detection"][i];
            tmpStars[1, i] = data.totalRate["Synthesis"][i];
            tmpStars[2, i] = data.totalRate["Elimination"][i];
            tmpStars[3, i] = data.totalRate["Alternative"][i];


            tmpTriedCnt["Detection"] = data.triedCnt["Detection"];
            tmpTriedCnt["Synthesis"] = data.triedCnt["Synthesis"];
            tmpTriedCnt["Elimination"] = data.triedCnt["Elimination"];
            tmpTriedCnt["Alternative"] = data.triedCnt["Alternative"];
        }
    }


    public void InitStageData()
    {
        tmpStage = new int[4];
    }

    public bool GetDataWithID(string id)
    {
        if (!allData.ContainsKey(id))
        {
            //창 띄우기
            Debug.Log("유효한 id가 아닙니다");
            return false;
        }
        else
        {
            currId = id;
            data = allData[id];
            bLogin = true;
            return true;
        }
    }

    public void deleteID()
    {
        StageStorageScript.deleteStageInfoOfCurrentId(currId);
        deleteTotalInfoOfCurrentId(currId);


        //SceneManager.LoadScene("StartMenu");
    }
}
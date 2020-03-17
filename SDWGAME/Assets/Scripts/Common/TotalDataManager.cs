using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;




class ConclusionDataForEachLevel
{
    public int obtainedStarCountForEachLevel; //gamename, level, step, obtained star 개수
    public float perfectionForEachLevel;
    public float avgResponseTimeForEachLevel;
    public float IESforEachLevel;

    public ConclusionDataForEachLevel()
    {
        obtainedStarCountForEachLevel = 0;
        perfectionForEachLevel = 0.0f;
        avgResponseTimeForEachLevel = 0.0f;
        IESforEachLevel = 0.0f;
    }
}

class ConclusionData
{
    public string id;
    public Dictionary<string, int> achievedLevel; // 피험자가 과반 이상 정답을 맞춘 최고단계
    public Dictionary<string, Dictionary<string, 
        Dictionary<string, int>>> triedCntForEachStage;
    
    public Dictionary<string, Dictionary<string, 
        ConclusionDataForEachLevel>> dataForEachLevel;

   


    public ConclusionData()
    {
        achievedLevel = new Dictionary<string, int>()
        {
            {"Alternative", 0},
            {"Elimination", 0},
            {"Synthesis", 0},
            {"Detection", 0}
        };

        triedCntForEachStage = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>()
        {
            {
                "Detection", new Dictionary<string, Dictionary<string, int>>()
                {
                    {"초급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}},
                    {"중급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}},
                    {"고급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}}
                }
            },
            {
                "Synthesis", new Dictionary<string, Dictionary<string, int>>()
                {
                    {"초급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}},
                    {"중급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}},
                    {"고급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}}
                }
            },
            {
                "Elimination", new Dictionary<string, Dictionary<string, int>>()
                {
                    {"초급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}},
                    {"중급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}},
                    {"고급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}}
                }
            },
            {
                "Alternative", new Dictionary<string, Dictionary<string, int>>()
                {
                    {"초급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}},
                    {"중급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}},
                    {"고급", new Dictionary<string, int>() {{"Stage1", 0}, {"Stage2", 0}, {"Stage3", 0}}}
                }
            }
        };
        dataForEachLevel = new Dictionary<string, Dictionary<string, ConclusionDataForEachLevel>>()
        {
            {
                "Detection", new Dictionary<string, ConclusionDataForEachLevel>()
                {
                    {"초급", new ConclusionDataForEachLevel()},
                    {"중급", new ConclusionDataForEachLevel()},
                    {"고급", new ConclusionDataForEachLevel()}
                }
            },
            {
                "Synthesis", new Dictionary<string, ConclusionDataForEachLevel>()
                {
                    {"초급", new ConclusionDataForEachLevel()},
                    {"중급", new ConclusionDataForEachLevel()},
                    {"고급", new ConclusionDataForEachLevel()}
                }
            },
            {
                "Elimination", new Dictionary<string, ConclusionDataForEachLevel>()
                {
                    {"초급", new ConclusionDataForEachLevel()},
                    {"중급", new ConclusionDataForEachLevel()},
                    {"고급", new ConclusionDataForEachLevel()}
                }
            },
            {
                "Alternative", new Dictionary<string, ConclusionDataForEachLevel>()
                {
                    {"초급", new ConclusionDataForEachLevel()},
                    {"중급", new ConclusionDataForEachLevel()},
                    {"고급", new ConclusionDataForEachLevel()}
                }
            },
        };
    }
}

public enum EGameName
{
    Detection,
    Synthesis,
    Elimination,
    Alternative
}

public class TotalDataManager : MonoBehaviour
{
    private GameObject eachQuestionStorage;
    private EachQuestionDataManager eachQuestionStorageScript;

    public string currId;
    private Dictionary<string, ConclusionData> allData;
    private ConclusionData data;
    public int chosenLevel;
    public int[] tmpLevel;
    public int[] tmpMaxLevel;
    public int[] tmpStage;
    public int[,] tmpStars;
    public Dictionary<string, int[,]> tmpTriedCnt;
    public Dictionary<string, int[,]> beforeTriedCnt;
    public bool[] isLevelOpen;
    public bool bLogin;
    private static TotalDataManager instance;
    private string conclusionPath;

    private string tmpJsonData;

    public int tmpCorrectAnswerCnt;
    public int beforeCorrectAnswerCnt;
    public float tmpResponseTime;
    public float beforeResponseTime;
    public Dictionary<string, Dictionary<int, int>> tmpObtainedStarForEachLevel;


    // Public static reference that can be accesd from anywhere
    public static TotalDataManager Instance
    {
        get
        {
            // Check if instance has not been set yet and set it it is not set already
            // This takes place only on the first time usage of this reference
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TotalDataManager>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    void Awake()
    {
        /*if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }*/


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
        eachQuestionStorage = GameObject.Find("EachQuestionStorage");
        eachQuestionStorageScript = eachQuestionStorage.GetComponent<EachQuestionDataManager>();
        
        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(Application.streamingAssetsPath);
        //conclusionPath = Path.Combine(Application.streamingAssetsPath + "/Conclusion.json");
        conclusionPath = Path.Combine(Application.persistentDataPath, "Conclusion.json");

        Debug.Log("ConclusionPath: " + conclusionPath);

        bLogin = false;
        isLevelOpen = new bool[3];
        tmpLevel = new int[4];
        tmpMaxLevel = new int[4];

        tmpStars = new int[4, 3];


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


        //StartCoroutine(WebGLPath());
        //jdata = tmpJsonData;
        //jdata = tmpJsonData;
        //Debug.Log("tmpJsonData" + jdata);

        //ReadFile(conclusionPath, jdata);
        allData = JsonConvert.DeserializeObject<Dictionary<string, ConclusionData>>(jdata);
        Debug.Log("alldata: " + JsonConvert.SerializeObject(allData, Formatting.Indented));


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


    public void Save(EGameName eGameName, int level, int step)
    {
        string levelString = "";
        string stepString = "";
        int afterTry = 0;
        float avgResTime = 0;
        float perfection = 0;
        switch (level)
        {
            case 0:
                levelString = "초급";
                break;
            case 1:
                levelString = "중급";
                break;
            case 2:
                levelString = "고급";
                break;
            default:
                Debug.Assert(false, "이상한 레벨");
                break;
        }

        switch (step)
        {
            case 0:
                stepString = "Stage1";
                break;
            case 1:
                stepString = "Stage2";
                break;
            case 2:
                stepString = "Stage3";
                break;
            default:
                Debug.Assert(false, "이상한 stage");
                break;
        }

        switch (eGameName)
        {
            case EGameName.Detection:
                data.achievedLevel["Detection"] = tmpMaxLevel[0];

                //int beforeTry = beforeTriedCnt["Detection"][level, step];
                afterTry = tmpTriedCnt["Detection"][level, step];
                

                data.triedCntForEachStage["Detection"][levelString][stepString] = afterTry;

                data.dataForEachLevel["Detection"][levelString].obtainedStarCountForEachLevel =
                    tmpObtainedStarForEachLevel["Detection"][level];
                avgResTime = data.dataForEachLevel["Detection"][levelString].avgResponseTimeForEachLevel =
                    (beforeResponseTime + tmpResponseTime) / (afterTry);
                perfection = data.dataForEachLevel["Detection"][levelString].perfectionForEachLevel =
                    tmpCorrectAnswerCnt * 100;
                data.dataForEachLevel["Detection"][levelString].IESforEachLevel = avgResTime / perfection;

                break;


            case EGameName.Synthesis:
                data.achievedLevel["Synthesis"] = tmpMaxLevel[1];
                
                //int beforeTry = beforeTriedCnt["Detection"][level, step];
                afterTry = tmpTriedCnt["Synthesis"][level, step];
                

                data.triedCntForEachStage["Synthesis"][levelString][stepString] = afterTry;

                data.dataForEachLevel["Synthesis"][levelString].obtainedStarCountForEachLevel =
                    tmpObtainedStarForEachLevel["Synthesis"][level];
                avgResTime = data.dataForEachLevel["Synthesis"][levelString].avgResponseTimeForEachLevel =
                    (beforeResponseTime + tmpResponseTime) / (afterTry);
                perfection = data.dataForEachLevel["Synthesis"][levelString].perfectionForEachLevel =
                    tmpCorrectAnswerCnt * 100;
                data.dataForEachLevel["Synthesis"][levelString].IESforEachLevel = avgResTime / perfection;

                break;


            case EGameName.Elimination:
                data.achievedLevel["Elimination"] = tmpMaxLevel[2];
                //data.obtainedStarCount["Elimination"][level][step] = tmpStars[2, level];
                afterTry = tmpTriedCnt["Elimination"][level, step];
                

                data.triedCntForEachStage["Elimination"][levelString][stepString] = afterTry;

                data.dataForEachLevel["Elimination"][levelString].obtainedStarCountForEachLevel =
                    tmpObtainedStarForEachLevel["Elimination"][level];
                avgResTime = data.dataForEachLevel["Elimination"][levelString].avgResponseTimeForEachLevel =
                    (beforeResponseTime + tmpResponseTime) / (afterTry);
                perfection = data.dataForEachLevel["Elimination"][levelString].perfectionForEachLevel =
                    tmpCorrectAnswerCnt * 100;
                data.dataForEachLevel["Elimination"][levelString].IESforEachLevel = avgResTime / perfection;

                break;


            case EGameName.Alternative:
                data.achievedLevel["Alternative"] = tmpMaxLevel[3];
                //data.obtainedStarCount["Alternative"][level][step] = tmpStars[3, level];
                afterTry = tmpTriedCnt["Alternative"][level, step];
                

                data.triedCntForEachStage["Alternative"][levelString][stepString] = afterTry;

                data.dataForEachLevel["Alternative"][levelString].obtainedStarCountForEachLevel =
                    tmpObtainedStarForEachLevel["Alternative"][level];
                avgResTime = data.dataForEachLevel["Alternative"][levelString].avgResponseTimeForEachLevel =
                    (beforeResponseTime + tmpResponseTime) / (afterTry);
                perfection = data.dataForEachLevel["Alternative"][levelString].perfectionForEachLevel =
                    tmpCorrectAnswerCnt * 100;
                data.dataForEachLevel["Alternative"][levelString].IESforEachLevel = avgResTime / perfection;

                break;
        }


        allData[currId] = data;
        string jdata = JsonConvert.SerializeObject(allData, Formatting.Indented);
        //File.WriteAllText(conclusionPath, jdata);
        WriteFile(conclusionPath, jdata);
    }


    public void LoadTmpData()
    {
        tmpMaxLevel[0] = tmpLevel[0] = data.achievedLevel["Detection"];
        tmpMaxLevel[1] = tmpLevel[1] = data.achievedLevel["Synthesis"];
        tmpMaxLevel[2] = tmpLevel[2] = data.achievedLevel["Elimination"];
        tmpMaxLevel[3] = tmpLevel[3] = data.achievedLevel["Alternative"];

        for (int i = 0; i < 3; i++)
        {
            /*tmpStars[0, i] = data.obtainedStarCountForEachLevel["Detection"][i];
            tmpStars[1, i] = data.obtainedStarCountForEachLevel["Synthesis"][i];
            tmpStars[2, i] = data.obtainedStarCountForEachLevel["Elimination"][i];
            tmpStars[3, i] = data.obtainedStarCountForEachLevel["Alternative"][i];


            tmpTriedCnt["Detection"] = data.triedCnt["Detection"];
            tmpTriedCnt["Synthesis"] = data.triedCnt["Synthesis"];
            tmpTriedCnt["Elimination"] = data.triedCnt["Elimination"];
            tmpTriedCnt["Alternative"] = data.triedCnt["Alternative"];*/
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
        eachQuestionStorageScript.deleteStageInfoOfCurrentId(currId);
        deleteTotalInfoOfCurrentId(currId);
    }
}
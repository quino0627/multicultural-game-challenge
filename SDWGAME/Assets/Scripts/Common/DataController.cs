using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NestedDictionaryLib;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

#endif

class userStageData
{
    //public string userId; //학습자 ID
    public int level; //난이도
    public int stageIndex; //문제 번호
    public int nthTry;
    public string[] corrAns; // 정답 보기 글자
    public List<string> chosenAns; //반응 보기 글자
    public bool isUserRight; //정오표시(60초가 지날 때까지 정답을 고르지 못하면 오답)
    public float responseTime; // 반응시간
    public string gameType; // 자극 유형/ 현재 문제에서 제시한 자극의 유형/숫자
    public int accumulatedCntClick; //각 stage마다 정답을 클릭하기까지 클릭 횟수


    public userStageData()
    {
        //this.userId = userId;
        level = 0;
        stageIndex = 0;
        nthTry = 0;
        corrAns = new string[5]; //합성과제때문에
        chosenAns = new List<string>();
        isUserRight = false;
        responseTime = 0;
        accumulatedCntClick = 0;
    }
}


public class DataController : MonoBehaviour
{
    private string detectionPath;
    private string eliminationPath;
    private string alternativePath;
    private string synthesisPath;

    private GameObject quizManager;

    private GameObject TotalStorage;
    private KeepTrackController TotalStorageScript;

    public DetectionQuizManager DQM;
    public SpreadChoices SC;
    public FishShowAnswer FSA;
    public AlternativeQuizManager AQM;

    /*Dictionary<string,List<userStageData>> data 
        = new Dictionary<string, List<userStageData>>();*/
    //Dictionary<string, List<userStageData>> stageDatas; //string은 id
    //List<userStageData> data = new List<userStageData>();
    Dictionary<int, Dictionary<int, userStageData>> data; //level,stage

    //Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>> detectionStageDatas; //string은 id
    private NestedDictionary<string, int, int, int, userStageData> detectionStageDatas;
    private NestedDictionary<int, NestedDictionary<int, int, userStageData>> detectionData; //level stage

    private NestedDictionary<string, int, int, int, userStageData> synthesisStageDatas;
    private NestedDictionary<int, NestedDictionary<int, int, userStageData>> synthesisData;

    private NestedDictionary<string, int, int, int, userStageData> eliminationStageDatas; //string은 id
    private NestedDictionary<int, NestedDictionary<int, int, userStageData>> eliminationData;

    private NestedDictionary<string, int, int, int, userStageData> alternativeStageDatas; //string은 id
    private NestedDictionary<int, NestedDictionary<int, int, userStageData>> alternativeData;

    public void Start()
    {
        detectionPath = Path.Combine(Application.streamingAssetsPath, "Detection.json");
        synthesisPath = Path.Combine(Application.streamingAssetsPath, "Synthesis.json");
        eliminationPath = Path.Combine(Application.streamingAssetsPath, "Elimination.json");
        alternativePath = Path.Combine(Application.streamingAssetsPath, "Alternative.json");

        Debug.Log("datacontroller start");
        DontDestroyOnLoad(gameObject);


        TotalStorage = GameObject.Find("TotalStorage");
        TotalStorageScript = TotalStorage.GetComponent<KeepTrackController>();

        LoadStageData();

        /*List<userStageData> tmp1 = new List<userStageData>();
        tmp1.Add(new userStageData());
        Dictionary<int, List<userStageData>> tmp2 = new Dictionary<int, List<userStageData>>();
        for (int i = 0; i < 30; i++)
        {
            tmp2.Add(i,tmp1);
        }
        Dictionary<int, Dictionary<int, List<userStageData>>> tmp3;
        tmp3 = new Dictionary<int, Dictionary<int, List<userStageData>>>()
        {
            {0,tmp2},{1,tmp2},{2,tmp2}
        };
        Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>> tmp4
            = new Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>>();
        tmp4["testID"] = tmp3;*/

        /*NestedDictionary<string,int,int,int,userStageData> tmp4 = new NestedDictionary<string, int, int, int, userStageData>();
        
        string jdata = JsonConvert.SerializeObject(tmp4);
        File.WriteAllText(Application.dataPath+"/Detection.json",jdata);
        
        jdata = JsonConvert.SerializeObject(tmp4);
        File.WriteAllText(Application.dataPath+"/Synthesis.json",jdata);
        
        jdata = JsonConvert.SerializeObject(tmp4);
        File.WriteAllText(Application.dataPath+"/Elimination.json",jdata);
        
        jdata = JsonConvert.SerializeObject(tmp4);
        File.WriteAllText(Application.dataPath+"/Alternative.json",jdata);*/
    }


    public void makeNewId(string id)
    {
        Debug.Log("makeNewId");

        detectionStageDatas.Add(id, new NestedDictionary<int, int, int, userStageData>());
        synthesisStageDatas.Add(id, new NestedDictionary<int, int, int, userStageData>());
        eliminationStageDatas.Add(id, new NestedDictionary<int, int, int, userStageData>());
        alternativeStageDatas.Add(id, new NestedDictionary<int, int, int, userStageData>());

        /*string jdata = JsonConvert.SerializeObject(detectionStageDatas, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Detection.json", jdata);

        jdata = JsonConvert.SerializeObject(synthesisStageDatas, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Synthesis.json", jdata);

        jdata = JsonConvert.SerializeObject(eliminationStageDatas, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Elimination.json", jdata);

        jdata = JsonConvert.SerializeObject(alternativeStageDatas, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Alternative.json", jdata);*/
        SaveAllAtOnce();
    }

    public void deleteStageInfoOfCurrentId(string id)
    {
        if (detectionStageDatas.Remove(id))
        {
            Debug.Log("Removed " + id + "'s 탐지과제 정보");
        }
        else
        {
            Debug.Log("No such id");
        }

        if (synthesisStageDatas.Remove(id))
        {
            Debug.Log("Removed " + id + "'s 탐지과제 정보");
        }
        else
        {
            Debug.Log("No such id");
        }

        if (eliminationStageDatas.Remove(id))
        {
            Debug.Log("Removed " + id + "'s 탐지과제 정보");
        }
        else
        {
            Debug.Log("No such id");
        }

        if (alternativeStageDatas.Remove(id))
        {
            Debug.Log("Removed " + id + "'s 탐지과제 정보");
        }
        else
        {
            Debug.Log("No such id");
        }

        SaveAllAtOnce();
    }

    private string ReadFile(string filePath, string json)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            json = reader.ReadToEnd();
        }

        return json;
    }

    public void LoadStageData()
    {
        Debug.Log("LoadStageData");

        NestedDictionary<string, int, int, int, userStageData> tmp4 =
            new NestedDictionary<string, int, int, int, userStageData>();
        tmp4.Add("initial", new NestedDictionary<int, int, int, userStageData>());
        string jdata = JsonConvert.SerializeObject(tmp4);


        if (!File.Exists(detectionPath))
        {
            //Directory.CreateDirectory(Application.persistentDataPath + "/Detection.json");
            //File.Create(Application.streamingAssetsPath + "/Detection.json");
            //File.WriteAllText(detectionPath, jdata);

            using (var fileStream = new FileStream(detectionPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(jdata);
                }
            }
        }

        string jdata1 = "";
        jdata1 = File.ReadAllText(detectionPath);
        //(detectionPath, jdata1);
        //var jdata1TextAsset = (TextAsset) Resources.Load("Detection.json");
        detectionStageDatas =
            JsonConvert.DeserializeObject<NestedDictionary<string, int, int, int, userStageData>>(
                jdata1);


        if (!File.Exists(synthesisPath))
        {
            //Directory.CreateDirectory(Application.persistentDataPath + "/Synthesis.json");
            //File.Create(Application.streamingAssetsPath + "/Synthesis.json");
            //File.WriteAllText(synthesisPath, jdata);

            using (var fileStream = new FileStream(synthesisPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(jdata);
                }
            }
        }


        string jdata2 = File.ReadAllText(synthesisPath);
        //string jdata2 = "";
        //ReadFile(synthesisPath,jdata2);
        //var jdata2TextAsset = (TextAsset) Resources.Load("Synthesis.json");
        synthesisStageDatas =
            JsonConvert.DeserializeObject<NestedDictionary<string, int, int, int, userStageData>>(
                jdata2);


        if (!File.Exists(eliminationPath))
        {
            //Directory.CreateDirectory(Application.persistentDataPath + "/Elimination.json");
            //File.Create(Application.streamingAssetsPath + "/Elimination.json");
            //File.WriteAllText(eliminationPath, jdata);

            using (var fileStream = new FileStream(eliminationPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(jdata);
                }
            }
        }

        string jdata3 = File.ReadAllText(eliminationPath);
        //string jdata3 = "";
        //ReadFile(eliminationPath,jdata3);
        //var jdata3TextAsset = (TextAsset) Resources.Load("Elimination.json");
        eliminationStageDatas =
            JsonConvert.DeserializeObject<NestedDictionary<string, int, int, int, userStageData>>(
                jdata3);


        if (!File.Exists(alternativePath))
        {
            //Directory.CreateDirectory(Application.persistentDataPath + "/Alternative.json");
            //File.Create(Application.streamingAssetsPath + "/Alternative.json");
            //File.WriteAllText(alternativePath, jdata);

            using (var fileStream = new FileStream(alternativePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(jdata);
                }
            }
        }

        string jdata4 = File.ReadAllText(alternativePath);
        //string jdata4 = "";
        //ReadFile(alternativePath, jdata4);
        //var jdata4TextAsset = (TextAsset) Resources.Load("Alternative.json");
        alternativeStageDatas =
            JsonConvert.DeserializeObject<NestedDictionary<string, int, int, int, userStageData>>(
                jdata4);
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

    public void LoadUserStageData()
    {
        Debug.Log("LoadUserStageData");
        detectionData = detectionStageDatas[TotalStorageScript.currId];
        synthesisData = synthesisStageDatas[TotalStorageScript.currId];
        eliminationData = eliminationStageDatas[TotalStorageScript.currId];
        alternativeData = alternativeStageDatas[TotalStorageScript.currId];
    }

    public void SaveAllAtOnce()
    {
        Debug.Log("SaveAllAtonce");
        string jdata = JsonConvert.SerializeObject(detectionStageDatas, Formatting.Indented);
        //File.WriteAllText(detectionPath, jdata);
        WriteFile(detectionPath, jdata);

        jdata = JsonConvert.SerializeObject(synthesisStageDatas, Formatting.Indented);
        //File.WriteAllText(synthesisPath, jdata);
        WriteFile(synthesisPath, jdata);

        jdata = JsonConvert.SerializeObject(eliminationStageDatas, Formatting.Indented);
        //File.WriteAllText(eliminationPath, jdata);
        WriteFile(eliminationPath, jdata);

        jdata = JsonConvert.SerializeObject(alternativeStageDatas, Formatting.Indented);
        //File.WriteAllText(alternativePath, jdata);
        WriteFile(alternativePath, jdata);
    }

    public void SaveDetection()
    {
        /*Debug.Log("detectionStageDatas 자구 만들기 전: " + JsonConvert.SerializeObject(
                      detectionStageDatas, Formatting.Indented));*/
        userStageData tmp = new userStageData();
        //tmp.userId = 
        //Debug.Log("DQM.level = " + DQM.level);
        tmp.level = DQM.level;
        tmp.stageIndex = DQM.ref_stage_no;
        //tmp.nthTry = ++TotalStorageScript.tmpTriedCnt[0, tmp.stageIndex];
        tmp.nthTry = ++TotalStorageScript.tmpTriedCnt["Detection"][tmp.level, tmp.stageIndex];
        tmp.corrAns[0] = DQM.ref_answer_string;
        tmp.chosenAns = DQM.chosenAns;
        tmp.isUserRight = DQM.isUserRight;
        tmp.responseTime = DQM.responseTime;
        tmp.gameType = "탐지";
        tmp.accumulatedCntClick = DQM.total_clicked;

        //data["Detection"].Add(tmp);
        /*detectionData[DQM.level][DQM.ref_stage_no].Add(tmp);
        detectionStageDatas[TotalStorageScript.currId] = detectionData;*/
        //Debug.Log("data= " + JsonConvert.SerializeObject(tmp, Formatting.Indented));

        /*Debug.Log("detectionStageDatas전: " + JsonConvert.SerializeObject(
                      detectionStageDatas, Formatting.Indented));*/
        /*
        List<userStageData> tmpList = detectionStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex];
        Debug.Log("tmpList전:" + JsonConvert.SerializeObject(tmpList, Formatting.Indented));
        tmpList.Add(tmp);
        Debug.Log("TmpList후: " + JsonConvert.SerializeObject(tmpList, Formatting.Indented));
*/

        // 문제가 되는 코드
        //Debug.Log(TotalStorageScript.currId + tmp.level + tmp.stageIndex);
        //((detectionStageDatas[TotalStorageScript.currId])[tmp.level])[tmp.stageIndex].Add(tmp);

        detectionStageDatas.Add(TotalStorageScript.currId, tmp.level, tmp.stageIndex, tmp.nthTry, tmp);


        /*Debug.Log("detectionStageDatas후: " + JsonConvert.SerializeObject(
                      detectionStageDatas, Formatting.Indented));*/

        string jdata = JsonConvert.SerializeObject(detectionStageDatas, Formatting.Indented);
        //File.WriteAllText(detectionPath, jdata);
        WriteFile(detectionPath, jdata);
    }

    public void SaveAlternative()
    {
        userStageData tmp = new userStageData();
        tmp.level = AQM.level;
        tmp.stageIndex = AQM.ref_stage_no;
        //tmp.nthTry = ++TotalStorageScript.tmpTriedCnt[3, tmp.stageIndex];
        tmp.nthTry = ++TotalStorageScript.tmpTriedCnt["Alternative"][tmp.level, tmp.stageIndex];
        tmp.corrAns[0] = AQM.ref_answer_string;
        tmp.chosenAns = AQM.chosenAns;
        tmp.isUserRight = AQM.isUserRight;
        tmp.gameType = "대치";
        tmp.responseTime = AQM.responseTime;
        tmp.accumulatedCntClick = AQM.total_clicked;

        //Debug.Log("data= " + JsonConvert.SerializeObject(tmp, Formatting.Indented));

        //이 코드로 하면 모든 list에 추가가됨... 왜그럴까...
        //alternativeData[AQM.level][AQM.ref_stage_no].Add(tmp);
        //alternativeStageDatas[TotalStorageScript.currId] = alternativeData;


        //이제 이 코드도 그러네 하...
        //아까는 되었는데!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!시발ㄹㄹㄹㄹㄹㄹㄹㄹㄹㄹㄹㄹ
        /*List<userStageData> tmpList = alternativeStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex];
        tmpList.Add(tmp);
        alternativeStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex] = tmpList;*/
        alternativeStageDatas.Add(TotalStorageScript.currId, tmp.level, tmp.stageIndex, tmp.nthTry, tmp);


        string jdata = JsonConvert.SerializeObject(alternativeStageDatas, Formatting.Indented);
        //File.WriteAllText(alternativePath, jdata);
        WriteFile(alternativePath, jdata);
    }

    public void SaveSynthesis()
    {
        int curLevel;
        userStageData tmp = new userStageData();
        curLevel = SC.realLevel;
        tmp.level = curLevel;
        tmp.stageIndex = SC.refStageIndex;

        curLevel++;

        //tmp.nthTry = ++TotalStorageScript.tmpTriedCnt[1, tmp.stageIndex];
        tmp.nthTry = ++TotalStorageScript.tmpTriedCnt["Synthesis"][tmp.level, tmp.stageIndex];
        for (int i = 0; i < curLevel; i++)
        {
            tmp.corrAns[i] = SC.choiceTexts[SC.corrAnsPosIndex[i]].text;
            //tmp.chosenAns[i] = SC.chosenAns[i];
        }

        tmp.chosenAns = SC.chosenAns;
        tmp.isUserRight = SC.isUserRight;
        tmp.responseTime = SC.responseTime;
        tmp.gameType = "합성";
        tmp.accumulatedCntClick = SC.ref_total_tried;

        /*synthesisData[curLevel][SC.refStageIndex].Add(tmp);
        synthesisStageDatas[TotalStorageScript.currId] = synthesisData;*/

        /*List<userStageData> tmpList = synthesisStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex];
        tmpList.Add(tmp);
        synthesisStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex] = tmpList;*/
        synthesisStageDatas.Add(TotalStorageScript.currId, tmp.level, tmp.stageIndex, tmp.nthTry, tmp);


        string jdata = JsonConvert.SerializeObject(synthesisStageDatas, Formatting.Indented);
        //File.WriteAllText(synthesisPath, jdata);
        WriteFile(synthesisPath, jdata);
    }

    public void SaveElimination()
    {
        userStageData tmp = new userStageData();
        tmp.level = FSA.level;
        tmp.stageIndex = FSA.refStageIndex;
        //tmp.nthTry = ++TotalStorageScript.tmpTriedCnt[2, tmp.stageIndex];
        tmp.nthTry = ++TotalStorageScript.tmpTriedCnt["Elimination"][tmp.level, tmp.stageIndex];
        tmp.corrAns[0] = FSA.choiceTexts[FSA.ansPosIndex[0]].text;
        tmp.chosenAns = (FSA.chosenAns);
        tmp.isUserRight = FSA.isUserRight;
        tmp.responseTime = FSA.responseTime;
        tmp.gameType = "탈락";
        tmp.accumulatedCntClick = FSA.ref_total_clicked;

        /*eliminationData[FSA.level][FSA.refStageIndex].Add(tmp);
        eliminationStageDatas[TotalStorageScript.currId] = eliminationData;*/

        /*print(JsonConvert.SerializeObject(
            eliminationStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex]
            , Formatting.Indented
        ));

        List<userStageData> tmpList = eliminationStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex];

        print("tmpList전: " + JsonConvert.SerializeObject(tmpList, Formatting.Indented));

        tmpList.Add(tmp);

        print("tmpList후: " + JsonConvert.SerializeObject(tmpList, Formatting.Indented));

        eliminationStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex] = tmpList;

        print(JsonConvert.SerializeObject(
            eliminationStageDatas[TotalStorageScript.currId][tmp.level][tmp.stageIndex]
            , Formatting.Indented));*/

        eliminationStageDatas.Add(TotalStorageScript.currId, tmp.level, tmp.stageIndex, tmp.nthTry, tmp);


        string jdata = JsonConvert.SerializeObject(eliminationStageDatas, Formatting.Indented);
        //File.WriteAllText(eliminationPath, jdata);
        WriteFile(eliminationPath, jdata);
    }
}
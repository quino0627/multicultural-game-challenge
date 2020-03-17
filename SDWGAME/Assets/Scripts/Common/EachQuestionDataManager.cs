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
    public int level; //난이도
    public int step; // 세션/단계
    public int stageIndex; //stageIndex
    public int questionId; //문제 번호
    public int nthTry;
    public string[] corrAns; // 정답 보기 글자
    public List<string> chosenAns; //반응 보기 글자
    public bool isUserRight; //정오표시(60초가 지날 때까지 정답을 고르지 못하면 오답)
    public float responseTime; // 반응시간
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
    private string tmpJsonData;

    private string detectionPath;
    private string eliminationPath;
    private string alternativePath;
    private string synthesisPath;

    private GameObject quizManager;

    private GameObject TotalStorage;
    private KeepTrackController TotalStorageScript;

    private GameObject StageStorage;
    private GameStageDataManager StageStorageScript;

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
    private NestedDictionary<string, int, int, int, userStageData> detectionDataForEachQuestion;
    private NestedDictionary<int, NestedDictionary<int, int, userStageData>> detectionData; //level stage

    private NestedDictionary<string, int, int, int, userStageData> synthesisDataForEachQuestion;
    private NestedDictionary<int, NestedDictionary<int, int, userStageData>> synthesisData;

    private NestedDictionary<string, int, int, int, userStageData> eliminationDataForEachQuestion; //string은 id
    private NestedDictionary<int, NestedDictionary<int, int, userStageData>> eliminationData;

    private NestedDictionary<string, int, int, int, userStageData> alternativeDataForEachQuestion; //string은 id
    private NestedDictionary<int, NestedDictionary<int, int, userStageData>> alternativeData;

    public void Start()
    {
        detectionPath = Path.Combine(Application.persistentDataPath, "Detection.json");
        synthesisPath = Path.Combine(Application.persistentDataPath, "Synthesis.json");
        eliminationPath = Path.Combine(Application.persistentDataPath, "Elimination.json");
        alternativePath = Path.Combine(Application.persistentDataPath, "Alternative.json");

        Debug.Log("datacontroller start");
        DontDestroyOnLoad(gameObject);


        TotalStorage = GameObject.Find("TotalStorage");
        TotalStorageScript = TotalStorage.GetComponent<KeepTrackController>();

        LoadStageData();
        //미리 다 받아놔야 회원가입 중복방지, 로그인 오류방지

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

        detectionDataForEachQuestion.Add(id, new NestedDictionary<int, int, int, userStageData>());
        synthesisDataForEachQuestion.Add(id, new NestedDictionary<int, int, int, userStageData>());
        eliminationDataForEachQuestion.Add(id, new NestedDictionary<int, int, int, userStageData>());
        alternativeDataForEachQuestion.Add(id, new NestedDictionary<int, int, int, userStageData>());
        SaveAllAtOnce();
    }

    public void deleteStageInfoOfCurrentId(string id)
    {
        if (detectionDataForEachQuestion.Remove(id))
        {
            Debug.Log("Removed " + id + "'s 탐지과제 정보");
        }
        else
        {
            Debug.Log("No such id");
        }

        if (synthesisDataForEachQuestion.Remove(id))
        {
            Debug.Log("Removed " + id + "'s 탐지과제 정보");
        }
        else
        {
            Debug.Log("No such id");
        }

        if (eliminationDataForEachQuestion.Remove(id))
        {
            Debug.Log("Removed " + id + "'s 탐지과제 정보");
        }
        else
        {
            Debug.Log("No such id");
        }

        if (alternativeDataForEachQuestion.Remove(id))
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

        //StartCoroutine(WebGLPath(detectionPath));
        //WebGLPath(detectionPath);
        //jdata = tmpJsonData;

        detectionDataForEachQuestion =
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

        //string jdata2 = "";
        string jdata2 = File.ReadAllText(synthesisPath);
        //StartCoroutine(WebGLPath(synthesisPath));
        //WebGLPath(synthesisPath);
        //jdata2 = tmpJsonData;

        synthesisDataForEachQuestion =
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

        //StartCoroutine(WebGLPath(eliminationPath));
        //WebGLPath(eliminationPath);
        //jdata3 = tmpJsonData;
        eliminationDataForEachQuestion =
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

        //StartCoroutine(WebGLPath(alternativePath));
        //WebGLPath(alternativePath);
        //jdata4 = tmpJsonData;

        alternativeDataForEachQuestion =
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

   

    public void SaveGameOver(EGameName eGameName)
    {
        string jdata = "";
        switch (eGameName)
        {
            case EGameName.Detection:
                jdata = JsonConvert.SerializeObject(detectionDataForEachQuestion, Formatting.Indented);
                WriteFile(detectionPath, jdata);
                break;

            case EGameName.Synthesis:
                jdata = JsonConvert.SerializeObject(synthesisDataForEachQuestion, Formatting.Indented);

                WriteFile(synthesisPath, jdata);
                break;

            case EGameName.Alternative:
                jdata = JsonConvert.SerializeObject(alternativeDataForEachQuestion, Formatting.Indented);
                WriteFile(alternativePath, jdata);
                break;

            case EGameName.Elimination:
                jdata = JsonConvert.SerializeObject(eliminationDataForEachQuestion, Formatting.Indented);
                WriteFile(eliminationPath, jdata);
                break;
        }
    }

    public void SaveAllAtOnce()
    {
        Debug.Log("SaveAllAtonce");
        string jdata = JsonConvert.SerializeObject(detectionDataForEachQuestion, Formatting.Indented);
        //File.WriteAllText(detectionPath, jdata);
        WriteFile(detectionPath, jdata);

        jdata = JsonConvert.SerializeObject(synthesisDataForEachQuestion, Formatting.Indented);
        //File.WriteAllText(synthesisPath, jdata);
        WriteFile(synthesisPath, jdata);

        jdata = JsonConvert.SerializeObject(eliminationDataForEachQuestion, Formatting.Indented);
        //File.WriteAllText(eliminationPath, jdata);
        WriteFile(eliminationPath, jdata);

        jdata = JsonConvert.SerializeObject(alternativeDataForEachQuestion, Formatting.Indented);
        //File.WriteAllText(alternativePath, jdata);
        WriteFile(alternativePath, jdata);
    }

    public void SaveDetectionDataForEachQuestion()
    {
        userStageData tmp = new userStageData();

        tmp.level = DQM.level;
        tmp.stageIndex = DQM.ref_stage_no;
        //tmp.nthTry = ++TotalStorageScript.tmpTriedCnt[0, tmp.stageIndex];
        tmp.nthTry = ++TotalStorageScript.tmpTriedCnt["Detection"][tmp.level, tmp.stageIndex];
        tmp.corrAns[0] = DQM.ref_answer_string;
        tmp.chosenAns = DQM.chosenAns;
        tmp.isUserRight = DQM.isUserRight;
        tmp.responseTime = DQM.responseTime;
        tmp.accumulatedCntClick = DQM.total_clicked;

        detectionDataForEachQuestion.Add(TotalStorageScript.currId, tmp.level, tmp.stageIndex, tmp.nthTry, tmp);
    }

    public void SaveAlternativeDataForEachQuestion()
    {
        userStageData tmp = new userStageData();
        tmp.level = AQM.level;
        tmp.stageIndex = AQM.ref_stage_no;
        //tmp.nthTry = ++TotalStorageScript.tmpTriedCnt[3, tmp.stageIndex];
        tmp.nthTry = ++TotalStorageScript.tmpTriedCnt["Alternative"][tmp.level, tmp.stageIndex];
        tmp.corrAns[0] = AQM.ref_answer_string;
        tmp.chosenAns = AQM.chosenAns;
        tmp.isUserRight = AQM.isUserRight;

        tmp.responseTime = AQM.responseTime;
        tmp.accumulatedCntClick = AQM.total_clicked;
        

        alternativeDataForEachQuestion.Add(TotalStorageScript.currId, tmp.level, tmp.stageIndex, tmp.nthTry, tmp);
    }

    public void SaveSynthesisDataForEachQuestion()
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

        tmp.accumulatedCntClick = SC.ref_total_tried;


        Debug.Log("tmp.nthTry" + tmp.nthTry);
        synthesisDataForEachQuestion.Add(TotalStorageScript.currId, tmp.level, tmp.stageIndex, tmp.nthTry, tmp);
    }

    public void SaveEliminationDataForEachData()
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

        tmp.accumulatedCntClick = FSA.ref_total_clicked;


        eliminationDataForEachQuestion.Add(TotalStorageScript.currId, tmp.level, tmp.stageIndex, tmp.nthTry, tmp);
    }
}
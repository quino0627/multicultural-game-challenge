using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;
using Newtonsoft.Json;

class userStageData
{
    //public string userId; //학습자 ID
    public int level; //난이도
    public int stageIndex;//문제 번호
    public int nthTry;
    public string[] corrAns; // 정답 보기 글자
    public List<string> chosenAns; //반응 보기 글자
    public bool isUserRight; //정오표시(60초가 지날 때까지 정답을 고르지 못하면 오답)
    public float responseTime; // 반응시간
    public string gameType;// 자극 유형/ 현재 문제에서 제시한 자극의 유형/숫자
    public int cntClick; //정답을 클릭하기까지 클릭 횟수


    public userStageData()
    {
        //this.userId = userId;
        this.level = 0;
        this.stageIndex = 0;
        this.nthTry = 0;
        this.corrAns = new string[5];
        this.chosenAns = new List<string>();
        this.isUserRight = false;
        this.responseTime = 0;
        this.cntClick = 0;
    }

}

public class DataController: MonoBehaviour
{
    
    private GameObject TotalStorage;
    private KeepTrackController TotalStorageScript;
    
    private DetectionQuizManager DQM;
    private SpreadChoices SC;
    private FishShowAnswer FSA;
    private AlternativeQuizManager AQM;
    
    /*Dictionary<string,List<userStageData>> data 
        = new Dictionary<string, List<userStageData>>();*/
    //Dictionary<string, List<userStageData>> stageDatas; //string은 id
    //List<userStageData> data = new List<userStageData>();
    Dictionary<int, Dictionary<int,userStageData>> data; //level,stage

    Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>> detectionStageDatas; //string은 id
    //List<userStageData> detectionData = new List<userStageData>();
    Dictionary<int, Dictionary<int, List<userStageData>>> detectionData; //level stage

    Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>> synthesisStageDatas; //string은 id
    //List<userStageData> synthesisData = new List<userStageData>();
    private Dictionary<int, Dictionary<int, List<userStageData>>> synthesisData;
    
    Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>> eliminationStageDatas; //string은 id
    private Dictionary<int, Dictionary<int, List<userStageData>>> eliminationData;


    Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>> alternativeStageDatas; //string은 id
    private Dictionary<int, Dictionary<int, List<userStageData>>> alternativeData;
    
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        //var quizManager  = GameObject.FindWithTag("QuizManager");
        TotalStorage = GameObject.Find("TotalStorage");
        TotalStorageScript = TotalStorage.GetComponent<KeepTrackController>();
        //LoadStageData();

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
        tmp4["testID"] = tmp3;
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
        List<userStageData> tmp1 = new List<userStageData>();
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
        

        detectionStageDatas[id] = new Dictionary<int, Dictionary<int, List<userStageData>>>();
        detectionStageDatas[id] = tmp3;
        
        synthesisStageDatas[id] = new Dictionary<int, Dictionary<int, List<userStageData>>>();
        synthesisStageDatas[id] = tmp3;
        
        eliminationStageDatas[id] = new Dictionary<int, Dictionary<int, List<userStageData>>>();
        eliminationStageDatas[id] = tmp3;
        
        alternativeStageDatas[id] = new Dictionary<int, Dictionary<int, List<userStageData>>>();
        alternativeStageDatas[id] = tmp3;

        string jdata = JsonConvert.SerializeObject(detectionStageDatas);
        File.WriteAllText(Application.dataPath+"/Detection.json",jdata);
        
        jdata = JsonConvert.SerializeObject(synthesisStageDatas);
        File.WriteAllText(Application.dataPath+"/Synthesis.json",jdata);
        
        jdata = JsonConvert.SerializeObject(eliminationStageDatas);
        File.WriteAllText(Application.dataPath+"/Elimination.json",jdata);
        
        jdata = JsonConvert.SerializeObject(alternativeStageDatas);
        File.WriteAllText(Application.dataPath+"/Alternative.json",jdata);
    }
    public void LoadStageData()
    {
        string jdata1 = File.ReadAllText(Application.dataPath + "/Detection.json");
        detectionStageDatas = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>>>(jdata1);
        detectionData = detectionStageDatas[TotalStorageScript.currId];
        
        string jdata2 = File.ReadAllText(Application.dataPath + "/Synthesis.json");
        synthesisStageDatas = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>>>(jdata2);
        synthesisData = synthesisStageDatas[TotalStorageScript.currId];
        
        string jdata3 = File.ReadAllText(Application.dataPath + "/Elimination.json");
        eliminationStageDatas = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>>>(jdata3);
        eliminationData = eliminationStageDatas[TotalStorageScript.currId];
        
        string jdata4 = File.ReadAllText(Application.dataPath + "/Alternative.json");
        alternativeStageDatas = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, Dictionary<int, List<userStageData>>>>>(jdata4);
        alternativeData = alternativeStageDatas[TotalStorageScript.currId];

    }
    public void SaveDetection()
    {
        userStageData tmp = new userStageData();
        //tmp.userId = 
        tmp.level = DQM.level;
        tmp.stageIndex = DQM.ref_stage_no;
        tmp.nthTry= ++TotalStorageScript.tmpTriedCnt[0];
        tmp.corrAns[0] = DQM.ref_answer_string;
        tmp.chosenAns.Add(DQM.chosenAns);
        tmp.isUserRight = DQM.isUserRight;
        tmp.responseTime = DQM.responseTime;
        //자극유형은 skip
        tmp.cntClick = DQM.total_clicked;
        
        //data["Detection"].Add(tmp);
        detectionData[DQM.level][DQM.ref_stage_no].Add(tmp);
        detectionStageDatas[TotalStorageScript.currId] = detectionData;
        string jdata = JsonConvert.SerializeObject(detectionStageDatas);
        File.WriteAllText(Application.dataPath+"/Detection.json",jdata);
    }

    public void SaveAlternative()
    {
        userStageData tmp  = new userStageData();
        tmp.level = AQM.level;
        tmp.stageIndex = AQM.ref_stage_no;
        tmp.nthTry= ++TotalStorageScript.tmpTriedCnt[3];
        tmp.corrAns[0] = AQM.ref_answer_string;
        tmp.chosenAns.Add(AQM.chosenAns);
        tmp.isUserRight = AQM.isUserRight;
        tmp.cntClick = AQM.total_clicked;
        
        alternativeData[AQM.level][AQM.ref_stage_no].Add(tmp);
        alternativeStageDatas[TotalStorageScript.currId] = alternativeData;
        string jdata = JsonConvert.SerializeObject(alternativeStageDatas);
        File.WriteAllText(Application.dataPath+"/Alternative.json",jdata);

    }

    public void SaveSynthesis()
    {
        int curLevel;
        userStageData tmp = new userStageData();
        curLevel = SC.level;
        tmp.level = curLevel;
        curLevel++;
        tmp.stageIndex = SC.refStageIndex;
        tmp.nthTry= ++TotalStorageScript.tmpTriedCnt[1];
        for (int i = 0; i < curLevel; i++)
        {
            tmp.corrAns[i] = SC.choiceTexts[SC.corrAnsPosIndex[i]].text;
            tmp.chosenAns[i] = SC.chosenAns[i];
        }

        tmp.isUserRight = SC.isUserRight;
        tmp.cntClick = SC.ref_total_tried;
        
        synthesisData[curLevel][SC.refStageIndex].Add(tmp);
        synthesisStageDatas[TotalStorageScript.currId] = synthesisData;
        string jdata = JsonConvert.SerializeObject(synthesisStageDatas);
        File.WriteAllText(Application.dataPath+"/Synthesis.json",jdata);

    }

    public void SaveElimination()
    {
        userStageData tmp = new userStageData();
        tmp.level = FSA.level;
        tmp.stageIndex = FSA.refStageIndex;
        tmp.nthTry= ++TotalStorageScript.tmpTriedCnt[2];
        tmp.corrAns[0] = FSA.choiceTexts[FSA.ansPosIndex[0]].text;
        tmp.chosenAns.Add(FSA.chosenAns);
        tmp.isUserRight = FSA.isUserRight;
        tmp.cntClick = FSA.ref_total_clicked;
        
        eliminationData[FSA.level][FSA.refStageIndex].Add(tmp);
        eliminationStageDatas[TotalStorageScript.currId] = eliminationData;
        string jdata = JsonConvert.SerializeObject(eliminationStageDatas);
        File.WriteAllText(Application.dataPath+"/Elimination.json",jdata);
    }

    
}

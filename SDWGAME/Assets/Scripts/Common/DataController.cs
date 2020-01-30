using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using Newtonsoft.Json;

class userStageData
{
    public string userId; //학습자 ID
    public int level; //난이도
    public int stageIndex; //문제 번호
    public string [] corrAns; // 정답 보기 글자
    public List<string> chosenAns; //반응 보기 글자
    public bool isUserRight; //정오표시(60초가 지날 때까지 정답을 고르지 못하면 오답)
    public float responseTime; // 반응시간
    // 자극 유형/ 현재 문제에서 제시한 자극의 유형/숫자
    public int cntClick; //정답을 클릭하기까지 클릭 횟수

    /*public userStageData(string userId, int level, int stageIndex, string[] corrAns,
        List<string> chosenAns, bool isUserRight, float responseTime, int cntClick)
    {
        /*this.userId = userId;
        this.level = level;
        this.stageIndex = stageIndex;
        this.corrAns = corrAns;
        this.chosenAns = chosenAns;
        this.isUserRight = isUserRight;
        this.responseTime = responseTime;
        this.cntClick = cntClick;#1#
    }*/
    
}

public class DataController: MonoBehaviour
{
    public DetectionQuizManager DQM;
    public SpreadChoices SC;
    public FishShowAnswer FSA;
    public AlternativeQuizManager AQM;
    
    /*Dictionary<string,List<userStageData>> data 
        = new Dictionary<string, List<userStageData>>();*/
    List<userStageData> data = new List<userStageData>();
    
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        var quizManager  = GameObject.FindWithTag("QuizManager");
        
        
    }

    public void SaveDetection()
    {
        userStageData tmp = new userStageData();
        //tmp.userId = 
        tmp.level = DQM.level;
        tmp.stageIndex = DQM.ref_stage_no;
        tmp.corrAns[0] = DQM.ref_answer_string;
        tmp.chosenAns.Add(DQM.chosenAns);
        tmp.isUserRight = DQM.isUserRight;
        tmp.responseTime = DQM.responseTime;
        //자극유형은 skip
        tmp.cntClick = DQM.total_clicked;
        
        //data["Detection"].Add(tmp);
        data.Add(tmp);
        string jdata = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath+"/Detection.json",jdata);
    }

    public void SaveAlternative()
    {
        userStageData tmp  = new userStageData();
        tmp.level = AQM.level;
        tmp.stageIndex = AQM.ref_stage_no;
        tmp.corrAns[0] = AQM.ref_answer_string;
        tmp.chosenAns.Add(AQM.chosenAns);
        tmp.isUserRight = AQM.isUserRight;
        tmp.cntClick = AQM.total_clicked;
        
        data.Add(tmp);
        string jdata = JsonConvert.SerializeObject(data);
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
        for (int i = 0; i < curLevel; i++)
        {
            tmp.corrAns[i] = SC.choiceTexts[SC.corrAnsPosIndex[i]].text;
            tmp.chosenAns[i] = SC.chosenAns[i];
        }

        tmp.isUserRight = SC.isUserRight;
        tmp.cntClick = SC.ref_total_tried;
        
        data.Add(tmp);
        string jdata = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath+"/Synthesis.json",jdata);

    }

    public void SaveElimination()
    {
        userStageData tmp = new userStageData();
        tmp.level = FSA.level;
        tmp.stageIndex = FSA.refStageIndex;
        tmp.corrAns[0] = FSA.choiceTexts[FSA.ansPosIndex[0]].text;
        tmp.chosenAns.Add(FSA.chosenAns);
        tmp.isUserRight = FSA.isUserRight;
        tmp.cntClick = FSA.ref_total_clicked;
        
        data.Add(tmp);
        string jdata = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath+"/Elimination.json",jdata);
    }

    
}

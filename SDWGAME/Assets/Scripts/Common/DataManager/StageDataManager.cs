using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using NestedDictionaryLib;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;


class DataForEachStage
{
    //id, level, stage, value
    public int playCnt; //몇번째 플레이인지
    public int avgCorrectAnswerCountForEachStage;
    public float avgResponseTimeForEachStage;

    public DataForEachStage()
    {
        playCnt = 0;
        avgCorrectAnswerCountForEachStage = 0;
        avgResponseTimeForEachStage = 0;
    }
}

class StageData
{
    public NestedDictionary<string, string, DataForEachStage> stageData;

    public StageData()
    {
        stageData = new NestedDictionary<string, string, DataForEachStage>()
        {
            {
                "초급",
                new NestedDictionary<string, DataForEachStage>()
                {
                    {"Stage1", new DataForEachStage()},
                    {"Stage2", new DataForEachStage()},
                    {"Stage3", new DataForEachStage()}
                }
            },
            {
                "중급",
                new NestedDictionary<string, DataForEachStage>()
                {
                    {"Stage1", new DataForEachStage()},
                    {"Stage2", new DataForEachStage()},
                    {"Stage3", new DataForEachStage()}
                }
            },
            {
                "고급",
                new NestedDictionary<string, DataForEachStage>()
                {
                    {"Stage1", new DataForEachStage()},
                    {"Stage2", new DataForEachStage()},
                    {"Stage3", new DataForEachStage()}
                }
            }
        };
    }
}

public class StageDataManager : MonoBehaviour
{
    public int playCnt;
    public float totalResponseTimeForEachStage;
    private const int TOTAL_QUESTION_CNT = 10;
    public float avgResponseTimeForEachStage;
    public int avgCorrectAnswerCnt;

    //ID, level, stage, value
    private Dictionary<string, StageData> tmpStageDatas;
    private Dictionary<string, StageData> tmpDStageDatas;
    private Dictionary<string, StageData> tmpSStageDatas;
    private Dictionary<string, StageData> tmpEStageDatas;
    private Dictionary<string, StageData> tmpAStageDatas;

    private string detectionPath;
    private string eliminationPath;
    private string alternativePath;
    private string synthesisPath;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        detectionPath = Path.Combine(Application.persistentDataPath, "DetectionStageData.json");
        synthesisPath = Path.Combine(Application.persistentDataPath, "SynthesisStageData.json");
        eliminationPath = Path.Combine(Application.persistentDataPath, "EliminationStageData.json");
        alternativePath = Path.Combine(Application.persistentDataPath, "AlternativeStageData.json");

        if (!File.Exists(detectionPath))
        {
            tmpStageDatas = new Dictionary<string, StageData>();
            tmpStageDatas.Add("initial",new StageData());
            string tmpJdata = JsonConvert.SerializeObject(tmpStageDatas, Formatting.Indented);
            File.WriteAllText(detectionPath,tmpJdata);
            File.WriteAllText(synthesisPath,tmpJdata);
            File.WriteAllText(eliminationPath,tmpJdata);
            File.WriteAllText(alternativePath,tmpJdata);
        }

        LoadStageGameDataAtFirst();
    }

    public void LoadStageGameDataAtFirst()
    {
        string jdata = File.ReadAllText(detectionPath);
        tmpDStageDatas = JsonConvert.DeserializeObject<Dictionary<string, StageData>>(jdata);

        jdata = File.ReadAllText(synthesisPath);
        tmpSStageDatas = JsonConvert.DeserializeObject<Dictionary<string, StageData>>(jdata);

        jdata = File.ReadAllText(eliminationPath);
        tmpEStageDatas = JsonConvert.DeserializeObject<Dictionary<string, StageData>>(jdata);

        jdata = File.ReadAllText(alternativePath);
        tmpAStageDatas = JsonConvert.DeserializeObject<Dictionary<string, StageData>>(jdata);

    }
    public void makeNewId(string id)
    {
        tmpDStageDatas.Add(id, new StageData());
        tmpSStageDatas.Add(id, new StageData());
        tmpEStageDatas.Add(id, new StageData());
        tmpAStageDatas.Add(id, new StageData());
        string jdata = JsonConvert.SerializeObject(tmpDStageDatas, Formatting.Indented);
        File.WriteAllText(detectionPath,jdata);

        jdata = JsonConvert.SerializeObject(tmpSStageDatas, Formatting.Indented);
        File.WriteAllText(synthesisPath,jdata);

        jdata = JsonConvert.SerializeObject(tmpEStageDatas, Formatting.Indented);
        File.WriteAllText(eliminationPath,jdata);

        jdata = JsonConvert.SerializeObject(tmpAStageDatas, Formatting.Indented);
        File.WriteAllText(alternativePath,jdata);
        
    }

    
    public void deleteStageInfoOfCurrentId(string id)
    {
        
        tmpDStageDatas.Remove(id);
        string jdata = JsonConvert.SerializeObject(tmpDStageDatas, Formatting.Indented);
        File.WriteAllText(detectionPath,jdata);
        
        tmpSStageDatas.Remove(id);
        jdata = JsonConvert.SerializeObject(tmpSStageDatas, Formatting.Indented);
        File.WriteAllText(synthesisPath,jdata);
        
        tmpEStageDatas.Remove(id);
        jdata = JsonConvert.SerializeObject(tmpEStageDatas, Formatting.Indented);
        File.WriteAllText(eliminationPath,jdata);
        
        tmpAStageDatas.Remove(id);
        jdata = JsonConvert.SerializeObject(tmpAStageDatas, Formatting.Indented);
        File.WriteAllText(alternativePath,jdata);
        
        
        Debug.Log("Removed " + id + "'s 스테이지 정보");
    }
    public void LoadGameStageData(EGameName eGameName, string id, int level, int stage)
    {
        string levelString = "";
        string stepString = "";
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

        switch (stage)
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

        //string jdata = "";
        switch (eGameName)
        {
            case EGameName.Detection:
                /*jdata = File.ReadAllText(detectionPath);
                tmpStageDatas = JsonConvert.DeserializeObject<Dictionary<string, StageData>>(jdata);*/
                playCnt = tmpDStageDatas[id].stageData[levelString][stepString].playCnt;
                avgResponseTimeForEachStage =
                    tmpDStageDatas[id].stageData[levelString][stepString].avgResponseTimeForEachStage;
                avgCorrectAnswerCnt = tmpDStageDatas[id].stageData[levelString][stepString]
                    .avgCorrectAnswerCountForEachStage;
                break;
            case EGameName.Synthesis:
                /*jdata = File.ReadAllText(synthesisPath);
                tmpStageDatas = JsonConvert.DeserializeObject<Dictionary<string, StageData>>(jdata);*/
                playCnt = tmpSStageDatas[id].stageData[levelString][stepString].playCnt;
                avgResponseTimeForEachStage =
                    tmpSStageDatas[id].stageData[levelString][stepString].avgResponseTimeForEachStage;
                avgCorrectAnswerCnt = tmpSStageDatas[id].stageData[levelString][stepString]
                    .avgCorrectAnswerCountForEachStage;
                break;
            case EGameName.Elimination:
                /*jdata = File.ReadAllText(eliminationPath);
                tmpStageDatas = JsonConvert.DeserializeObject<Dictionary<string, StageData>>(jdata);*/
                playCnt = tmpEStageDatas[id].stageData[levelString][stepString].playCnt;
                avgResponseTimeForEachStage =
                    tmpEStageDatas[id].stageData[levelString][stepString].avgResponseTimeForEachStage;
                avgCorrectAnswerCnt = tmpEStageDatas[id].stageData[levelString][stepString]
                    .avgCorrectAnswerCountForEachStage;
                break;
            case EGameName.Alternative:
                /*jdata = File.ReadAllText(alternativePath);
                tmpStageDatas = JsonConvert.DeserializeObject<Dictionary<string, StageData>>(jdata);*/
                playCnt = tmpAStageDatas[id].stageData[levelString][stepString].playCnt;
                avgResponseTimeForEachStage =
                    tmpAStageDatas[id].stageData[levelString][stepString].avgResponseTimeForEachStage;
                avgCorrectAnswerCnt = tmpAStageDatas[id].stageData[levelString][stepString]
                    .avgCorrectAnswerCountForEachStage;
                break;
            default:
                Debug.Assert(false, "로드 스테이지 데이터 실패");
                break;
        }

        
    }

    public float GetAvgResponseTimeForLevel(string id, int level, EGameName chosenGame)
    {
        tmpStageDatas = chooseTmpData(chosenGame);
        float avg;
        switch (level)
        {
            case 0:
                avg = (tmpStageDatas[id].stageData["초급"]["Stage1"].avgResponseTimeForEachStage +
                       tmpStageDatas[id].stageData["초급"]["Stage2"].avgResponseTimeForEachStage +
                       tmpStageDatas[id].stageData["초급"]["Stage3"].avgResponseTimeForEachStage) / 3;
                break;
            case 1:
                avg = (tmpStageDatas[id].stageData["중급"]["Stage1"].avgResponseTimeForEachStage +
                       tmpStageDatas[id].stageData["중급"]["Stage2"].avgResponseTimeForEachStage +
                       tmpStageDatas[id].stageData["중급"]["Stage3"].avgResponseTimeForEachStage) / 3;
                break;
            case 2:
                avg = (tmpStageDatas[id].stageData["고급"]["Stage1"].avgResponseTimeForEachStage +
                       tmpStageDatas[id].stageData["고급"]["Stage2"].avgResponseTimeForEachStage +
                       tmpStageDatas[id].stageData["고급"]["Stage3"].avgResponseTimeForEachStage) / 3;

                break;
            default:
                avg = 0;
                Debug.Assert(false, "이상한 레벨, 평균반응시간 못구함");
                break;
        }

        return avg;
    }

    private  Dictionary<string, StageData> chooseTmpData(EGameName chosenGame)
    {
        Dictionary<string, StageData> result = new Dictionary<string, StageData>();
        switch (chosenGame)
        {
            case EGameName.Detection:
                result = tmpDStageDatas;
                break;
            case EGameName.Synthesis:
                result = tmpSStageDatas;
                break;
            case EGameName.Elimination:
                result = tmpEStageDatas;
                break;
            case EGameName.Alternative:
                result = tmpAStageDatas;
                break;
            default:
                Debug.Assert(false,"??");
                break;
        }

        return result;
    }
    
    public int GetAvgCorrectAnswerCountForLevel(string id, int level,EGameName chosenGame)
    {
        tmpStageDatas = chooseTmpData(chosenGame);
        int avg;
        switch (level)
        {
            case 0:
                avg = (tmpStageDatas[id].stageData["초급"]["Stage1"].avgCorrectAnswerCountForEachStage +
                       tmpStageDatas[id].stageData["초급"]["Stage2"].avgCorrectAnswerCountForEachStage +
                       tmpStageDatas[id].stageData["초급"]["Stage3"].avgCorrectAnswerCountForEachStage) / 3;
                break;
            case 1:
                avg = (tmpStageDatas[id].stageData["중급"]["Stage1"].avgCorrectAnswerCountForEachStage +
                       tmpStageDatas[id].stageData["중급"]["Stage2"].avgCorrectAnswerCountForEachStage +
                       tmpStageDatas[id].stageData["중급"]["Stage3"].avgCorrectAnswerCountForEachStage) / 3;
                break;
            case 2:
                avg = (tmpStageDatas[id].stageData["고급"]["Stage1"].avgCorrectAnswerCountForEachStage +
                       tmpStageDatas[id].stageData["고급"]["Stage2"].avgCorrectAnswerCountForEachStage +
                       tmpStageDatas[id].stageData["고급"]["Stage3"].avgCorrectAnswerCountForEachStage) / 3;

                break;
            default:
                avg = 0;
                Debug.Assert(false, "이상한 레벨, 평균반응시간 못구함");
                break;
        }

        return avg;
    }

    public void SaveGameStageData(EGameName eGameName, string id, int level, int stage, int score)
    {
        tmpStageDatas = chooseTmpData(eGameName);
        DataForEachStage tmp = new DataForEachStage();
        string levelString = "";
        string stepString = "";
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

        switch (stage)
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


        //playCnt++;
        avgCorrectAnswerCnt = (score + (playCnt - 1) * avgCorrectAnswerCnt) / (playCnt);
        avgResponseTimeForEachStage
            = (totalResponseTimeForEachStage + (playCnt - 1) * avgResponseTimeForEachStage) /
              (playCnt * TOTAL_QUESTION_CNT);


        string jdata = "";
        switch (eGameName)
        {
            case EGameName.Detection:
                tmp.playCnt = playCnt;
                tmp.avgCorrectAnswerCountForEachStage = avgCorrectAnswerCnt;
                tmp.avgResponseTimeForEachStage = avgResponseTimeForEachStage;

                tmpStageDatas[id].stageData[levelString][stepString] = tmp;

                jdata = JsonConvert.SerializeObject(tmpStageDatas, Formatting.Indented);
                File.WriteAllText(detectionPath, jdata);
                break;

            case EGameName.Synthesis:
                tmp.playCnt = playCnt;
                tmp.avgCorrectAnswerCountForEachStage = avgCorrectAnswerCnt;
                tmp.avgResponseTimeForEachStage = avgResponseTimeForEachStage;

                tmpStageDatas[id].stageData[levelString][stepString] = tmp;

                jdata = JsonConvert.SerializeObject(tmpStageDatas, Formatting.Indented);
                File.WriteAllText(synthesisPath, jdata);
                break;

            case EGameName.Elimination:
                tmp.playCnt = playCnt;
                tmp.avgCorrectAnswerCountForEachStage = avgCorrectAnswerCnt;
                tmp.avgResponseTimeForEachStage = avgResponseTimeForEachStage;

                tmpStageDatas[id].stageData[levelString][stepString] = tmp;

                jdata = JsonConvert.SerializeObject(tmpStageDatas, Formatting.Indented);
                File.WriteAllText(eliminationPath, jdata);
                break;

            case EGameName.Alternative:
                tmp.playCnt = playCnt;
                tmp.avgCorrectAnswerCountForEachStage = avgCorrectAnswerCnt;
                tmp.avgResponseTimeForEachStage = avgResponseTimeForEachStage;

                tmpStageDatas[id].stageData[levelString][stepString] = tmp;

                jdata = JsonConvert.SerializeObject(tmpStageDatas, Formatting.Indented);
                File.WriteAllText(alternativePath, jdata);
                break;
        }
    }
}
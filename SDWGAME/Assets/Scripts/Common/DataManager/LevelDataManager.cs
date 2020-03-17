using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NestedDictionaryLib;
using Newtonsoft.Json;
using UnityEngine;

class DataForEachLevel
{
    public int[] obtainedStarCountForEachLevel; //gamename, level, step, obtained star 개수
    public float avgPerfectionForEachLevel;
    public float avgResponseTimeForEachLevel;
    public float IESforEachLevel;

    public DataForEachLevel()
    {
        obtainedStarCountForEachLevel = new int[3];
        avgPerfectionForEachLevel = 0.0f;
        avgResponseTimeForEachLevel = 0.0f;
        IESforEachLevel = 0.0f;
    }
}

class LevelDataForEachUser
{
    public Dictionary<string, DataForEachLevel> levelData;

    public LevelDataForEachUser()
    {
        levelData = new Dictionary<string, DataForEachLevel>()
        {
            {"초급", new DataForEachLevel()},
            {"중급", new DataForEachLevel()},
            {"고급", new DataForEachLevel()}
        };
    }
}

public class LevelDataManager : MonoBehaviour
{
    private string detectionLevelDataPath;
    private Dictionary<string, LevelDataForEachUser> tmpDetectionLevelData;

    private string synthesisLevelDataPath;
    private Dictionary<string, LevelDataForEachUser> tmpSynthesisLevelData;

    private string eliminationDataPath;
    private Dictionary<string, LevelDataForEachUser> tmpEliminationLevelData;

    private string alternativeDataPath;
    private Dictionary<string, LevelDataForEachUser> tmpAlternativeLevelData;

    public int[,] obtainedStarCnt; //한게임 레벨의 스테이지 int[3,3]
    public float[] avgPerfection;
    public float[] avgResponseTime;
    public float[,] IES;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        obtainedStarCnt = new int[3, 3];
        avgPerfection = new float[3];
        avgResponseTime = new float[3];
        IES = new float[3, 3];


        detectionLevelDataPath = Path.Combine(Application.persistentDataPath, "DetectionLevelData.json");
        synthesisLevelDataPath = Path.Combine(Application.persistentDataPath, "SynthesisLevelData.json");
        eliminationDataPath = Path.Combine(Application.persistentDataPath, "eliminationLevelData.json");
        alternativeDataPath = Path.Combine(Application.persistentDataPath, "alternativeLevelData.json");

        if (!File.Exists(detectionLevelDataPath))
        {
            tmpDetectionLevelData = new Dictionary<string, LevelDataForEachUser>();
            tmpDetectionLevelData.Add("initial", new LevelDataForEachUser());
            string tmpJdata = JsonConvert.SerializeObject(tmpDetectionLevelData, Formatting.Indented);
            File.WriteAllText(detectionLevelDataPath, tmpJdata);
            File.WriteAllText(synthesisLevelDataPath, tmpJdata);
            File.WriteAllText(eliminationDataPath, tmpJdata);
            File.WriteAllText(alternativeDataPath, tmpJdata);
        }


        //Load data
        string jdata = File.ReadAllText(detectionLevelDataPath);
        tmpDetectionLevelData = JsonConvert.DeserializeObject<Dictionary<string, LevelDataForEachUser>>(jdata);
        jdata = File.ReadAllText(synthesisLevelDataPath);
        tmpSynthesisLevelData = JsonConvert.DeserializeObject<Dictionary<string, LevelDataForEachUser>>(jdata);
        jdata = File.ReadAllText(eliminationDataPath);
        tmpEliminationLevelData = JsonConvert.DeserializeObject<Dictionary<string, LevelDataForEachUser>>(jdata);
        jdata = File.ReadAllText(alternativeDataPath);
        tmpAlternativeLevelData = JsonConvert.DeserializeObject<Dictionary<string, LevelDataForEachUser>>(jdata);
    }

    public int[,] LoadLevelSceneStar(EGameName eGameName, string id)
    {
        string levelString = "";

        for (int level = 0; level < 3; level++)
        {
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

            switch (eGameName)
            {
                case EGameName.Detection:
                    obtainedStarCnt[level, 0] =
                        tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[0];
                    obtainedStarCnt[level, 1] =
                        tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[1];
                    obtainedStarCnt[level, 2] =
                        tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[2];
                    break;
                case EGameName.Synthesis:
                    obtainedStarCnt[level, 0] =
                        tmpSynthesisLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[0];
                    obtainedStarCnt[level, 1] =
                        tmpSynthesisLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[1];
                    obtainedStarCnt[level, 2] =
                        tmpSynthesisLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[2];
                    break;
                case EGameName.Elimination:
                    obtainedStarCnt[level, 0] =
                        tmpEliminationLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[0];
                    obtainedStarCnt[level, 1] =
                        tmpEliminationLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[1];
                    obtainedStarCnt[level, 2] =
                        tmpEliminationLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[2];
                    break;
                case EGameName.Alternative:
                    obtainedStarCnt[level, 0] =
                        tmpAlternativeLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[0];
                    obtainedStarCnt[level, 1] =
                        tmpAlternativeLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[1];
                    obtainedStarCnt[level, 2] =
                        tmpAlternativeLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[2];
                    break;
                default:
                    Debug.Assert(false, "레벨 스타 불러오기 실패");
                    break;
            }
        }

        return obtainedStarCnt;
    }

    public bool makeNewId(string id)
    {
        if (!tmpDetectionLevelData.ContainsKey(id))
        {
            LevelDataForEachUser tmp = new LevelDataForEachUser();
            tmpDetectionLevelData[id] = tmp;
            tmpSynthesisLevelData[id] = tmp;
            tmpEliminationLevelData[id] = tmp;
            tmpAlternativeLevelData[id] = tmp;

            string jdata = JsonConvert.SerializeObject(tmpDetectionLevelData, Formatting.Indented);
            File.WriteAllText(detectionLevelDataPath, jdata);

            jdata = JsonConvert.SerializeObject(tmpSynthesisLevelData, Formatting.Indented);
            File.WriteAllText(synthesisLevelDataPath, jdata);

            jdata = JsonConvert.SerializeObject(tmpEliminationLevelData, Formatting.Indented);
            File.WriteAllText(eliminationDataPath, jdata);

            jdata = JsonConvert.SerializeObject(tmpAlternativeLevelData, Formatting.Indented);
            File.WriteAllText(alternativeDataPath, jdata);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void deleteLevelInfoOfCurrentId(string id)
    {
        if (tmpDetectionLevelData.Remove(id))
        {
            tmpSynthesisLevelData.Remove(id);
            tmpEliminationLevelData.Remove(id);
            tmpAlternativeLevelData.Remove(id);
            Debug.Log("removed");
        }
        else
        {
            Debug.Log("No such Id");
        }

        string jdata = JsonConvert.SerializeObject(tmpDetectionLevelData, Formatting.Indented);
        File.WriteAllText(detectionLevelDataPath, jdata);

        jdata = JsonConvert.SerializeObject(tmpSynthesisLevelData, Formatting.Indented);
        File.WriteAllText(synthesisLevelDataPath, jdata);

        jdata = JsonConvert.SerializeObject(tmpEliminationLevelData, Formatting.Indented);
        File.WriteAllText(eliminationDataPath, jdata);

        jdata = JsonConvert.SerializeObject(tmpAlternativeLevelData, Formatting.Indented);
        File.WriteAllText(alternativeDataPath, jdata);
    }

    public void SaveLevelData(EGameName eGameName, string id, int level)
    {
        string levelString = "";

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

        string jdata = "";
        switch (eGameName)
        {
            case EGameName.Detection:
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[0] =
                    obtainedStarCnt[level, 0];
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[1] =
                    obtainedStarCnt[level, 1];
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[2] =
                    obtainedStarCnt[level, 2];

                float a, b;
                a = tmpDetectionLevelData[id].levelData[levelString].avgPerfectionForEachLevel =
                    avgPerfection[level];
                b = tmpDetectionLevelData[id].levelData[levelString].avgResponseTimeForEachLevel =
                    avgResponseTime[level];
                tmpDetectionLevelData[id].levelData[levelString].IESforEachLevel = a / b;

                jdata = JsonConvert.SerializeObject(tmpDetectionLevelData, Formatting.Indented);
                File.WriteAllText(detectionLevelDataPath, jdata);

                break;
            case EGameName.Synthesis:
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[0] =
                    obtainedStarCnt[level, 0];
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[1] =
                    obtainedStarCnt[level, 1];
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[2] =
                    obtainedStarCnt[level, 2];

                a = tmpDetectionLevelData[id].levelData[levelString].avgPerfectionForEachLevel = avgPerfection[level];
                b = tmpDetectionLevelData[id].levelData[levelString].avgResponseTimeForEachLevel =
                    avgResponseTime[level];
                tmpDetectionLevelData[id].levelData[levelString].IESforEachLevel = a / b;

                jdata = JsonConvert.SerializeObject(tmpSynthesisLevelData, Formatting.Indented);
                File.WriteAllText(synthesisLevelDataPath, jdata);

                break;
            case EGameName.Elimination:
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[0] =
                    obtainedStarCnt[level, 0];
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[1] =
                    obtainedStarCnt[level, 1];
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[2] =
                    obtainedStarCnt[level, 2];

                a = tmpDetectionLevelData[id].levelData[levelString].avgPerfectionForEachLevel =
                    avgPerfection[level]; //= 
                b = tmpDetectionLevelData[id].levelData[levelString].avgResponseTimeForEachLevel =
                    avgResponseTime[level]; //= 
                tmpDetectionLevelData[id].levelData[levelString].IESforEachLevel = a / b;

                jdata = JsonConvert.SerializeObject(tmpEliminationLevelData, Formatting.Indented);
                File.WriteAllText(eliminationDataPath, jdata);

                break;

            case EGameName.Alternative:
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[0] =
                    obtainedStarCnt[level, 0];
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[1] =
                    obtainedStarCnt[level, 1];
                tmpDetectionLevelData[id].levelData[levelString].obtainedStarCountForEachLevel[2] =
                    obtainedStarCnt[level, 2];

                a = tmpDetectionLevelData[id].levelData[levelString].avgPerfectionForEachLevel =
                    avgPerfection[level]; //= 
                b = tmpDetectionLevelData[id].levelData[levelString].avgResponseTimeForEachLevel =
                    avgResponseTime[level]; //= 
                tmpDetectionLevelData[id].levelData[levelString].IESforEachLevel = a / b;

                jdata = JsonConvert.SerializeObject(alternativeDataPath, Formatting.Indented);
                File.WriteAllText(alternativeDataPath, jdata);

                break;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;

class userStageData
{
    public string userId; //학습자 ID
    public int level; //난이도
    public int stageIndex; //문제 번호
    public int corrAnsIndex; // 정답 보기 번호
    public List<int> chosenAnsIndex; //반응 보기 번호
    public bool isUserRight; //정오표시(60초가 지날 때까지 정답을 고르지 못하면 오답)
    public float responseTime; // 반응시간
    // 자극 유형/ 현재 문제에서 제시한 자극의 유형/숫자
    public int cntClick; //정답을 클릭하기까지 클릭 횟수
    
}
public class DataManager : MonoBehaviour
{
    public void Start()
    {
        
    }

    public void Save()
    {
        
    }

    public void Load()
    {
        
    }
}

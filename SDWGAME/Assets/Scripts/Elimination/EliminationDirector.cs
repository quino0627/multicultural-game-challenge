using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EliminationDirector : MonoBehaviour
{
    private GameObject timerText;

    private GameObject pointText;

    private GameObject stageText;

    private GameObject levelText;
    
    // 제한시간 10초
    private float time = 60.0f;
    // 현재 포인트
    private static int point = 0;
    // 현재 stage
    private int stage = 0;
    // 현재 level
    // l
    private string level = "default value";
    
    public void GetPoint(int po)
    {
        point = point + po;
    }

    public void SetTime(float ti)
    {
        this.time = Convert.ToSingle(System.Math.Truncate(Convert.ToDouble(60f - ti)));
    }

    public void InitTime()
    {
        this.time = 60f;
    }

    public void setStage(int st)
    {
        this.stage = st;
    }

    public void setLevel(int le)
    {
        if (le == 0)
        {
            this.level = "쉬움";
        }

        if (le == 1)
        {
            this.level = "보통";
        }

        if (le == 2)
        {
            this.level = "어려움";
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        this.timerText = GameObject.Find("Time");
        this.pointText = GameObject.Find("Score");
        this.stageText = GameObject.Find("Stage");
        this.levelText = GameObject.Find("Level");
        this.InitTime();
    }

    // Update is called once per frame
    void Update()
    {
//        this.pointText.GetComponent<TextMeshProUGUI>().text = point.ToString() + "점";
        this.stageText.GetComponent<TextMeshProUGUI>().text = $"{this.stage + 1}단계";
        this.levelText.GetComponent<TextMeshProUGUI>().text = level;
//        this.timerText.GetComponent<TextMeshProUGUI>().text = time.ToString("F")+ "초";
        GameObject.Find("UIP_ClockBar").GetComponent<TimerScript>().remainedTime = time;

    }
}

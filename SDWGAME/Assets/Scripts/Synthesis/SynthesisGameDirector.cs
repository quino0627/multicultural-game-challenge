using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SynthesisGameDirector : MonoBehaviour
{

    public PauseHandler m_Pause = null;

    private GameObject stageText;

    private GameObject levelText;

    // 제한시간 10초
    private float time = 60.0f;
    // 현재 포인트
    private int point = 0;
    // 현재 stage
    private int stage = 0;
    // 현재 level
    private string level = "default value";
    
    
    // Start is called before the first frame update
    void Start()
    {
        this.stageText = GameObject.Find("Stage");
        this.levelText = GameObject.Find("Level");
        this.InitTime();
    }

    // Update is called once per frame
    void Update()
    {
        this.stageText.GetComponent<TextMeshProUGUI>().text = $"{this.stage + 1}단계";
        // 두글자인 경우 stage+maxLevel0Stage를 더해야하는 코드 추가해됨.

        this.levelText.GetComponent<TextMeshProUGUI>().text = level;
        GameObject.Find("UIP_ClockBar").GetComponent<TimerScript>().remainedTime = time;
    }

    public void GetPoint(int po)
    {
        this.point = this.point + po;
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
        Debug.Log(le);
        if (le == 0)
        {
            this.level = "쉬움";
        }
        if (le == 1)
        {
            this.level = "쉬움";
        }

        if (le == 2)
        {
            this.level = "보통";
        }
        
        if (le == 3)
        {
            this.level = "어려움";
        }
    }
}

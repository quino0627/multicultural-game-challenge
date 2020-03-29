using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlternativeGameDirector : MonoBehaviour
{
    public PauseHandler m_Pause = null;
    
    private GameObject timerText;

    private GameObject pointText;

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
    
    // 사운드를 내기 위한 플래그
    
    // 5,4,3,2,1 초일 때마다 경고음
    private Boolean[] hurryOutFlags = new Boolean[5];
    // 게임이 0초가 되어 끝날 때 효과음
    private Boolean timeOverFlag;
    
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
        for (int i = 0; i < hurryOutFlags.Length; i++)
        {
            hurryOutFlags[i] = false;
        }

        timeOverFlag = false;
        this.InitTime();
    }

    // Update is called once per frame
    void Update()
    {
        this.stageText.GetComponent<TextMeshProUGUI>().text = $"{this.stage + 1}단계";
        this.levelText.GetComponent<TextMeshProUGUI>().text = level;
        GameObject.Find("UIP_ClockBar").GetComponent<TimerScript>().remainedTime = time;
        
        // 5초일 때부터 5,4,3,2,1 일 때마다 hurryOutTimeSound를 한번씩 냅니다.
        if (time == 5f)
        {
            if (!hurryOutFlags[4])
            {
                SoundManager.Instance.Play_HurryOutTimeSound();
            }

            timeOverFlag = false;
            hurryOutFlags[4] = true;
        }

        if (time == 4)
        {
            if (!hurryOutFlags[3])
            {
                SoundManager.Instance.Play_HurryOutTimeSound();
            }

            hurryOutFlags[3] = true;
        }

        if (time == 3)
        {
            if (!hurryOutFlags[2])
            {
                SoundManager.Instance.Play_HurryOutTimeSound();
            }

            hurryOutFlags[2] = true;
        }

        if (time == 2)
        {
            if (!hurryOutFlags[1])
            {
                SoundManager.Instance.Play_HurryOutTimeSound();
            }

            hurryOutFlags[1] = true;
        }

        if (time == 1)
        {
            if (!hurryOutFlags[0])
            {
                SoundManager.Instance.Play_HurryOutTimeSound();
            }

            hurryOutFlags[0] = true;
        }

        if (time == 0)
        {
            if (!timeOverFlag)
            {
                SoundManager.Instance.Play_TimeOverSound();
            }

            timeOverFlag = true;
            
            // 다른 플래그들은 여기서 초기화해주면 다음 문제에서 정상적으로 동작 가능함
            for (int i = 0; i < hurryOutFlags.Length; i++)
            {
                hurryOutFlags[i] = false;
            }
        }
    }
    
    // 일시정지 창 띄우기
    public void openPause()
    {
        m_Pause.Show();
    }
}
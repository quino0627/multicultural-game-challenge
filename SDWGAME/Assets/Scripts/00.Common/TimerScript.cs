using System;
using UnityEngine;
using UnityEngine.UI;


public class TimerScript : MonoBehaviour
{
    private Slider slTimer;
    public Text timeText;
    public float remainedTime;

    // 사운드를 내기 위한 플래그
    
    // 5,4,3,2,1 초일 때마다 경고음
    private Boolean[] hurryOutFlags = new Boolean[5];
    // 게임이 0초가 되어 끝날 때 효과음
    private Boolean timeOverFlag;
    
    // Start is called before the first frame update
    void Start()
    {
        slTimer = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slTimer.value = remainedTime;
        timeText.text = remainedTime.ToString();
        
//        if (remainedTime == 5f)
//        {
//            if (!hurryOutFlags[4])
//            {
//                SoundManager.Instance.Play_HurryOutTimeSound();
//            }
//
//            timeOverFlag = false;
//            hurryOutFlags[4] = true;
//        }
//        
//        
//        if (remainedTime == 4)
//        {
//            if (!hurryOutFlags[3])
//            {
//                SoundManager.Instance.Play_HurryOutTimeSound();
//            }
//
//            hurryOutFlags[3] = true;
//        }
//        
//        if (remainedTime == 3)
//        {
//            if (!hurryOutFlags[2])
//            {
//                SoundManager.Instance.Play_HurryOutTimeSound();
//            }
//
//            hurryOutFlags[2] = true;
//        }
//        
//        if (remainedTime == 2)
//        {
//            if (!hurryOutFlags[1])
//            {
//                SoundManager.Instance.Play_HurryOutTimeSound();
//            }
//
//            hurryOutFlags[1] = true;
//        }
//        
//        if (remainedTime == 1)
//        {
//            if (!hurryOutFlags[0])
//            {
//                SoundManager.Instance.Play_HurryOutTimeSound();
//            }
//
//            hurryOutFlags[0] = true;
//        }
//        
//        if (remainedTime == 0)
//        {
//            if (!timeOverFlag)
//            {
//                SoundManager.Instance.Play_TimeOverSound();
//            }
//
//            timeOverFlag = true;
//            
//            // 다른 플래그들은 여기서 초기화해주면 다음 문제에서 정상적으로 동작 가능함
//            for (int i = 0; i < hurryOutFlags.Length; i++)
//            {
//                hurryOutFlags[i] = false;
//            }
//        }


    }
}

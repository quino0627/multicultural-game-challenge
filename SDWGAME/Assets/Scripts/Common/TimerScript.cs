using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;


public class TimerScript : MonoBehaviour
{
    private Slider slTimer;
    public Text timeText;
    public float remainedTime;

    
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
    }
}

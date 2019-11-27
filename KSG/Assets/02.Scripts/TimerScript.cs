using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class TimerScript : MonoBehaviour
{
    public static Stopwatch sw = new Stopwatch();

    void OnEnable()
    {
        if(gameObject.name != "Quiz1")
        StartCoroutine(StopWatchStart());
    }

    public static IEnumerator StopWatchStart()
    {
        sw.Start();
        while (true)
        {
            yield return null;
        }
    }

    public void StartTimerAfterParsing()
    {
        StartCoroutine(StopWatchStart());
    }
}
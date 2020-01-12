using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    public bool shouldStart;
    private Slider slTimer;
    private float fSliderBarTime;

    // Start is called before the first frame update
    void Start()
    {
        slTimer = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //각 director마다
        //이 컴포넌트를 들고와서 shouldStart를 true로 설정
        //watch.Start()와 같이
        if (shouldStart)
        {
            if (slTimer.value > 0.0f)
            {
                //시간이 변경한 만큼 slider Value 변경을 합니다.
                slTimer.value -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time is Zero.");
            }
        }
    }
}

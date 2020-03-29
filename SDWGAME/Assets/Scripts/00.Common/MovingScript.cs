using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingScript : MonoBehaviour {
    public float acc = 0.001f, defaultSpeed =  0.05f;
    int var = 1, var2 = 1;
    float speed = 0;
    public int cnt = 0, cntMax = 200;
    private void Start()
    {
        speed = defaultSpeed;
    }

    // Update is called once per frame 
    void Update()
    {
        if(Time.timeScale == 0)return;
        
        cnt += var;
        speed += acc * var2;
        this.transform.Translate(new Vector3(0, speed* var*-1, 0));  //이동
        if (cnt % (cntMax/2) == 0) var2 = var2 * -1;
        if (cnt == cntMax)
        {
            var = -1;
            speed = defaultSpeed;
        }
        else if (cnt == 0)
        {
            var = 1;
            speed = defaultSpeed;
        }
    }


}
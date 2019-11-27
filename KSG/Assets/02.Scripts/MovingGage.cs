using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingGage : MonoBehaviour {
    public float Star_1_Score, Star_2_Score, Star_3_Score;
    private bool Star_1_Pass = true, Star_2_Pass = true, Star_3_Pass = true;
    public GameObject Star_1, Star_2, Star_3;
    public Sprite Star;

    private void Awake()
    {
        
    }

    private void Start()
    {

    }

    // Update is called once per frame 
    void Update()
    {

    }

    public void AnimateGage(float lim, float fadeOutTime, System.Action nextEvent = null)
    {
        StartCoroutine(GageAni(lim, fadeOutTime, nextEvent));
    }

    IEnumerator GageAni(float limit, float fadeOutTime, System.Action nextEvent = null)
    {
        Slider sl = this.GetComponent<Slider>();
        while (sl.value < limit)
        {
            sl.value += 0.2f*Time.deltaTime / fadeOutTime;

            yield return null;

            if (sl.value >= Star_1_Score && Star_1_Pass)
            {
                Star_1_Pass = false;
                Star_1.GetComponent<Image>().sprite = Star;
            }
            if (sl.value >= Star_2_Score && Star_2_Pass)
            {
                Star_2_Pass = false;
                Star_2.GetComponent<Image>().sprite = Star;
            }
            if (sl.value >= Star_3_Score && Star_3_Pass)
            {
                Star_3_Pass = false;
                Star_3.GetComponent<Image>().sprite = Star;
            }
        }
    }
}

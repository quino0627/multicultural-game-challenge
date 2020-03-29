using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeahorseLeftController : MonoBehaviour
{
    private Animator animator;
//    private Animation _animation;
    public Animator waterFallAnimator;
    
//    public GameObject _WaterFallAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.animator.speed = .25f;
        this.waterFallAnimator = transform.Find("WaterFallAnimation").GetComponent<Animator>();
        Debug.Log(waterFallAnimator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
  
}

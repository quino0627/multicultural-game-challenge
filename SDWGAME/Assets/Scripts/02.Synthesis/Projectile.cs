using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] 
    public Transform[] TargetObjectTF;
    public Vector2 stopPosition; //목표지점
    public int offset = 0;
    public Transform AnswerTransform;
    //[Range(1.0f, 6.0f)] public float TargetRadius;
    [Range(20.0f, 75.0f)] public float LaunchAngle;

    public float ModifyTargetY;
   
    //state
    private bool bTargetReady;
    public bool bump1stTime; 
    public bool justJump;
    //cache
    private Rigidbody2D rigid;
    private Vector2 initialPosition;
    private Quaternion initialRotation;
    
    
    // Initializaion
    void Start()
    {
        for(int i =0; i<TargetObjectTF.Length; i++)
        {
            TargetObjectTF[i] = GameObject.FindWithTag("answer").transform;
        }
        rigid = GetComponent<Rigidbody2D>();
        bTargetReady = true;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        stopPosition = TargetObjectTF[0].position;
    }

    //Launches the object towards the TargetObject with a given LaunchAngle
    public void Launch()
    {
        

        Vector2 projectileXZPos = new Vector2(transform.position.x, 0.0f);
        Vector2 targetXZPos = new Vector2(stopPosition.x,0.0f);
        //transform.LookAt(targetXZPos);
        //Debug.Log("lookat" + transform.rotation);
        Debug.Log("transform's position:"+transform.position);
        Debug.Log("TargetObj's position: " + stopPosition);
        
        // shorthands for the formula
        float R = Vector2.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = (stopPosition.y + 0.0f) - transform.position.y;
        Debug.Log(R+" "+G+" "+tanAlpha+" "+H);
        // calculate initial speed required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)) );
        float Vy = tanAlpha * Vz;
        Debug.Log("(Vx, Vy) = " + Vz + ", " + Vy);
        // create the velocity vector in local space and get it in global space
        Vector2 localVelocity = new Vector2(Vz, Vy);
        Vector2 globalVelocity = transform.TransformDirection(localVelocity);

        Debug.Log(localVelocity);
        Debug.Log(globalVelocity);
        
        
        // launch the object by setting its initial velocity and flipping its state
        rigid.velocity = globalVelocity;
        //rigid.velocity = localVelocity;
        
        
        bTargetReady = false;
        
    }
    
    //Sets a random target around the object based on the TargetRadius
    void SetNewTarget()
    {
        //GameObject tar = GameObject.Find("AnswerPlatform");
        //TargetObjectTF.SetPositionAndRotation(tar.transform.position,tar.transform.rotation);
        stopPosition = TargetObjectTF[++offset].position;
        bTargetReady = true;
    }

    //Resets the projectile to its initial position
    void ResetToInitialState()
    {
        rigid.velocity = Vector2.zero;
        transform.SetPositionAndRotation(initialPosition,initialRotation);
        bTargetReady = false;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (justJump) //just jump
        {
            if(bTargetReady){Launch();}
        }
        
        
        

        //crab가 목표지점에 점프하여 도달하면 멈춤.
        if (Math.Abs(transform.position.x - stopPosition.x) <= 0.2f &&
            Math.Abs(transform.position.y - stopPosition.y) <= 0.2f)
        {
            Rigidbody2D rigid = gameObject.GetComponent<Rigidbody2D>();
            rigid.velocity = Vector2.zero;
            bump1stTime = true;
        }
        
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name == "Jellyfish" && bump1stTime)
        {
            transform.SetParent(AnswerTransform);
            StageClear script2 = GameObject.Find("AnswerPlatform").GetComponent<StageClear>();
            script2.numAns++;
            bump1stTime = false;
        }
    }
}

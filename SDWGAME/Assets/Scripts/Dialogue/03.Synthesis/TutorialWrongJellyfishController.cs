using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWrongJellyfishController : MonoBehaviour
{
    
    public float speed = 2f;
    public bool onCircle;
    public GameObject Spark;
    public bool sparked;
    public Transform InitialTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; //움직이는 속도
        float fly = 2 * speed * Time.deltaTime; //빠른 속도...
        
        if (!onCircle) //onCircle은 DragAndDrop.cs에서 설정해줌
        {
            return; //해파리가 circle에 놓여져 있지 않으면 return
        }
        
        StartCoroutine(ChoseWrongAnswer(fly));
        Debug.Log("WRONG!!!");
    }
    
    IEnumerator ChoseWrongAnswer(float step)
    {
        
        
        //spark animation
        if (!sparked)
        {
            SoundManager.Instance.Play_JellyFishShocked();
            // totaltried ++
            
            Instantiate(Spark, transform.position, Quaternion.identity);
            sparked = true;
            
        }
        
        yield return new WaitForSeconds(1.2f);
        
        // move toward initial position
        transform.position = Vector2.MoveTowards(transform.position,
            InitialTransform.position, step);

        
        //다시 도착했을때
        float distance = Vector2.Distance(transform.position, InitialTransform.position);
        if (distance < 0.01f)
        {
            onCircle = false;
            sparked = false;
            
        }
    }
}



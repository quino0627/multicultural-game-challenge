using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrabMove : MonoBehaviour
{
    
    //MoveJellyfish.cs와 같이 보시길 권장합니다.
    private Animator animator;
    private Rigidbody2D rb2d;
    public GameObject jellyfish;
    private Vector2 clearPosition;
    private bool okNext; 
    public float speed = 1f;
    public float targetAngle = 80f;
    public float turnSpeed = 5f;

    
    public bool isAboarding;
    private bool foundJf;
    // Start is called before the first frame update
    void Start()
    {
        okNext = true;
        animator = GetComponent<Animator>();
        //jellyfish = GameObject.FindGameObjectWithTag("Carrier");
        clearPosition = GameObject.Find("ClearPosition").transform.position;
        rb2d = GetComponent<Rigidbody2D>();
        //Debug.Log("jellyfish: "+jellyfish);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!foundJf)
        {
            jellyfish = GameObject.FindGameObjectWithTag("Carrier");
            if (jellyfish != null)
            {
                foundJf = true;
            }
        } */
        
        //Debug.Log("crab's parent transform: " + transform.parent.name);

        /*
        In MoveJellyfish.cs
        1. if jellyfish was correct and it moved to crab
        2. it will trigger crab to walk
        그 코드는 애니메이션만 하게 하고 
        3. & 7. 이 코드는 실제로 이동하게 함
        */
        if (animator.GetFloat("WalkSpeed") > 0 )
        {
            Debug.Log("CrabMoving~");
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        
        // 4. when crab is safely on jellyfish, stop
        if (Mathf.Abs(transform.position.x - jellyfish.transform.position.x) <= 0.05f
            && isAboarding)
        {
            Debug.Log("Crab should stop...");
            animator.SetFloat("WalkSpeed",-1f);
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            transform.SetParent(jellyfish.transform);
            isAboarding = false;
        }

        // 7. when jellyfish stops crab walks
        
        // 8. when crab goes to clear position crab snaps and stand(rotate)
        if (Mathf.Abs(transform.position.x - clearPosition.x) <= 0.05f)
        {
            
            //transform.SetParent(null);
            animator.SetFloat("WalkSpeed", 0f);
            Debug.Log("Crab on Clear position");
            rb2d.velocity = Vector2.zero;

            animator.SetTrigger("Hurray");
            //rb2d.MoveRotation(rb2d.rotation + rotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0, 0, targetAngle),
                turnSpeed * Time.deltaTime
            );

            // 다음 스테이지로 이동
            if (okNext)
            {
                Invoke(nameof(NextStage), 0.0f);
                okNext = false;
                Debug.Log("Inoved Next Stage");
            }
        }



    }

    void NextStage()
    {
        GameObject quizmanager = GameObject.Find("QuizManager");
        quizmanager.GetComponent<SpreadChoices>().GoNext();
    }
}

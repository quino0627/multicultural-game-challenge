using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사인파형 움직임을 위한 스크립트
public class OctoSinusodialMove : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float frequency = 5f;

    [SerializeField] private float magnitude = 0.5f;

    [HideInInspector] private GameObject SpeechBubble;
    [HideInInspector] private GameObject BubbleText;
    
    private bool facingRight = true;
    // 이게 true가 되는 위치까지 이동하게 되면 게임이 시작한다.
    public bool originPosition = false;

    private Vector3 pos, localScale;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.Play_SoundOctopusMove();
        SpeechBubble = transform.Find("DescriptionBubble").gameObject;
        BubbleText = SpeechBubble.transform.Find("DescriptionText").gameObject;
        pos = transform.position;
        localScale = transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public IEnumerator MoveOctopus()
    {
        while (!originPosition)
        {
            yield return new WaitForEndOfFrame();
            CheckOriginPosition();
            CheckWhereToFace();
            ChangeBubbleTextFlipX();
            
                if (facingRight)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
            
        }
    }

    void CheckOriginPosition()
    {
        if(transform.position.x > -7 && transform.position.y < -2)
        {
            
            originPosition = true;
            SoundManager.Instance.StopMusic();
            Debug.Log("STOP MUSIC");
        }
    }

    void CheckWhereToFace()
    {
        if (pos.x < -7f)
        {
            facingRight = true;
        }
        else if(pos.x > 7f)
        {
            facingRight = false;
        }

        if ((facingRight && localScale.x < 0) || (!facingRight && localScale.x > 0))
        {
            localScale.x = localScale.x * -1;
        }

        transform.localScale = localScale;
    }

    void MoveRight()
    {
        pos = pos + transform.right * Time.deltaTime * moveSpeed;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
    }

    void MoveLeft()
    {
        pos = pos - transform.right * Time.deltaTime * moveSpeed;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
    }

    void ChangeBubbleTextFlipX()
    {
        Vector3 tmp_vector = BubbleText.GetComponent<RectTransform>().localScale;
        if ((facingRight && tmp_vector.x < 0) || (!facingRight && tmp_vector.x > 0))
        {
            tmp_vector.x = tmp_vector.x * -1;
        }
        
        BubbleText.GetComponent<RectTransform>().localScale = tmp_vector;
    }
}

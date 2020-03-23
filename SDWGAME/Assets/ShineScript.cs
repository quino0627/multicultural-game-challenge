using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;
using UnityEngine.EventSystems;


// 코드 작성자: 김후정
// 주석 작정사: 송동욱
public class ShineScript : MonoBehaviour
{
    // 최상단의 Detection, Alter, Syn, Eli에 붙어있는 클래스.
    // 여기에 isHovering이 있다.
    public SelectResponse parent;

    private SpriteGlowEffect glow;
    // Start is called before the first frame update
    void Start()
    {
        //parent = gameObject.GetComponentInParent<SelectResponse>();
        glow = gameObject.GetComponent<SpriteGlowEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (parent.isHovering)
        {
            glow.GlowBrightness = 4;
        }
        else
        {
            glow.GlowBrightness = 1;
        }
    }
}

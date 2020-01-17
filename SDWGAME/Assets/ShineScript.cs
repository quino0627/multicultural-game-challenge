using System.Collections;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;

public class ShineScript : MonoBehaviour
{
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

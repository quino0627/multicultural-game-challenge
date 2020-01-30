using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuUIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Show());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator Show()
    {
        //Debug.Log("ASDF");
        // Play MoveIn animation
        GSui.Instance.MoveIn(this.transform, true);

        // Creates a yield instruction to wait for a given number of seconds
        // http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
        yield return new WaitForSeconds(1.0f);

        // Play particles in the hierarchy of given transfrom
        GSui.Instance.PlayParticle(this.transform);
    }
}

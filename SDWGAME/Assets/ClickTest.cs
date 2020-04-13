using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("CRAB CLICLED");
            ListRaycasts();
        }
    }
    
    private void ListRaycasts()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        int i = 0;
        while (i < hits.Length)
        {
            RaycastHit hit = hits[i];
            Debug.Log(hit.collider.gameObject.name);
            hit.collider.gameObject.SendMessage("OnMouseDown");
            i++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//    public GameObject Panel;

    public void OpenPanel(GameObject Panel)
    {
        if (!Panel != null)
        {
            Animator animator = Panel.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);
            }
        }
    }
    
    public void OpenTreasureChest(GameObject Chest)
    {
        if (!Chest != null)
        {
            Animator animator = Chest.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("toTreasure", true);
            }
        }
    }

    public void OpenEmptyChest(GameObject Chest)
    {
        if (!Chest != null)
        {
            Animator animator = Chest.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("toEmpty", true);
            }
        }
    }
    
    
    
}

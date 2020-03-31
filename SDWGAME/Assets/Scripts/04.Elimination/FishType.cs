using TMPro;
using UnityEngine;

public class FishType : MonoBehaviour
{
    private GameObject totalStorageObject;
    private TotalDataManager totalStorageScript;

    public Sprite Fish1IdleRedSprite;
    public Sprite Fish2IdleGreenSprite;
    public Sprite Fish4IdleYellowSprite;

    public RuntimeAnimatorController fish1IdleRedAnimatorController;
    public RuntimeAnimatorController fish2IdleGreenAnimatorController;
    public RuntimeAnimatorController fish4IdleYellowAnimatorController;

    public bool bEscapeScene;
    
    public GameObject[] choices;
    public int NUM_OF_CHOICES;
    private int level;


    void Awake()
    {
        totalStorageObject = GameObject.Find("TotalStorage");
        totalStorageScript = totalStorageObject.GetComponent<TotalDataManager>();

        level = totalStorageScript.chosenLevel;

        switch (level)
        {
            case 0:
                for (int i = 0; i < NUM_OF_CHOICES; i++)
                {
                    choices[i].transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite =
                        Fish4IdleYellowSprite;
                    choices[i].transform.GetChild(2).gameObject.GetComponent<Animator>().runtimeAnimatorController =
                        fish4IdleYellowAnimatorController;

                    choices[i].transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                    if (bEscapeScene)
                    {
                        choices[i].GetComponentInChildren<TextMeshPro>().color = Color.black;
                    }
                    else
                    {
                        choices[i].GetComponentInChildren<TextMeshPro>().color = new Color(0,0,0,0); 
                    }
                }

                break;
            case 1:
                for (int i = 0; i < NUM_OF_CHOICES; i++)
                {
                    choices[i].transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite =
                        Fish1IdleRedSprite;
                    choices[i].transform.GetChild(2).gameObject.GetComponent<Animator>().runtimeAnimatorController =
                        fish1IdleRedAnimatorController;

                    choices[i].transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                    if (bEscapeScene)
                    {
                        choices[i].GetComponentInChildren<TextMeshPro>().color = Color.white;
                    }
                    else
                    {
                        choices[i].GetComponentInChildren<TextMeshPro>().color = new Color(1,1,1,0); 
                    }
                }

                break;
            case 2:
                for (int i = 0; i < NUM_OF_CHOICES; i++)
                {
                    choices[i].transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite =
                        Fish2IdleGreenSprite;
                    choices[i].transform.GetChild(2).gameObject.GetComponent<Animator>().runtimeAnimatorController =
                        fish2IdleGreenAnimatorController;

                    choices[i].transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                    if (bEscapeScene)
                    {
                        choices[i].GetComponentInChildren<TextMeshPro>().color = Color.red;
                    }
                    else
                    {
                        choices[i].GetComponentInChildren<TextMeshPro>().color = new Color(1,0,0,0);
                    }
                }

                break;
        }
    }
}

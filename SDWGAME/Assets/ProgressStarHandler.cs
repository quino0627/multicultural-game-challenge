using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;

public class ProgressStarHandler : MonoBehaviour
{
    private GameObject totalStorageObject;
    private TotalDataManager totalStorageScript;

    private GameObject levelStorage;
    private LevelDataManager levelStorageScript;

    private Slider slider;

    //Inspector에서 설정
    public EGameName chosenGame;
    public Image[] StarImage;

    private shinyEffectAnimationPlay _shinyEffectAnimationPlay;

    private int[,] starData; //[level(0/1/2),stage(0/1/2)]

    public bool bShow;

    public float[] progressAmount;

    public void Start()
    {
        totalStorageObject = GameObject.Find("TotalStorage");
        totalStorageScript = totalStorageObject.GetComponent<TotalDataManager>();

        levelStorage = GameObject.Find("LevelStorage");
        levelStorageScript = levelStorage.GetComponent<LevelDataManager>();

        
        //slider = GetComponent<Slider>();

        //progressAmount = new float[3];

        /*for (int i = 0; i < 3; i++)
        {
            progressAmount[i] = starData[i, 0] + starData[i, 1] + starData[i, 2];
        }*/

        starData = new int[3, 3];
    }

    public void LoadStarImages()
    {
        starData = levelStorageScript.LoadLevelSceneStar(chosenGame, totalStorageScript.currId);

        #region FillStarImage

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int progress = starData[i, j];
                Debug.Log($"starData[{i},{j}] = {progress}");
                if (progress > 0)
                {
                    if (progress == 4)
                    {
                        StarImage[3 * i + j].fillAmount = 1f;
                    }

                    else if (progress == 3)
                    {
                        StarImage[3 * i + j].fillAmount = 0.75f;
                    }

                    else if (progress == 2)
                    {
                        StarImage[3 * i + j].fillAmount = 0.5f;
                    }

                    else if (progress == 1)
                    {
                        StarImage[3 * i + j].fillAmount = 0.25f;
                    }

                    Debug.Log($"StarImage[ {3 * i + j} ] = {StarImage[3 * i + j].fillAmount}");
                    StarImage[3 * i + j].color = new Color(255f, 255f, 255f, 255f);
                    /*StarImage[3 * i + j].gameObject.GetComponent<ShinyEffectForUGUI>()
                        .Play(2, AnimatorUpdateMode.Normal);*/
                    StartCoroutine(ShineStarPlayLoop(StarImage[3 * i + j].GetComponent<ShinyEffectForUGUI>()));
                }
            }
        }

        //if starData is 0 then it does not shine nor opaque

        #endregion
    }


    IEnumerator ShineStarPlayLoop(ShinyEffectForUGUI shinyEffectForUgui)
    {
        Debug.Log("beforeLoop");
        while (bShow)
        {
            Debug.Log("WhileLoop");
            shinyEffectForUgui.Play(2, AnimatorUpdateMode.Normal);
            yield return new WaitForSeconds(2.5f);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanelHandler : UIPT_PRO_Demo_GUIPanel
{
    //Inspector창에서 드래그
    public ProgressStarHandler[] starHandlers;
    public shinyEffectAnimationPlay[] gameCharactersShinyEffectAnimationPlays;
    
    private const int NUMBER_OF_GAMES = 4;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowProgressPanelHandler()
    {
        
        for (int i = 0; i < NUMBER_OF_GAMES; i++)
        {
            starHandlers[i].bShow = true;
            starHandlers[i].LoadStarImages();
            gameCharactersShinyEffectAnimationPlays[i].bTriggerShine = true;
            
        }

        Show();
    }

    public void HideProgressPanelHandler()
    {
        for (int i = 0; i < NUMBER_OF_GAMES; i++)
        {
            starHandlers[i].bShow = false;
            gameCharactersShinyEffectAnimationPlays[i].bShine = false;

        }

        Hide();
    }
}
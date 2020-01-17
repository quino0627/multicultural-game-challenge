using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        //SceneManager.LoadScene("SelectMenu");
    }

    public void GoToResult()
    {
        //SceneManager.LoadScene("ResultScene");
    }

    public void ChangeSettings()
    {
        //환경설정 창 띄우기
        SceneManager.LoadScene("Scenes/Settings");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationQuitScript : MonoBehaviour
{
    public GameObject quitPage;

	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (quitPage.activeSelf == false)
                quitPage.SetActive(true);
            else
                quitPage.SetActive(false);
        }
	}

    public void OKBtnClicked()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) { // forelink insert
            Application.OpenURL("https://kukp.forelink-cloud.co.kr/moodle3/day.php?ver=1.1&type=quit");
        }
        else {
            Application.Quit();
        }
    }

    public void NOBtnClicked()
    {
        quitPage.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemButtons : MonoBehaviour
{
   public void GoToMenu()
   {
      SceneManager.LoadScene("Scenes/Menu");
      
   }
}

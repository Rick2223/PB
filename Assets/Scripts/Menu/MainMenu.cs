using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button LoadGameBTN;

    public void NewGame()
   {
        SceneManager.LoadScene("GameScene");
   }

   public void ExitGame()
   {
        Debug.Log("Quitting game");
        Application.Quit();
   }
}

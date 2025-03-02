using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private GameManager gameManager;
    public void PlayNormalMode()
    {
        gameManager.SetGameMode("Normal");
        SceneManager.LoadSceneAsync("Endless Runner");
    }

    // Update is called once per frame
    public void PlayTimeTrails()
    {
        gameManager.SetGameMode("Time Trial");
        SceneManager.LoadSceneAsync("Time Trials");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

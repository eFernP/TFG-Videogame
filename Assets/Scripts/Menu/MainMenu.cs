using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject MainMenuUI;
    public GameObject ControlsMenuUI;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void ShowControls()
    {
        MainMenuUI.SetActive(false);
        ControlsMenuUI.SetActive(true);
    }

    public void ReturnToMenu()
    {
        MainMenuUI.SetActive(true);
        ControlsMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

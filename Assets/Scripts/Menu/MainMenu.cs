using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public GameObject MainMenuUI;
    public GameObject ControlsMenuUI;

    private SaveData data;
    public GameObject continueText;

    void Start(){
        data = SaveSystem.LoadGame();

        if(data !=null){
            continueText.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
            continueText.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void PlayGame()
    {
        SaveSystem.SaveGame("MazeScene", false, Constants.SavePositions[0]);
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

    public void ContinueGame()
    {
        SceneManager.LoadScene(data.CurrentScene);
    }

    void onEnable(){
        Debug.Log("ENABLED AGAIN");
    }
}

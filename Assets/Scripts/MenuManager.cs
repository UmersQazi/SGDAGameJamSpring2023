using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel, helpMenuPanel, creditsPanel;


    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void HelpButton()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
        helpMenuPanel.SetActive(true);
    }

    public void CreditsButton()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        helpMenuPanel.SetActive(false);
    }

    public void MainMenuButton()
    {
        mainMenuPanel.SetActive(true);
        helpMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}

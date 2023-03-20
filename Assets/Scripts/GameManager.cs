using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Animator playerHorseAnimator;
    public ThirdPersonCam thirdPersonGroundCamera;
    public ThirdPersonPlayerController player;
    public Image reticleSprite, reticleSprite2;
    public Color noHitColor, hitColor, lightColor;
    public int arrowCount;
    public Text arrowText;

    public int targetCount = 10;


    public float timeLimit = 300f;

    [Header("Sound")]
    public AudioSource levelMusic;

    [Header("UI")]
    public GameObject reticleUI, reticleUI2;
    
    public GameObject pausePanel, timeUpPanel, winPanel;
    public GameObject timerUI;
    public Text timerText;
    public bool timeIsUp;
    public bool gameWon;

    // Start is called before the first frame update
    void Start()
    {
        timeIsUp = false;
        gameWon = false;
        levelMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetCount <= 0)
        {
            gameWon = true;
            WinGame();
        }


        if(timeLimit <= 0)
        {
            TimeUp();
        }

        timerText.text = "Time:" + timeLimit.ToString();
        if (!timeIsUp)
        {
            timeLimit -= Time.deltaTime;
        }
        else if(timeIsUp || gameWon)
        {
            timeLimit = 0;
        }
        arrowText.text = arrowCount.ToString();

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerHorseAnimator.SetBool("isRunning", true);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            playerHorseAnimator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }


        ReticleChange();


        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level");
    }
    public void PauseGame()
    {
        reticleUI.SetActive(false);
        reticleUI2.SetActive(false);
        timeUpPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void TimeUp()
    {
        reticleUI.SetActive(false);
        reticleUI2.SetActive(false);
        timeUpPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void WinGame()
    {
        reticleUI.SetActive(false);
        reticleUI2.SetActive(false);
        winPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }


    public void ContinueButton()
    {
        pausePanel.SetActive(false);
        reticleUI.SetActive(true);
        reticleUI2.SetActive(true);
        Time.timeScale = player.startTimeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenuButtonGM()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ReticleChange()
    {
        if (thirdPersonGroundCamera.canHit)
        {
            reticleSprite.color = hitColor;
            reticleSprite2.color = hitColor;
        }
        else if (thirdPersonGroundCamera.canDeluminate)
        {
            reticleSprite.color = lightColor;
            reticleSprite2.color = lightColor;
        }
        else
        {
           reticleSprite.color = noHitColor;
            reticleSprite2.color = noHitColor;
        }
    }

}

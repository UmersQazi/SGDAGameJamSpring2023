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
    public Image reticleSprite;
    public Color noHitColor, hitColor;
    public int arrowCount;
    public Text arrowText;

    public float timeLimit = 300f;

    [Header("Sound")]
    public AudioSource levelMusic;

    [Header("UI")]
    public GameObject reticleUI;
    public GameObject pausePanel;
    public GameObject timerUI;
    public Text timerText;
   

    // Start is called before the first frame update
    void Start()
    {
        levelMusic.Play();
        arrowCount = 5;
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = timeLimit.ToString();
        timeLimit -= Time.deltaTime;
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

    }

    public void PauseGame()
    {
        reticleUI.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }


    public void ContinueButton()
    {
        pausePanel.SetActive(false);
        reticleUI.SetActive(true);
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
        }
        else
        {
           reticleSprite.color = noHitColor;
        }
    }

}

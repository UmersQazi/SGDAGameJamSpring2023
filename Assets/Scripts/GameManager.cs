using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Animator playerHorseAnimator;
    public ThirdPersonCam thirdPersonGroundCamera;
    public ThirdPersonPlayerController player;
    public Image reticleSprite;
    public Color noHitColor, hitColor;
    public int arrowCount;
    public Text arrowText;
    // Start is called before the first frame update
    void Start()
    {
        arrowCount = 5;
    }

    // Update is called once per frame
    void Update()
    {
        arrowText.text = arrowCount.ToString();

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerHorseAnimator.SetBool("isRunning", true);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            playerHorseAnimator.SetBool("isRunning", false);
        }

        ReticleChange();

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

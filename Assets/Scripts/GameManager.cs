using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Animator playerHorseAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerHorseAnimator.SetBool("isRunning", true);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            playerHorseAnimator.SetBool("isRunning", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineSwitcher : MonoBehaviour
{
    //[SerializeField] private InputAction action;

    private Animator animator;
    public bool inGroundCamera = true;
    public ThirdPersonPlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //action.performed += _ => SwitchState();

    }

    public void SwitchState()
    {
        if (inGroundCamera)
        {
            Debug.Log("Switching Camera");
            animator.Play("Air Camera State");
        }
        else
        {
            animator.Play("Ground Camera State");
        }

        inGroundCamera = !inGroundCamera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

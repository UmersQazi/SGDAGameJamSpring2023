using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerController : MonoBehaviour
{

    [SerializeField] GameManager gameMan;

    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    float horizontalInput, verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool groundCheck;

    [Header("Jumping Data")]
    public GameObject playerObj;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Slow Motion")]
    public float slowMotionTimeScale;
    public float startTimeScale;
    private float startFixedDeltaTime;
    public bool inSlowMotion;
    public CinemachineSwitcher cameraStates;

    [Header("Deluminator Data")]
    public ParticleSystem lightOrbHitParticle;
    //Collider[] lightOverlap;
    [SerializeField] float overlapRadius;
    [SerializeField] LayerMask lightLayer;

    [Header("Gun Data")]
    [SerializeField] ParticleSystem gunFlashParticle;
    [SerializeField] AudioSource gunSound;
    bool gunSoundPlayed;


    [Header("Creature Data")]
    [SerializeField] private Animator creatureAnimator;
    [SerializeField] private Vector3 playerVelocity;
    bool canGroundCheck;


    public float slowMoCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        
        canGroundCheck = true;
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Time.timeScale = 1;
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }


    void AnimMoves()
    {
        if (horizontalInput == 0 && verticalInput == 0 && groundCheck)
        {
            creatureAnimator.SetBool("canRoll", false);
            creatureAnimator.SetBool("canRollHorizontal", false);
            creatureAnimator.SetBool("canAimAndFire", false);
        }
        else if (horizontalInput != 0 && verticalInput == 0 && groundCheck)
        {
            creatureAnimator.SetBool("canRoll", false);
            creatureAnimator.SetBool("canRollHorizontal", true);
            creatureAnimator.SetBool("canAimAndFire", false);

        }
        else if (horizontalInput != 0 && verticalInput != 0 && groundCheck)
        {
            creatureAnimator.SetBool("canRoll", true);
            creatureAnimator.SetBool("canRollHorizontal", false);
            creatureAnimator.SetBool("canAimAndFire", false);

        }
        else if (horizontalInput == 0 && verticalInput != 0 && groundCheck)
        {
            creatureAnimator.SetBool("canRoll", true);
            creatureAnimator.SetBool("canRollHorizontal", false);
            creatureAnimator.SetBool("canAimAndFire", false);
        }
    }

    private void FixedUpdate()
    {
        playerVelocity = rb.velocity;
        MovePlayer();
        groundCheck = canGroundCheck;
        AnimMoves();

        groundCheck = Physics.Raycast(transform.position, Vector3.down, playerHeight * .05f + 0.2f, groundLayer);

        MyInput();
        SpeedControl();

        if (groundCheck)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        transform.forward = orientation.forward;

        if(groundCheck && cameraStates.inGroundCamera == false)
        {
            cameraStates.SwitchState();
        }
        if(!groundCheck && cameraStates.inGroundCamera == true)
        {
            cameraStates.SwitchState();
        }

    }

    private void LateUpdate()
    {
       
    }



    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(jumpKey) && readyToJump && groundCheck)
        {
            readyToJump = false;
            StartCoroutine(JumpGun());
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
            cameraStates.SwitchState();

        }

        if(Input.GetKey(jumpKey) &&!readyToJump && !groundCheck)
        {
            inSlowMotion = true;
            
            StartSlowMotion();
        }

        if (Input.GetKeyUp(jumpKey))
        {
            
        }

    }


    IEnumerator FinishSlowMo()
    {
        yield return new WaitForSeconds(slowMoCoolDown);
        Debug.Log("Exiting Slow Motion");
        //cameraStates.inGroundCamera = false;
        cameraStates.SwitchState();
        StopSlowMotion();
        inSlowMotion = false;
    }

    public void StartSlowMotion()
    {
        Debug.Log("In Slow Motion");
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;
        StartCoroutine(FinishSlowMo());
    }

    public void StopSlowMotion()
    {
        AnimMoves();
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }


    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        if (groundCheck && canGroundCheck)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        else if (!groundCheck)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    public void Jump()
    {
        groundCheck = false;
        creatureAnimator.SetBool("canRoll", false);
        creatureAnimator.SetBool("canRollHorizontal", false);
        creatureAnimator.SetBool("canAimAndFire", true);

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    IEnumerator JumpGun()
    {
        canGroundCheck = false;
        yield return new WaitForSeconds(1f);
        canGroundCheck = true;
    }


    public void ResetJump()
    {
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && gameMan.arrowCount > 0)
        {
            gunFlashParticle.Play();
            if (!gunSoundPlayed)
            {
                gunSound.Play();
                gunSoundPlayed = true;
            }
            gameMan.arrowCount--;

            
            gunSoundPlayed = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Light Orb"))
        {
            collision.gameObject.SetActive(false);
            lightOrbHitParticle.Play();
        }
    }

    private void OnDrawGizmos()
    {
        
        //Debug.DrawLine(transform.position, Vector3.down, Color.green, playerHeight * .05f + 0.2f); 
    }
}

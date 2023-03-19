using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerController : MonoBehaviour
{
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



    // Start is called before the first frame update
    void Start()
    {
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(jumpKey) && readyToJump && groundCheck)
        {
            readyToJump = false;
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
            Debug.Log("Exiting Slow Motion");
            cameraStates.SwitchState();
            StopSlowMotion();
            inSlowMotion = false;
        }

    }

    public void StartSlowMotion()
    {
        Debug.Log("In Slow Motion");
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }


    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        if (groundCheck)
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
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        groundCheck = Physics.Raycast(transform.position, Vector3.down, playerHeight * .05f + 0.2f, groundLayer);

        MyInput();
        SpeedControl();

        if (groundCheck)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        transform.forward = orientation.forward;
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

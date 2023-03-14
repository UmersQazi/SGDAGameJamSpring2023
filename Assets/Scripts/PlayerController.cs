using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float playerSpeed;
    [SerializeField] float fallMultiplier;
    public float slowMotionTimeScale;
    private float startTimeScale;
    private float startFixedDeltaTime;

    private float horizontalInput, verticalInput;

    private Vector2 turn;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRb = GetComponent<Rigidbody>();
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontalInput * playerSpeed, 0, verticalInput * playerSpeed) * Time.deltaTime;

        turn.x += Input.GetAxis("Mouse X");
        transform.localRotation = Quaternion.Euler(0, turn.x, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(Vector3.up * playerSpeed, ForceMode.Impulse);
            StartSlowMotion();
        }
        if(playerRb.velocity.y < 0)
        {
            playerRb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopSlowMotion();
        }
    }

    public void StartSlowMotion()
    {
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }

}

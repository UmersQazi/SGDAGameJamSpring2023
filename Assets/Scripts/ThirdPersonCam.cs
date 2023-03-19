using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    [Header("Aim and Fire")]
    public Vector3 origin;
    public Vector3 direction;
    public float range;
    public LayerMask targetLayer;
    public LayerMask lightLayer;
    public bool canDeluminate;
    public bool canHit;
    GameObject currentTarget;
    public GameObject lightOrb;

    [SerializeField] GameManager gameMan;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        origin = transform.position;
        direction = transform.forward;
        
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        Aim();

        
    }

    public void Aim()
    {
        RaycastHit hit, lightHit;
        canHit = Physics.Raycast(origin, direction, out hit, range, targetLayer);
        canDeluminate = Physics.Raycast(origin, direction, out lightHit, range, lightLayer);

        if (canHit && Input.GetKeyDown(KeyCode.E))
        {
            currentTarget = hit.collider.gameObject;
            currentTarget.SetActive(false);
            gameMan.arrowCount -= 1;
            //Debug.Log("Found Target!");
        }

        if(canDeluminate && Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(lightOrb, hit.transform.position, hit.transform.rotation);
            
            gameMan.arrowCount++;

        }

        
    }

}

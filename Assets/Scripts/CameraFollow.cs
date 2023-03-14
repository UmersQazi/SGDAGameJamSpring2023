using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;

    private Vector2 turn;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
        turn.x += Input.GetAxis("Mouse X");
        turn.y = Input.GetAxis("Mouse Y");
        transform.localRotation = Quaternion.Euler(-turn.y, 0, 0);
    }
}

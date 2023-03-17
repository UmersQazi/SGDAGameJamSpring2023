using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour
{
    public float speed = 5f;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);
        transform.position += direction * speed * Time.deltaTime;
    }
}

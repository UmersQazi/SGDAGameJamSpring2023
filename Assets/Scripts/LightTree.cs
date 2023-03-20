using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTree : MonoBehaviour
{

    [SerializeField] GameObject lightOrb;
    [SerializeField] float cooldown = 3f;
    public Animator animator;
    bool canMakeLight;
    public void MakeLight(Vector3 position, Quaternion rotation)
    {
        if (canMakeLight)
        {
            animator.SetBool("canDeluminate", true);
            Instantiate(lightOrb, position, rotation);
            StartCoroutine(Recharge());
            canMakeLight = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canMakeLight = true;
    }

    IEnumerator Recharge()
    {
        yield return new WaitForSeconds(cooldown);
        animator.SetBool("canDeluminate", false);
        animator.SetBool("canRecharge", true);
        canMakeLight= true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    SlomoController slomoController;
    Animator animator;
    
    // Start is called before the first frame update
    void Awake()
    {
        slomoController = FindObjectOfType<SlomoController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(slomoController.isSlomoEnabled)
        {
            animator.SetBool("Slomo", true);
        }
        else
        {
            animator.SetBool("Slomo", false);
        }
    }

    public void ExplosionCamera()
    {
        animator.SetTrigger("Explosion");
    }


}

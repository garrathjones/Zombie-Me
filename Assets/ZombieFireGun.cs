using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFireGun : MonoBehaviour
{
    [SerializeField] ZombieGun zombieGun;
    ZombieMovement zombieMovement;
    ZombieHealth zombieHealth;
    Coroutine firingCoroutine;
    bool firing = false;

    [SerializeField] float bulletFiringPeriod = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

        zombieMovement = GetComponent<ZombieMovement>();
        zombieHealth = GetComponent<ZombieHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        Debug.Log("firing = " + firing);
    }



    void Fire()
    {
        Debug.Log("fIRE CALLED");
        if (zombieMovement.playerInFiringRange && !firing && zombieHealth.alive)
        {
            Debug.Log("firing coroutine called");
            firing = true;
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!zombieMovement.playerInFiringRange && firing && zombieHealth.alive)
        {            
            StopCoroutine(firingCoroutine);
            firing = false;
        }
            
    }

    IEnumerator FireContinuously()
    {

        while (zombieHealth.alive)
        {
            zombieGun.Fire();
            firing = true;
            yield return new WaitForSeconds(bulletFiringPeriod);
        }

    }


}

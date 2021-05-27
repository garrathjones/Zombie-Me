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

    void Start()
    {
        zombieMovement = GetComponent<ZombieMovement>();
        zombieHealth = GetComponent<ZombieHealth>();
    }

    void Update()
    {
        Fire();
    }



    void Fire()
    {
        if (zombieMovement.playerInFiringRange && !firing && zombieHealth.alive)
        {
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

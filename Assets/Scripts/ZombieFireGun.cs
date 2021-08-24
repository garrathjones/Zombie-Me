using System.Collections;
using UnityEngine;

public class ZombieFireGun : MonoBehaviour
{
    [SerializeField] ZombieGun zombieGun;
    [SerializeField] [Range(0, 100)] float chanceOfStepBackwards = 50f;
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
            RandomJumpFwdOrBackwards();
            yield return new WaitForSeconds(bulletFiringPeriod);
        }
    }
    
    public void RandomJumpFwdOrBackwards()
    {
        float randomChance = Random.Range(0, 100);
        if (randomChance <= chanceOfStepBackwards)
        {
            zombieMovement.RandomJumpFwdOrBackwards();
        }
    }


}

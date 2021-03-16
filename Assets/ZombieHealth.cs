using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField] float health = 30;
    [SerializeField] float KickSpeedLimit = 5f;

    Zombie zombie;

    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponent<Zombie>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckIfDead()
    {
        if (health <= 0)
        {
            zombie.zombieRigidBody.constraints = RigidbodyConstraints2D.None;
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        Machete machete = other.gameObject.GetComponent<Machete>();
        SlideKick slideKick = other.gameObject.GetComponent<SlideKick>();
        if (!bullet && !machete && !slideKick) { return; }
        if (bullet)
        {
            ProcessBulletHit(bullet);
        }
        if (machete)
        {
            ProcessMacheteHit(machete);
        }
        if (slideKick)
        {
            ProcessSlideKick(slideKick);
        }

    }



    private void ProcessBulletHit(Bullet bullet)
    {
        health -= bullet.GetDamage();
        bullet.DestroyBulletWithBloodSplat();
        CheckIfDead();
    }

    private void ProcessMacheteHit(Machete machete)
    {
        if (zombie.zombieRigidBody.velocity.y > KickSpeedLimit)
        {
            return;
        }
        health -= machete.GetDamage();
        machete.CreateMacheteHitFX();
        zombie.zombieRigidBody.velocity += machete.GetMacheteHitVelocity() * new Vector2(-zombie.DirectionOfPlayer(), 1);
        zombie.zombieAnimator.SetTrigger("TakingDamage");
        zombie.zombieAnimator.SetBool("Biting", false);
        zombie.zombieAnimator.SetBool("Running", true);
        CheckIfDead();
    }

    private void ProcessSlideKick(SlideKick slideKick)
    {
        if (zombie.zombieRigidBody.velocity.y > KickSpeedLimit)
        {
            return;
        }
        if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
        {
            return;
        }
        health -= slideKick.GetDamage();
        zombie.zombieRigidBody.velocity += slideKick.GetSlideKickVelocity() * new Vector2(-zombie.DirectionOfPlayer(), 1);
        zombie.zombieAnimator.SetTrigger("TakingDamage");
        zombie.zombieAnimator.SetBool("Biting", false);
        zombie.zombieAnimator.SetBool("Running", true);
        CheckIfDead();
    }


    private void Die()
    {
        //AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        zombie.RandomSlomo();
        zombie.DeathKick();
        zombie.MakeRagDoll();
        zombie.alive = false;
        zombie.BleedWhenDead();
    }

}

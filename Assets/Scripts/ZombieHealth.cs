using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField] float health = 30;
    [SerializeField] float machetteHitVelocityMulitplier = 1f;
    [SerializeField] float slideHitVelocityMulitplier = 1f;
    [SerializeField] bool gateBoss = false;
    [SerializeField] AudioClip zombieDeathSFX;
    [SerializeField] [Range(0, 10)] float zombieDeathVolume = 1f;

    public bool alive = true;
    Zombie zombie;
    ZombieMovement zombieMovement;
    PlayerPosition playerPosition;
    SlomoController slomoController;
    Gate gate;



    void Start()
    {
        zombie = GetComponent<Zombie>();
        zombieMovement = GetComponent<ZombieMovement>();
        playerPosition = GetComponent<PlayerPosition>();
        slomoController = FindObjectOfType<SlomoController>();
    }

    private void CheckIfDead()
    {
        if (health <= 0)
        {
            //in case health goes negative then set it to zero
            health = 0;
            zombieMovement.zombieRigidBody.constraints = RigidbodyConstraints2D.None;
            Killed();            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        Machete machete = other.gameObject.GetComponent<Machete>();
        SlideKick slideKick = other.gameObject.GetComponent<SlideKick>();
        Pipe pipe = other.gameObject.GetComponent<Pipe>();
        if (!bullet && !machete && !slideKick && !pipe) { return; }
        if(!alive) { return; }
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
        if (pipe)
        {
            PipeThrust(pipe);
        }
    }

    private void PipeThrust(Pipe pipe)
    {
        zombieMovement.zombieRigidBody.velocity = new Vector2(zombieMovement.zombieRigidBody.velocity.x + pipe.thrustX, zombieMovement.zombieRigidBody.velocity.y + pipe.thrustY);
    }


    private void ProcessBulletHit(Bullet bullet)
    {
        health -= bullet.GetDamage();
        zombieMovement.GiveZombieVelocityOnHit(bullet);
        bullet.DestroyBulletWithBloodSplat();
        CheckIfDead();
    }





    private void ProcessMacheteHit(Machete machete)
    {
        health -= machete.GetDamage();
        machete.CreateMacheteHitFX();
        zombieMovement.zombieRigidBody.velocity += machete.GetMacheteHitVelocity() * new Vector2(-playerPosition.DirectionOfPlayer() * machetteHitVelocityMulitplier, 1);
        zombie.zombieAnimator.SetTrigger("TakingDamage");
        zombie.zombieAnimator.SetBool("Biting", false);
        zombie.zombieAnimator.SetBool("Running", true);
        CheckIfDead();
    }

    private void ProcessSlideKick(SlideKick slideKick)
    {
        if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
        {
            return;
        }
        health -= slideKick.GetDamage();
        zombieMovement.zombieRigidBody.velocity += slideKick.GetSlideKickVelocity() * new Vector2(-playerPosition.DirectionOfPlayer() * slideHitVelocityMulitplier, 1);
        zombie.zombieAnimator.SetTrigger("TakingDamage");
        zombie.zombieAnimator.SetBool("Biting", false);
        zombie.zombieAnimator.SetBool("Running", true);
        slideKick.PlaySlideKickHitFX();
        CheckIfDead();
    }

    private void Killed()
    {
        slomoController.RandomSlomo();
        zombie.DeathKick();
        zombie.MakeRagDoll();
        alive = false;
        zombie.BleedWhenDead();
        AudioSource.PlayClipAtPoint(zombieDeathSFX, Camera.main.transform.position, zombieDeathVolume);
        if (gateBoss)
        {
            gate = FindObjectOfType<Gate>();
            gate.DestroyGate();
        }
        var killCounter = FindObjectOfType<KillCounter>();
        killCounter.AddKill();
        zombie.DestroyZombieGameObject();
    }
    public void FallToDeath()
    {
        zombie.MakeRagDoll();
        alive = false;
        zombie.BleedWhenDead();
        zombie.DestroyZombieGameObject();
    }

    public float GetZombieHealth()
    {
        return health;
    }

}

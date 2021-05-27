using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField] float health = 30;
    //[SerializeField] float KickSpeedLimit = 5f;
    public bool alive = true;
    Zombie zombie;
    ZombieMovement zombieMovement;
    PlayerPosition playerPosition;
    SlomoController slomoController;

    [SerializeField] AudioClip hitBySlideSFX;
    [SerializeField] [Range(0, 1)] float hitBySlideSFXVolume = 0.5f;

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
        //if (zombie.zombieRigidBody.velocity.y > KickSpeedLimit)
        //{
        //    return;
        //}
        health -= machete.GetDamage();
        machete.CreateMacheteHitFX();
        zombieMovement.zombieRigidBody.velocity += machete.GetMacheteHitVelocity() * new Vector2(-playerPosition.DirectionOfPlayer(), 1);
        zombie.zombieAnimator.SetTrigger("TakingDamage");
        zombie.zombieAnimator.SetBool("Biting", false);
        zombie.zombieAnimator.SetBool("Running", true);
        CheckIfDead();
    }

    private void ProcessSlideKick(SlideKick slideKick)
    {
        //if (zombie.zombieRigidBody.velocity.y > KickSpeedLimit)
        //{
        //    return;
        //}
        if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
        {
            return;
        }
        health -= slideKick.GetDamage();
        zombieMovement.zombieRigidBody.velocity += slideKick.GetSlideKickVelocity() * new Vector2(-playerPosition.DirectionOfPlayer(), 1);
        zombie.zombieAnimator.SetTrigger("TakingDamage");
        zombie.zombieAnimator.SetBool("Biting", false);
        zombie.zombieAnimator.SetBool("Running", true);
        AudioSource.PlayClipAtPoint(hitBySlideSFX, Camera.main.transform.position, hitBySlideSFXVolume);
        CheckIfDead();
    }

    private void Killed()
    {
        //AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        slomoController.RandomSlomo();
        zombie.DeathKick();
        zombie.MakeRagDoll();
        alive = false;
        zombie.BleedWhenDead();
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

}

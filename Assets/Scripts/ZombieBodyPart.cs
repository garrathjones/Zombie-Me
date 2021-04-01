using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBodyPart : MonoBehaviour
{    
    Rigidbody2D bodyPartRigidBody;
    Player player;
    Transform playerTransform;
    [SerializeField] float bleedingDuration = 5f;
    [SerializeField] float pulseRate = 0.5f;
    [SerializeField] GameObject bloodSplurt;

    bool bodyPartIsBleeding = false;

    void Start()
    {
        bodyPartRigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        playerTransform = player.GetComponent<Transform>();       
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        Machete machete = other.gameObject.GetComponent<Machete>();
        SlideKick slideKick = other.gameObject.GetComponent<SlideKick>();
        Pipe pipe = other.gameObject.GetComponent<Pipe>();
        if (!bullet && !machete && !slideKick && !pipe) { return; }
        if (bullet)
        {
            ProcessBulletHit(bullet);
        }
        if (machete)
        {            
            ProcessMacheteHit(machete);
        }
        if(slideKick)
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
        bodyPartRigidBody.velocity = new Vector2(bodyPartRigidBody.velocity.x + pipe.thrustX, bodyPartRigidBody.velocity.y + pipe.thrustY);
    }



    private void ProcessBulletHit(Bullet bullet)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        Vector2 bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
        Vector2 newVelocity = new Vector2(bulletVelocity.x * bullet.GetBodyPartBulletBlastMultiplierX(), Math.Abs(bulletVelocity.y + bullet.GetBodyPartBulletBlastMultiplierY()));
        bodyPartRigidBody.velocity = newVelocity;
        bullet.DestroyBulletWithBloodSplat();
    }     

    private void ProcessMacheteHit(Machete machete)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        machete.CreateMacheteHitFX();
        Vector2 macheteVelocity = machete.GetMacheteDeathHitVelocty() * new Vector2(DirectionOfPlayer(), 1);
        bodyPartRigidBody.velocity = macheteVelocity;        
    }
    private void ProcessSlideKick(SlideKick slideKick)
    {
        Vector2 kickVelocity = slideKick.GetSlideKickVelocity() * new Vector2(DirectionOfPlayer(), 1);
        bodyPartRigidBody.velocity = kickVelocity;
    }


    private void OnJointBreak2D(Joint2D joint)
    {
        bodyPartIsBleeding = true;
        StartCoroutine(BodyPartBleedingCoroutine());
    }

    //void BodyPartBleeding()
    //{
    //    StartCoroutine(BodyPartBleedingCoroutine());
    //}


    IEnumerator BodyPartBleedingCoroutine()
    {
        while (bodyPartIsBleeding)
        {
            GameObject splurt = Instantiate(bloodSplurt, transform.position, transform.rotation);
            Destroy(splurt, bleedingDuration);
            yield return new WaitForSeconds(pulseRate);
        }
    }


    private float DirectionOfPlayer()
    {
        if (playerTransform.position.x   > transform.position.x)
        {
            return -1;
        }
        else
            return 1;
    }

}

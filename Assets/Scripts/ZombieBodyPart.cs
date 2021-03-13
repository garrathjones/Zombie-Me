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
        if (!bullet && !machete && !slideKick) { return; }
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
    }

    private void ProcessBulletHit(Bullet bullet)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        Vector2 bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
        bodyPartRigidBody.velocity = bulletVelocity * bullet.GetBulletBlastMultiplier();
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

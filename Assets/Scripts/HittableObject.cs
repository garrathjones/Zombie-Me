using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour
{
    Rigidbody2D rigidBody;
    Player player;
    Transform playerTransform;
    [SerializeField] float bulletVelocityDampeningMultiplier = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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
        if (slideKick)
        {
            ProcessSlideKick(slideKick);
        }
    }

    private void ProcessBulletHit(Bullet bullet)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        Vector2 bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
        rigidBody.velocity = bulletVelocity * bulletVelocityDampeningMultiplier * bullet.GetBulletBlastMultiplier();
        bullet.DestroyBulletWithDust();
    }

    private void ProcessMacheteHit(Machete machete)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        //add dust effect or somthing here
        Vector2 macheteVelocity = machete.GetMacheteDeathHitVelocty() * new Vector2(DirectionOfPlayer(), 1);
        rigidBody.velocity = macheteVelocity;
    }
    private void ProcessSlideKick(SlideKick slideKick)
    {
        Vector2 kickVelocity = slideKick.GetSlideKickVelocity() * new Vector2(DirectionOfPlayer(), 1);
        rigidBody.velocity = kickVelocity;
    }

    private float DirectionOfPlayer()
    {
        if (playerTransform.position.x > transform.position.x)
        {
            return -1;
        }
        else
            return 1;
    }
}

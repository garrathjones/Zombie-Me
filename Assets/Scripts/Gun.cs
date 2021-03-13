using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] float bulletFiringPeriod = 0.1f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.5f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletSpawner;
    [SerializeField] ParticleSystem flashFX;
    [SerializeField] ParticleSystem smokeFX;


    //[SerializeField] float bulletCapacity;
    //[SerializeField] float reloadTime;

    Coroutine firingCoroutine;
    Rigidbody2D gunRigidBody;
    Collider2D gunCollider;    
    Player player;
    Pause pause;

    void Start()
    {
        pause = FindObjectOfType<Pause>();
        player = FindObjectOfType<Player>();
        gunRigidBody = GetComponent<Rigidbody2D>();
        gunCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        Fire();
        if (!player.PlayerAlive())
        {
            DropGun();
        }       

    }

    private void Fire()
    {             
        if (Input.GetButtonDown("Fire1"))
        {           
            if(player.PlayerAlive())
            {
                firingCoroutine = StartCoroutine(FireContinuously());
            }        
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (player.PlayerAlive())
            {
                StopCoroutine(firingCoroutine);
            }
                       
        }
        return;
    }

    IEnumerator FireContinuously()
    {
        while (player.PlayerAlive())
        {
            var playerDirection = PlayerDirection();
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.transform.rotation) as GameObject;
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            flashFX.Play();
            smokeFX.Play();
            bullet.transform.localScale *= playerDirection;
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * playerDirection * bulletSpeed;
            //add trail here

                       
            yield return new WaitForSeconds(bulletFiringPeriod);        
        }
    }

    private float PlayerDirection()
    {
        var playerDirection = player.transform.localScale.x;
        return playerDirection;
    }

    private void DropGun()
    {
        gunRigidBody.isKinematic = false;
        gunCollider.enabled = true;
    }
}

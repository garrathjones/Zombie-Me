using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGun : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 100f;

    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.5f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletSpawner;
    [SerializeField] ParticleSystem flashFX;
    [SerializeField] ParticleSystem smokeFX;

    [SerializeField] Zombie zombie;



    public bool firing = false;
    void Start()
    {
        zombie = GetComponent<Zombie>();
    }


    public void Fire()
    {
        var zombieDirection = ZombieDirection();
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.transform.rotation) as GameObject;        
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        flashFX.Play();
        smokeFX.Play();
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * zombieDirection * bulletSpeed;
        //add trail here
    }


    private float ZombieDirection()
    {
        //var zombieDirection = zombie.transform.localScale.x;
        var zombieDirection = this.transform.parent.parent.parent.parent.parent.localScale.x;
        return zombieDirection;
    }
}

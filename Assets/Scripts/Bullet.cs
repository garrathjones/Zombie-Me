using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] float bulletBlastMultiplierX = 1f;
    [SerializeField] float bulletBlastMultiplierY = 1f;
    [SerializeField] float bodyPartBulletBlastMultiplierX = 10f;
    [SerializeField] float bodyPartBulletBlastMultiplierY = 10f;
    [SerializeField] float dustDuration = 2f;
    [SerializeField] float splatDuration = 5f;
    [SerializeField] float disablePlayerRunDuration = 0.2f;
    [SerializeField] GameObject bulletImpactDust;
    [SerializeField] GameObject bloodSplat;
    [SerializeField] GameObject explosionFlash;
    [SerializeField] GameObject explosionSmoke;
    //[SerializeField] GameObject bulletTrail;
    //[SerializeField] float trailDuration = 2f;

    [SerializeField] AudioClip bulletToFleshSFX;
    [SerializeField] [Range(0, 1)] float bulletToFleshVolume = 1f;
    [SerializeField] AudioClip bulletToWallSFX;
    [SerializeField] [Range(0, 1)] float bulletToWallVolume = 0.5f;

    [SerializeField] public bool isRocket = false;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] [Range(0, 1)] float explosionVolume = 1f;

    [SerializeField] ParticleSystem rocketSmoke;

    bool damageDone = false;

    private void Update()
    {
        if(isRocket)
        {
            rocketSmoke.Play();
        }
    }


    public int GetDamage()
    {
        if (damageDone)
        {
            return 0;
        }
        else
        {
            damageDone = true;
            return damage;
        }
        
    }

    public float GetDisablePlayerRunDuration()
    {
        return disablePlayerRunDuration;
    }

    public void DestroyBulletWithDust()
    {
        if (isRocket)
        {
            AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, explosionVolume);
            GameObject exploSmoke = Instantiate(explosionSmoke, transform.position, transform.rotation);
            Destroy(exploSmoke, splatDuration);
            GameObject exploFlash = Instantiate(explosionFlash, transform.position, transform.rotation);
            Destroy(exploFlash, splatDuration);
        }
        AudioSource.PlayClipAtPoint(bulletToWallSFX, Camera.main.transform.position, bulletToWallVolume);
        GameObject poof = Instantiate(bulletImpactDust, transform.position, transform.rotation);
        Destroy(poof, dustDuration);  
        Destroy(gameObject);
    }

    public void DestroyBulletWithBloodSplat()
    {
        if (isRocket)
        {
            AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, explosionVolume);
            GameObject exploSmoke = Instantiate(explosionSmoke, transform.position, transform.rotation);
            Destroy(exploSmoke, splatDuration);
            GameObject exploFlash = Instantiate(explosionFlash, transform.position, transform.rotation);
            Destroy(exploFlash, splatDuration);
        }
        AudioSource.PlayClipAtPoint(bulletToFleshSFX, Camera.main.transform.position, bulletToFleshVolume);
        GameObject splat = Instantiate(bloodSplat, transform.position, transform.rotation);
        Destroy(splat, splatDuration);
        Destroy(gameObject);
    }

    public float GetBulletBlastMultiplierX()
    {
        return bulletBlastMultiplierX;
    }

    public float GetBulletBlastMultiplierY()
    {
        return bulletBlastMultiplierY;
    }
    public float GetBodyPartBulletBlastMultiplierX()
    {
        return bodyPartBulletBlastMultiplierX;
    }

    public float GetBodyPartBulletBlastMultiplierY()
    {
        return bodyPartBulletBlastMultiplierY;
    }



}

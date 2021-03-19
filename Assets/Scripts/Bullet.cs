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
    //[SerializeField] GameObject bulletTrail;
    //[SerializeField] float trailDuration = 2f;

    [SerializeField] AudioClip bulletToFleshSFX;
    [SerializeField] [Range(0, 1)] float bulletToFleshVolume = 1f;
    [SerializeField] AudioClip bulletToWallSFX;
    [SerializeField] [Range(0, 1)] float bulletToWallVolume = 0.5f;



    //private void BulletTrail()
    //{
    //GameObject trail = Instantiate(bulletTrail, transform.position, transform.rotation);
    //Destroy(trail, trailDuration);
    //}



    public int GetDamage()
    {
        return damage;
    }

    public float GetDisablePlayerRunDuration()
    {
        return disablePlayerRunDuration;
    }

    public void DestroyBulletWithDust()
    {
        AudioSource.PlayClipAtPoint(bulletToWallSFX, Camera.main.transform.position, bulletToWallVolume);
        GameObject poof = Instantiate(bulletImpactDust, transform.position, transform.rotation);
        Destroy(poof, dustDuration);  
        Destroy(gameObject);
    }

    public void DestroyBulletWithBloodSplat()
    {
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

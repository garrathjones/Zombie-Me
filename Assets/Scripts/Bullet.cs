using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] float bulletBlastMultiplier = 1f;
    [SerializeField] float dustDuration = 2f;
    [SerializeField] float splatDuration = 5f;
    [SerializeField] GameObject bulletImpactDust;
    [SerializeField] GameObject bloodSplat;
    //[SerializeField] GameObject bulletTrail;
    //[SerializeField] float trailDuration = 2f;



    //private void BulletTrail()
    //{
    //GameObject trail = Instantiate(bulletTrail, transform.position, transform.rotation);
    //Destroy(trail, trailDuration);
    //}



    public int GetDamage()
    {
        return damage;
    }

    public void DestroyBulletWithDust()
    {
        GameObject poof = Instantiate(bulletImpactDust, transform.position, transform.rotation);
        Destroy(poof, dustDuration);  
        Destroy(gameObject);
    }

    public void DestroyBulletWithBloodSplat()
    {
        GameObject splat = Instantiate(bloodSplat, transform.position, transform.rotation);
        Destroy(splat, splatDuration);
        Destroy(gameObject);
    }

    public float GetBulletBlastMultiplier()
    {
        return bulletBlastMultiplier;
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyPart : MonoBehaviour
{
    Rigidbody2D bodyPartRigidBody;
    [SerializeField] float bleedingDuration = 5f;
    [SerializeField] float pulseRate = 0.5f;
    [SerializeField] GameObject bloodSplurt;

    bool bodyPartIsBleeding = false;

    private void Start()
    {
        bodyPartRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        bodyPartIsBleeding = true;
        StartCoroutine(BodyPartBleedingCoroutine());
    }

    IEnumerator BodyPartBleedingCoroutine()
    {
        while (bodyPartIsBleeding)
        {
            GameObject splurt = Instantiate(bloodSplurt, transform.position, transform.rotation);
            Destroy(splurt, bleedingDuration);
            yield return new WaitForSeconds(pulseRate);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        if (!bullet) { return; }
        if (bullet)
        {
            ProcessBulletHit(bullet);
        }

    }
    private void ProcessBulletHit(Bullet bullet)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        Vector2 bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
        bodyPartRigidBody.velocity = bulletVelocity * bullet.GetBulletBlastMultiplier();
        bullet.DestroyBulletWithBloodSplat();
    }
}

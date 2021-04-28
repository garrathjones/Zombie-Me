using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyPart : MonoBehaviour
{
    Rigidbody2D bodyPartRigidBody;
    [SerializeField] float bleedingDuration = 5f;
    [SerializeField] float pulseRate = 0.5f;
    [SerializeField] GameObject bloodSplurt;
    [SerializeField] float pipeThrustMultiplier = 2f;
    [SerializeField] public bool isTorso = false; 

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
        Pipe pipe = other.gameObject.GetComponent<Pipe>();
        if (!bullet && !pipe) { return; }
        if (bullet)
        {
            ProcessBulletHit(bullet);
        }
        if(pipe)
        {
            PipeThrust(pipe);
        }

    }

    private void PipeThrust(Pipe pipe)
    {
        bodyPartRigidBody.velocity = new Vector2(bodyPartRigidBody.velocity.x + pipe.thrustX * pipeThrustMultiplier, bodyPartRigidBody.velocity.y + pipe.thrustY * pipeThrustMultiplier);
    }

    private void ProcessBulletHit(Bullet bullet)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        Vector2 bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
        Vector2 newVelocity = new Vector2(bulletVelocity.x * bullet.GetBodyPartBulletBlastMultiplierX(), Math.Abs(bulletVelocity.y + bullet.GetBodyPartBulletBlastMultiplierY()));
        bodyPartRigidBody.velocity = newVelocity;
        bullet.DestroyBulletWithBloodSplat();
    }
}

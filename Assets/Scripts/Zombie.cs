using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

public class Zombie : MonoBehaviour
{
    [SerializeField] float deathKickSpeed = 5f;
    [SerializeField] float pulseRate = 0.5f;
    [SerializeField] GameObject bloodSplurt;
    [SerializeField] GameObject torsoBleedPoint;
    [SerializeField] public GameObject mainBone;  
    [SerializeField] float bleedDuration = 5f;

    bool ragDolled = false;
    bool torsoBleeding = false;

    ZombieHealth zombieHealth;
    ZombieMovement zombieMovement;
    Rigidbody2D zombieRigidBody;
    public Animator zombieAnimator;
    UnityEngine.U2D.IK.IKManager2D zombieIK2D;
    public GameObject[] BodyParts;

    void Start()
    {
        zombieAnimator = GetComponent<Animator>();
        zombieRigidBody = GetComponent<Rigidbody2D>();
        zombieHealth = GetComponent<ZombieHealth>();
        zombieMovement = GetComponent<ZombieMovement>();
        zombieIK2D = GetComponent<UnityEngine.U2D.IK.IKManager2D>();     
    }

    void FixedUpdate()
    {
        if(!zombieHealth.alive)
        {
            BleedWhenDead();
        }
    }   

    public void FallToDeath()
    {
        MakeRagDoll();
        zombieHealth.alive = false;
        BleedWhenDead();
    }

    public void DeathKick()
    {
        zombieMovement.zombieRigidBody.constraints = RigidbodyConstraints2D.None;
        Vector2 newVelocity = new Vector2(zombieMovement.zombieRigidBody.velocity.x, deathKickSpeed);
        zombieMovement.zombieRigidBody.velocity = newVelocity;
    }

    public void BleedWhenDead()
    {
        StartCoroutine(MakeTorsoBleed());
    }
    
    IEnumerator MakeTorsoBleed()
    {
            if (!torsoBleeding)
            {
                torsoBleeding = true;
                GameObject splurt = Instantiate(bloodSplurt, torsoBleedPoint.transform.position, torsoBleedPoint.transform.rotation);
                Destroy(splurt, bleedDuration);
                yield return new WaitForSeconds(pulseRate);
                torsoBleeding = false;
            }        
    }
        

    public void MakeRagDoll()
    {
        if (ragDolled) { return; }
        zombieAnimator.enabled = false;
        zombieIK2D.enabled = false;
        zombieMovement.bodyCollider.enabled = false;
        for (int i = 0; i < BodyParts.Length; i++)
        {
            var bodyPartRigidBody = BodyParts[i].GetComponent<Rigidbody2D>();
            bodyPartRigidBody.isKinematic = false;
            bodyPartRigidBody.velocity = zombieMovement.zombieRigidBody.velocity;
            BodyParts[i].GetComponent<Collider2D>().enabled = true;
        }
        zombieMovement.zombieRigidBody.isKinematic = true;
        zombieMovement.zombieRigidBody.transform.position = mainBone.transform.position;
        ragDolled = true;
        zombieRigidBody.bodyType = RigidbodyType2D.Static;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

public class Zombie : MonoBehaviour
{
    //CONFIG
    [SerializeField] float health = 30;
    [SerializeField] float biteDamage = 10f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float deathKickSpeed = 5f;
    [SerializeField] float KickSpeedLimit = 5f;
    [SerializeField] float fallMultiplier = 3f;
    [SerializeField] float randomJumpMaxTime = 7f;
    [SerializeField] float randomJumpMinTime = 2f;
    [SerializeField] float zombieReactionTime = 0.6f;
    [SerializeField] float CanSeePlayerDistance = 40f;
    [SerializeField] float bufferDistance = 0.8f;
    [SerializeField] float biteRange = 0.5f;
    [SerializeField] float biteRangeWhenPlayerDead = 1.2f;
    [SerializeField] float crawlHeight = 0.8f;
    [SerializeField] float bleedingDuration = 5f;
    [SerializeField] float pulseRate = 0.5f;
    [SerializeField] GameObject bloodSplurt;
    [SerializeField] GameObject bitingBlood;
    [SerializeField] GameObject torsoBleedPoint;
    [SerializeField] GameObject mouth;

    [SerializeField] public GameObject mainBone;   
    [SerializeField] CapsuleCollider2D bodyCollider;
    [SerializeField] BoxCollider2D crawlCollider;
    [SerializeField] BoxCollider2D groundCheckCollider;

    [SerializeField] AudioClip footstepSFX;
    [SerializeField] [Range(0, 1)] float footstepVolume = 0.3f;
    [SerializeField] ParticleSystem dust;
 


    bool ragDolled = false;
    bool alive = true;
    bool touchingFloor;
    float randomJumpPeriod = 0f;
    bool isMovementAICoroutineExecuting = false;
    bool torsoBleeding = false;


    Player player;
    Animator zombieAnimator;
    Rigidbody2D zombieRigidBody;
    UnityEngine.U2D.IK.IKManager2D zombieIK2D;
    Transform playerTransform;
    Transform playerTorsoPosition;
    public GameObject[] BodyParts;

    void Start()
    {
        player = FindObjectOfType<Player>();
        zombieAnimator = GetComponent<Animator>();
        zombieRigidBody = GetComponent<Rigidbody2D>();
        zombieIK2D = GetComponent<UnityEngine.U2D.IK.IKManager2D>();
        playerTransform = player.transform;
        playerTorsoPosition = player.LocationOfTorso();
        RandomJump();
    }

    void FixedUpdate()
    {
        if (alive)
        {
            FallModifier();
            MovementAI();
            AttackAI();
        }
        if(!alive)
        {
            BleedWhenDead();
        }
    }

    Vector2 PlayerPosition()
    {        
        if (player.PlayerAlive())
        {
            return playerTransform.position;
        }
        else
            return playerTorsoPosition.position;
    }    
    
    void AttackAI()
    {
        if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage") || zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Biting"))
        {
            return;
        }
        if (alive)
        {
            if (!player.PlayerAlive())
            {
                biteRange = biteRangeWhenPlayerDead;
            }
            var distance = Vector3.Distance(PlayerPosition(), transform.position);
            if (distance < bufferDistance + biteRange)
            {
                Biting();
            }
            if (distance > bufferDistance + biteRange)
            {
                StopBiting();
            }
        }
    }

    void MovementAI()
    {
        if (alive)
        {
            if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
            {
                return;
            }
            StartCoroutine(MovementAICoroutine());
        }       
    }

    IEnumerator MovementAICoroutine()
    {
        if(isMovementAICoroutineExecuting)
        {
            yield break;
        }
        isMovementAICoroutineExecuting = true;
        yield return new WaitForSeconds(zombieReactionTime);
        if(alive)
        {
            FlipSprite();
            var distance = Vector3.Distance(PlayerPosition(), transform.position);
            if (distance <= CanSeePlayerDistance && distance >= bufferDistance + biteRange)
            {
                Run();
            }
            if (distance > CanSeePlayerDistance)
            {
                Idle();
            }
        }
        isMovementAICoroutineExecuting = false;
    }

    
    private void Biting()
    {
        if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage") || zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Biting"))
        {
            return;
        }
        var zombieYDistanceFromPlayer = zombieRigidBody.position.y - PlayerPosition().y;
        if (!player.PlayerAlive() && zombieYDistanceFromPlayer > crawlHeight)
        {
            zombieAnimator.SetBool("Crawling", true);
            bodyCollider.enabled = false;
            crawlCollider.enabled = true;
        }
        else
        {
            crawlCollider.enabled = false;
            bodyCollider.enabled = true;
            zombieAnimator.SetBool("Biting", true);
        }
    }
    
    private void StopBiting()
    {
        zombieAnimator.SetBool("Biting", false);
        zombieAnimator.SetBool("Crawling", false);
    }
    private void RandomJump()
    {
        StartCoroutine(RandomJumpCoroutine());
    }

    IEnumerator RandomJumpCoroutine()
    {
        while(alive)
        {                      
            randomJumpPeriod = Random.Range(randomJumpMinTime, randomJumpMaxTime);
            yield return new WaitForSeconds(randomJumpPeriod);
            Jump();
        }
    }

    private void Jump()
    {
        if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
        {
            return;
        }
        if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
        {
            CheckIfTouchingGround();
            if (touchingFloor)
            {
                AudioSource.PlayClipAtPoint(footstepSFX, Camera.main.transform.position, footstepVolume);
                Vector2 newVelocity = new Vector2(zombieRigidBody.velocity.x, jumpSpeed);
                zombieRigidBody.velocity = newVelocity;
            }
        }
    }

    private void CheckIfTouchingGround()
    {
        if (groundCheckCollider.IsTouchingLayers())
        {
            touchingFloor = true;
        }
        else
        {
            touchingFloor = false;
        }
    }

    private int DirectionOfPlayer()
    {
        if (PlayerPosition().x > transform.position.x)
        {
            return 1;
        }
        else
            return -1;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        Machete machete = other.gameObject.GetComponent<Machete>();
        SlideKick slideKick = other.gameObject.GetComponent<SlideKick>();
        if (!bullet && !machete && !slideKick) { return; }
        if(bullet)
        {
            ProcessBulletHit(bullet);
        }
        if(machete)
        {
            ProcessMacheteHit(machete);
        }
        if (slideKick)
        {
            ProcessSlideKick(slideKick);
        }

    }

    private void CheckIfDead()
    {
        if (health <= 0)
        {
            zombieRigidBody.constraints = RigidbodyConstraints2D.None;
            Die();
        }
    }

    private void ProcessBulletHit(Bullet bullet)
    {
        health -= bullet.GetDamage();
        bullet.DestroyBulletWithBloodSplat();
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        CheckIfDead();
    }

    private void ProcessMacheteHit(Machete machete)
    {
        if(zombieRigidBody.velocity.y > KickSpeedLimit)
        {
            return;
        }
        health -= machete.GetDamage();
        machete.CreateMacheteHitFX();
        zombieRigidBody.velocity += machete.GetMacheteHitVelocity() * new Vector2(-DirectionOfPlayer(), 1);
        zombieAnimator.SetTrigger("TakingDamage");
        zombieAnimator.SetBool("Biting", false);
        zombieAnimator.SetBool("Running", true);
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        CheckIfDead();
    }

    private void ProcessSlideKick(SlideKick slideKick)
    {
        if(zombieRigidBody.velocity.y > KickSpeedLimit)
        {
            return;
        }
        if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
        {
            return;
        }
        health -= slideKick.GetDamage();
        zombieRigidBody.velocity += slideKick.GetSlideKickVelocity() * new Vector2(-DirectionOfPlayer(), 1);
        zombieAnimator.SetTrigger("TakingDamage");
        zombieAnimator.SetBool("Biting", false);
        zombieAnimator.SetBool("Running", true);
        CheckIfDead();

    }


    private void Run()
    {
        if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
        {
            return;
        }
        Vector2 zombieVeloctiy = new Vector2(DirectionOfPlayer() * runSpeed, zombieRigidBody.velocity.y);
        zombieRigidBody.velocity = zombieVeloctiy;
        zombieAnimator.SetBool("Crawling", false);
        zombieAnimator.SetBool("Biting", false);
        zombieAnimator.SetBool("Running", true);
    }

    private void Idle()
    {        
        zombieAnimator.SetBool("Running", false);
    }

    public void BitePlayer()
    {
        if (!player) { return; }
        player.PlayerDamage(biteDamage);
    }

    public void BitingBlood()
    {
        GameObject splurt = Instantiate(bitingBlood, mouth.transform.position, mouth.transform.rotation);
        Destroy(splurt, bleedingDuration);
    }



    private void FlipSprite()
    {
        transform.localScale = new Vector2(-DirectionOfPlayer(), 1);
    }
    
    private void FallModifier()
    {
        if (zombieRigidBody.velocity.y < 0)
        {
            zombieRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void StopMovingX()
    {
        CheckIfTouchingGround();
        if (touchingFloor)
        {
            Vector2 newVelocity = new Vector2(0, zombieRigidBody.velocity.x);
            zombieRigidBody.velocity = newVelocity;
        }
    }

    public void FallToDeath()
    {
        MakeRagDoll();
        alive = false;
        BleedWhenDead();
    }

    private void Die()
    {
        //GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        //Destroy(explosion, explosionDuration);
        //AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        float randomTime = Random.Range(0f, 1.5f);
        if (randomTime > 0.5f)
        {
            player.SloMoEvent(randomTime);
        }        
        DeathKick();
        MakeRagDoll();
        alive = false;
        BleedWhenDead();
    }

    private void DeathKick()
    {
        zombieRigidBody.constraints = RigidbodyConstraints2D.None;
        Vector2 newVelocity = new Vector2(zombieRigidBody.velocity.x, deathKickSpeed);
        zombieRigidBody.velocity = newVelocity;
    }

    private void BleedWhenDead()
    {
        StartCoroutine(MakeTorsoBleed());
    }
    


    IEnumerator MakeTorsoBleed()
    {
            if (!torsoBleeding)
            {
                torsoBleeding = true;
                //GameObject splurt = Instantiate(bloodSplurt, transform.position, transform.rotation);
                GameObject splurt = Instantiate(bloodSplurt, torsoBleedPoint.transform.position, torsoBleedPoint.transform.rotation);
                Destroy(splurt, bleedingDuration);
                yield return new WaitForSeconds(pulseRate);
                torsoBleeding = false;
            }        
    }
        

    private void MakeRagDoll()
    {
        if (ragDolled) { return; }
        zombieAnimator.enabled = false;
        zombieIK2D.enabled = false;
        bodyCollider.enabled = false;
        for (int i = 0; i < BodyParts.Length; i++)
        {
            var bodyPartRigidBody = BodyParts[i].GetComponent<Rigidbody2D>();
            bodyPartRigidBody.isKinematic = false;
            bodyPartRigidBody.velocity = zombieRigidBody.velocity;
            BodyParts[i].GetComponent<Collider2D>().enabled = true;
        }
        zombieRigidBody.isKinematic = true;
        zombieRigidBody.transform.position = mainBone.transform.position;
        ragDolled = true;
    }



    private void FootstepFx()
    {
        if (touchingFloor)
        {
            dust.Play();
            AudioSource.PlayClipAtPoint(footstepSFX, Camera.main.transform.position, footstepVolume);
        }
    }

}

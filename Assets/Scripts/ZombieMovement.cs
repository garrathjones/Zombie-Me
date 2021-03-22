using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    Zombie zombie;
    ZombieHealth zombieHealth;
    [SerializeField] float CanSeePlayerDistance = 40f;
    [SerializeField] float StopAtDistanceFromPlayer = 10f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float velocityCapX = 15f;
    [SerializeField] float velocityCapY = 15f;

    [SerializeField] float fallMultiplier = 3f;
    [SerializeField] float randomJumpMaxTime = 7f;
    [SerializeField] float randomJumpMinTime = 2f;
    [SerializeField] float zombieReactionTime = 0.6f;
    [SerializeField] BoxCollider2D groundCheckCollider;
    [SerializeField] public CapsuleCollider2D bodyCollider;
    [SerializeField] public BoxCollider2D crawlCollider;


    [SerializeField] AudioClip footstepSFX;
    [SerializeField] [Range(0, 1)] float footstepVolume = 0.3f;
    [SerializeField] ParticleSystem dust;

    public Rigidbody2D zombieRigidBody;
    PlayerPosition playerPosition;
    ZombieAttack zombieAttack;

    bool isMovementAICoroutineExecuting = false;
    bool touchingFloor;
    public bool playerInFiringRange = false;


    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponent<Zombie>();
        zombieHealth = GetComponent<ZombieHealth>();
        zombieRigidBody = GetComponent<Rigidbody2D>();
        zombieAttack = GetComponent<ZombieAttack>();
        playerPosition = GetComponent<PlayerPosition>();
        RandomJump();
    }

    private void Update()
    {
        PlayerInFiringRange();
    }

    private void FixedUpdate()
    {
        FallModifier();
        MovementAI();        
    }

    private void PlayerInFiringRange()
    {
        var distance = Vector3.Distance(playerPosition.GetPlayerPosition(), transform.position);
        if (distance < CanSeePlayerDistance)
        {
            playerInFiringRange = true;
        }
        if (distance > CanSeePlayerDistance)
        {
            playerInFiringRange = false;
        }

    }

    private void Run()
    {
        if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
        {
            return;
        }
        Vector2 zombieVeloctiy = new Vector2(playerPosition.DirectionOfPlayer() * runSpeed, zombieRigidBody.velocity.y);
        zombieRigidBody.velocity = zombieVeloctiy;
        zombie.zombieAnimator.SetBool("Crawling", false);
        zombie.zombieAnimator.SetBool("Biting", false);
        zombie.zombieAnimator.SetBool("Running", true);
    }

    private void VelocityCap()
    {
        if (zombieRigidBody.velocity.x > velocityCapX)
        {
            Vector2 cappedXVelocity = new Vector2(velocityCapX, zombieRigidBody.velocity.y);
            zombieRigidBody.velocity = cappedXVelocity;
        }
        if (zombieRigidBody.velocity.y > velocityCapY)
        {
            Vector2 cappedYVelocity = new Vector2(zombieRigidBody.velocity.x, velocityCapY);
            zombieRigidBody.velocity = cappedYVelocity;
        }
    }

    private void FallModifier()
    {
        if (zombieRigidBody.velocity.y < 0)
        {
            zombieRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void MovementAI()
    {
        if (zombieHealth.alive)
        {
            if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
            {
                return;
            }
            StartCoroutine(MovementAICoroutine());
        }
    }

    IEnumerator MovementAICoroutine()
    {
        if (isMovementAICoroutineExecuting)
        {
            yield break;
        }
        isMovementAICoroutineExecuting = true;
        yield return new WaitForSeconds(zombieReactionTime);
        if (zombieHealth.alive)
        {
            FlipSprite();
            var distance = Vector3.Distance(playerPosition.GetPlayerPosition(), transform.position);
            if (distance <= CanSeePlayerDistance && distance >= zombieAttack.bufferDistance + zombieAttack.biteRange && distance > StopAtDistanceFromPlayer)
            {
                Run();
            }
            if (distance > CanSeePlayerDistance || distance < StopAtDistanceFromPlayer)
            {
                Idle();
            }
        }
        isMovementAICoroutineExecuting = false;
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector2(-playerPosition.DirectionOfPlayer(), 1);
    }

    private void RandomJump()
    {
        StartCoroutine(RandomJumpCoroutine());
    }

    IEnumerator RandomJumpCoroutine()
    {
        while (zombieHealth.alive)
        {
            float randomJumpPeriod;
            randomJumpPeriod = UnityEngine.Random.Range(randomJumpMinTime, randomJumpMaxTime);
            yield return new WaitForSeconds(randomJumpPeriod);
            Jump();
        }
    }

    private void Idle()
    {
        zombie.zombieAnimator.SetBool("Running", false);
    }

    private void Jump()
    {
        if (!zombieHealth.alive) { return; }
        if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage"))
        {
            return;
        }
        if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
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


    private void StopMovingX()
    {
        CheckIfTouchingGround();
        if (touchingFloor)
        {
            Vector2 newVelocity = new Vector2(0, zombieRigidBody.velocity.x);
            zombieRigidBody.velocity = newVelocity;
        }
    }

    private void FootstepFx()
    {
        if (touchingFloor)
        {
            dust.Play();
            AudioSource.PlayClipAtPoint(footstepSFX, Camera.main.transform.position, footstepVolume);
        }
    }

    public void GiveZombieVelocityOnHit(Bullet bullet)
    {
        Vector2 bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
        if (bullet.isRocket)
        {
            Vector2 newVelocity = new Vector2(bulletVelocity.x * bullet.GetBulletBlastMultiplierX(), bullet.GetBulletBlastMultiplierY());
            zombieRigidBody.velocity = newVelocity;
        }
        if (!bullet.isRocket)
        {
            Vector2 newVelocity = new Vector2(zombieRigidBody.velocity.x * bullet.GetBulletBlastMultiplierX(), zombieRigidBody.velocity.y * bullet.GetBulletBlastMultiplierY());
            zombieRigidBody.velocity = newVelocity;
        }
    }

}

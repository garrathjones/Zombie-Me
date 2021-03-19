
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;


public class Player : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float almostDeadhealth = 30;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float runBob = 2f;
    [SerializeField] float slideSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float flipSlomoTime = 1f;
    [SerializeField] float fallMultiplier = 3f;
    //[SerializeField] float RagDollKick = 5f;
    [SerializeField] float RunningDisabledOnHitPeriod = 0.5f;
    [SerializeField] float deathKickSpeed = 5f;
    [SerializeField] float velocityCapX = 20f;
    [SerializeField] float velocityCapY = 20f;
    [SerializeField] float hitVelocityToCauseFlip = 8f;
    [SerializeField] public GameObject torsoLocation;
    [SerializeField] CapsuleCollider2D headCollider;
    [SerializeField] BoxCollider2D bodyCollider;
    [SerializeField] CapsuleCollider2D feetCollider;
    [SerializeField] BoxCollider2D groundCheckCollider;
    [SerializeField] Machete machete;
    [SerializeField] SlideKick slideKick;

    [SerializeField] AudioClip footstepSFX;
    [SerializeField] [Range(0, 1)] float footstepVolume = 0.3f;
    [SerializeField] AudioClip meleeSFX;
    [SerializeField] [Range(0, 1)] float meleeVolume = 1f;
    [SerializeField] ParticleSystem dust;


    bool ragDolled = false;
    bool alive = true;
    bool touchingFloor;
    bool hit = false;



    Animator playerAnimator;
    Rigidbody2D playerRigidBody;
    UnityEngine.U2D.IK.IKManager2D playerIK2D;
    public GameObject[] BodyParts;
    Pause pause;
    AlmostDeadBleed almostDeadBleed;
    SlomoController slomoController;


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerIK2D = GetComponent<UnityEngine.U2D.IK.IKManager2D>();
        almostDeadBleed = GetComponent<AlmostDeadBleed>();
        pause = FindObjectOfType<Pause>();
        slomoController = FindObjectOfType<SlomoController>();
    }

    private void Update()
    {
        if(!pause.paused)
        {
            FallModifier();
            if (alive)
            {
                Run();                            
                Flip();
                Jump();
                FlipSprite();
                MeleeAttack();
                Duck();
                UnDuck();
                VelocityCap();
                //if (Input.GetKeyDown("q"))
                //{
                //    Die();
                //}
                //if (Input.GetKeyDown("g"))
                //{
                //    ToggleSloMo();
                //}
            }
        }

    }

    private void VelocityCap()
    {
        if(playerRigidBody.velocity.x > velocityCapX)
        {
            Vector2 cappedXVelocity = new Vector2(velocityCapX, playerRigidBody.velocity.y);
            playerRigidBody.velocity = cappedXVelocity;
        }
        if (playerRigidBody.velocity.y > velocityCapY)
        {
            Vector2 cappedYVelocity = new Vector2(playerRigidBody.velocity.x, velocityCapY);
            playerRigidBody.velocity = cappedYVelocity;
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

    private void Run()
    {
        if(PlayerIsSliding())
        {
            return;
        }
        if(hit)
        {
            return;
        }
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVeloctiy = new Vector2(controlThrow * runSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVeloctiy;
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;        
        playerAnimator.SetBool("Running", playerHasHorizontalSpeed);
        Slide();
    }

    private void Duck()
    {
        
        if (!playerAnimator.GetBool("Duck"))
        {
            float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
            if (verticalInput < 0 && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                playerAnimator.SetBool("Duck", true);
            }
        }       
    
    }

    private void UnDuck()
    {
        if (playerAnimator.GetBool("Duck"))
        {
            float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
            float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
            if(horizontalInput!=0 || verticalInput >0)
            {
                playerAnimator.SetBool("Duck", false);
            }
        }
            
    }

    private void RunBob()
    {
        Vector2 newVelocity = new Vector2(0f, runBob);
        playerRigidBody.velocity += newVelocity;
    }


    private void Jump()
    {
        
        if (PlayerIsFlipping() || PlayerIsSliding())
        {
            return;
        }
        CheckIfTouchingGround();
        if (CrossPlatformInputManager.GetButtonDown("Jump") && touchingFloor == true)
        {
            AudioSource.PlayClipAtPoint(footstepSFX, Camera.main.transform.position, footstepVolume);
            Vector2 newVelocity = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
            playerRigidBody.velocity = newVelocity;
        }
    }

    private void Flip()
    {
        CheckIfTouchingGround();
        if (CrossPlatformInputManager.GetButtonDown("Jump") && touchingFloor == false)
        {
            if (PlayerIsFlipping() || PlayerIsSliding())
            {
                return;
            }
            FlipAnimation();
        }
    }

    private void FlipAnimation()
    {
        if (playerRigidBody.transform.localScale.x == 1)
        {
            slomoController.SlomoEvent(flipSlomoTime);
            playerAnimator.SetTrigger("FlipLeft");
        }
        if (playerRigidBody.transform.localScale.x == -1)
        {
            slomoController.SlomoEvent(flipSlomoTime);
            playerAnimator.SetTrigger("FlipRight");
        }
    }


    private void Slide()
    {

        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
        if (verticalInput < 0 && touchingFloor == true)
        {
            if (PlayerIsSliding())
            {
                return;
            }
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                if (playerRigidBody.transform.localScale.x == 1)
                {
                    playerAnimator.SetTrigger("SlideLeft");
                    Vector2 playerVeloctiy = new Vector2(-slideSpeed, playerRigidBody.velocity.y);
                    playerRigidBody.velocity = playerVeloctiy;
                }
                if (playerRigidBody.transform.localScale.x == -1)
                {
                    playerAnimator.SetTrigger("SlideRight");
                    Vector2 playerVeloctiy = new Vector2(slideSpeed, playerRigidBody.velocity.y);
                    playerRigidBody.velocity = playerVeloctiy;
                }
            }           

        }
    }


    private void FallModifier()
    {
        if (playerRigidBody.velocity.y < 0)
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
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

    private void MeleeAttack()
    {
        if (alive && Input.GetButtonDown("Fire2"))
        {
            playerAnimator.SetTrigger("Attacking");
            AudioSource.PlayClipAtPoint(meleeSFX, Camera.main.transform.position, meleeVolume);
        }
    }

    private void ProcessBulletHit(Bullet bullet)
    {
        hit = true;
        PlayerDamage(bullet.GetDamage());
        GivePlayerVelocityOnHit(bullet);
        bullet.DestroyBulletWithBloodSplat();
        float waitTime = bullet.GetDisablePlayerRunDuration();
        StartCoroutine(PlayerHit(waitTime));
    }

    IEnumerator PlayerHit(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hit = false;
    }
    private void GivePlayerVelocityOnHit(Bullet bullet)
    {
        Vector2 bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
        Vector2 newVelocity = new Vector2(bulletVelocity.x * bullet.GetBulletBlastMultiplierX(), Math.Abs(bulletVelocity.y * bullet.GetBulletBlastMultiplierY()));
        playerRigidBody.velocity = newVelocity;
        if(playerRigidBody.velocity.x > hitVelocityToCauseFlip || playerRigidBody.velocity.y > hitVelocityToCauseFlip)
        {
            FlipAnimation();
        }
    }


    public void PlayerDamage(float damage)
    {
        if (alive)
        {
            playerAnimator.SetTrigger("TakingDamage");
            health -= damage;
            if(health<=almostDeadhealth)
            {
                almostDeadBleed = FindObjectOfType<AlmostDeadBleed>();
                almostDeadBleed.AlmostDeadBleeding();
            }
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public float PlayerHealth()
    {
        return health;
    }

    public void FallToDeath()
    {
        health = 0;
        MakeRagDoll();
        slomoController.SlomoOn();
        alive = false;
        TriggerGameOver();
    }

    public void Die()
    {
        DeathKick();
        MakeRagDoll();
        slomoController.SlomoOn();
        alive = false;
        TriggerGameOver();

    }

    public void TriggerGameOver()
    {
        GameOver gameover = FindObjectOfType<GameOver>();
        gameover.EnableGameOverUI();
    }

    public bool PlayerAlive()
    {
        return alive;
    }

    private void FlipSprite()
    {
        if(PlayerIsFlipping() || PlayerIsSliding())
        {
            return;
        }
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;   // Mathf.Epsilon is better than using zero
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(-1 * Mathf.Sign(playerRigidBody.velocity.x), 1);
        }

    }

    private bool PlayerIsSliding()
    {
        if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlideLeft") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlideRight"))
        {
            return true;
        }
        return false;
    }

    private bool PlayerIsFlipping()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("FlipLeft") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("FlipRight"))
        {
            return true;
        }
        return false;
    }


    public Transform LocationOfTorso()
    {
        return torsoLocation.transform;
    }

    private void DeathKick()
    {
        playerRigidBody.constraints = RigidbodyConstraints2D.None;
        Vector2 newVelocity = new Vector2(playerRigidBody.velocity.x, deathKickSpeed);
        playerRigidBody.velocity = newVelocity;
    }

    private void MakeRagDoll()
    {
        if (ragDolled) { return; }
        playerRigidBody.constraints = RigidbodyConstraints2D.None;
        playerAnimator.enabled = false;
        playerIK2D.enabled = false;
        bodyCollider.enabled = false;
        feetCollider.enabled = false;
        headCollider.enabled = false;
        for (int i = 0; i < BodyParts.Length; i++)
        {
            var bodyPartRigidBody = BodyParts[i].GetComponent<Rigidbody2D>();
            bodyPartRigidBody.isKinematic = false;
            bodyPartRigidBody.velocity = playerRigidBody.velocity;
            BodyParts[i].GetComponent<Collider2D>().enabled = true;
        }
        ragDolled = true;

    }





    private void StepFx()
    {
        if (touchingFloor)
        {
            dust.Play();
            AudioSource.PlayClipAtPoint(footstepSFX, Camera.main.transform.position, footstepVolume);
        }
    }
    private void EnableMachete()
    {
        machete.EnableDamageCollider(true);
        machete.EnableTrail(true);
    }
    private void DisableMachete()
    {
        machete.EnableDamageCollider(false);
        machete.EnableTrail(false);
    }

    private void EnableSlideKick()
    {
        slideKick.EnableDamageCollider(true);
    }
    private void DisableSlideKick()
    {
        slideKick.EnableDamageCollider(false);
    }

}

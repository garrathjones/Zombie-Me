
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;


public class Player : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float runBob = 2f;
    [SerializeField] float slideSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float fallMultiplier = 3f;
    //[SerializeField] float RagDollKick = 5f;
    [SerializeField] float deathKickSpeed = 5f;
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
    public bool isSloMoEnabled = false;


    Animator playerAnimator;
    Rigidbody2D playerRigidBody;
    UnityEngine.U2D.IK.IKManager2D playerIK2D;
    public GameObject[] BodyParts;
    Pause pause;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerIK2D = GetComponent<UnityEngine.U2D.IK.IKManager2D>();
        pause = FindObjectOfType<Pause>();
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



    private void Run()
    {
        if(PlayerIsSliding())
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

    private void RunBob()
    {
        Vector2 newVelocity = new Vector2(0f, runBob);
        playerRigidBody.velocity += newVelocity;
    }


    private void Jump()
    {
        if(PlayerIsFlipping() || PlayerIsSliding())
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
            if (playerRigidBody.transform.localScale.x == 1)
            {
                playerAnimator.SetTrigger("FlipLeft");
            }
            if (playerRigidBody.transform.localScale.x == -1)
            {
                playerAnimator.SetTrigger("FlipRight");
            }
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




    public void PlayerDamage(float damage)
    {
        if (alive)
        {
            playerAnimator.SetTrigger("TakingDamage");
            health -= damage;
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
        SloMoOn();
        alive = false;
        TriggerGameOver();
    }

    public void Die()
    {
        DeathKick();
        MakeRagDoll();
        SloMoOn();
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

    public void SloMoOn()
    {
        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isSloMoEnabled = true;
    }

    public void SloMoOff()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isSloMoEnabled = false;
    }

    private void ToggleSloMo()
    {
        if (!isSloMoEnabled)
        {
            SloMoOn();
        }
        else if (isSloMoEnabled)
        {
            SloMoOff();
        }
    }

    public void SloMoEvent(float sloMoTime)
    {
        StartCoroutine(SloMoEventCoroutine(sloMoTime));
    }

    IEnumerator SloMoEventCoroutine(float sloMoTime)
    {
        SloMoOn();
        yield return new WaitForSeconds(sloMoTime);
        SloMoOff();
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

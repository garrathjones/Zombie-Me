﻿
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float almostDeadhealth = 30;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float animateRunSpeed = 1f;
    [SerializeField] float slideSpeedOffset = 2f;
    [SerializeField] float slideDelay = 0.5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float pipeThrustOffset = 0f;
    [SerializeField] float pipeHitDelayTime = 0.3f;
    [SerializeField] float flipSlomoTime = 1f;
    [SerializeField] float fallMultiplier = 3f;
    [SerializeField] float deathKickSpeed = 5f;
    [SerializeField] float ragDollSpinMin = -500f;
    [SerializeField] float ragDollSpinMax = 500f;
    [SerializeField] float velocityCapX = 20f;
    [SerializeField] float velocityCapY = 20f;
    [SerializeField] float hitVelocityToCauseFlip = 8f;
    [SerializeField] public GameObject torsoLocation;
    [SerializeField] CapsuleCollider2D headCollider;
    [SerializeField] BoxCollider2D bodyCollider;
    [SerializeField] CapsuleCollider2D feetCollider;
    [SerializeField] BoxCollider2D groundCheckCollider;
    [SerializeField] Machete machete;
    [SerializeField] Gun gun;
    [SerializeField] SlideKick slideKick;

    [SerializeField] AudioClip footstepSFX;
    [SerializeField] [Range(0, 1)] float footstepVolume = 0.3f;
    [SerializeField] AudioClip meleeSFX;
    [SerializeField] [Range(0, 5)] float meleeVolume = 1f;
    [SerializeField] ParticleSystem dust;
    [SerializeField] AudioClip slideSFX;
    [SerializeField] [Range(0, 1)] float slideVolume = 1f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 10)] float deathVolume = 1f;

    [SerializeField] float pulseRate = 0.5f;
    [SerializeField] GameObject bloodSplurt;
    [SerializeField] GameObject torsoBleedPoint;
    [SerializeField] float bleedDuration = 5f;

    //float smallDelay = 0.5f;
    bool ragDolled = false;
    bool alive = true;
    bool disabled = false;
    bool touchingFloor;
    bool hit = false;
    bool pipeHit = false;
    bool torsoBleeding = false;
    bool sliding = false;
    //bool jumping = false;

    Animator playerAnimator;
    Rigidbody2D playerRigidBody;
    UnityEngine.U2D.IK.IKManager2D playerIK2D;
    public GameObject[] BodyParts;
    Pause pause;
    AlmostDeadBleed almostDeadBleed;
    SlomoController slomoController;
    CinemachineSwitcher cinemachineSwitcher;
    Win win;
    Parallax parallax;


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerIK2D = GetComponent<UnityEngine.U2D.IK.IKManager2D>();
        almostDeadBleed = GetComponent<AlmostDeadBleed>();
        pause = FindObjectOfType<Pause>();
        slomoController = FindObjectOfType<SlomoController>();
        cinemachineSwitcher = FindObjectOfType<CinemachineSwitcher>();
        win = FindObjectOfType<Win>();
        parallax = FindObjectOfType<Parallax>();
    }

    void Update()
    {
        if(!pause.paused)
        {
            FallModifier();
            VelocityCap();
            if (alive && !disabled)
            {
                if(!hit)
                {
                    Run();
                    FlipSprite();
                }
                Flip();
                Jump();                
                MeleeAttack();
                Duck();
                UnDuck();                
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
        if (pipeHit)
        {
            return;
        }
        pipeHit = true;
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x + pipe.thrustX, playerRigidBody.velocity.y + pipe.thrustY + pipeThrustOffset);
        StartCoroutine(PipeHitDelay(pipeHitDelayTime));
    }

    IEnumerator PipeHitDelay(float waitTime)
    {
        yield return new WaitForSeconds(pipeHitDelayTime);
        pipeHit = false;
    }

    private void Run()
    {
        if(hit) { return; }
        HorizontalMotionControl(); 
        bool animateRun = Mathf.Abs(playerRigidBody.velocity.x) > animateRunSpeed;
        playerAnimator.SetBool("Running", animateRun);
        Slide();
    }

    private void HorizontalMotionControl()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        if(PlayerSlidingAnimationIsPlaying())
        {
            //give player velocity when sliding left
            if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlideLeft") && controlThrow < 0)
            {
                Vector2 playerVeloctiy = new Vector2(controlThrow * (runSpeed + slideSpeedOffset), playerRigidBody.velocity.y);
                playerRigidBody.velocity = playerVeloctiy;
            }
            //give player velocity when sliding right
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlideRight") && controlThrow > 0)
            {
                Vector2 playerVeloctiy = new Vector2(controlThrow * (runSpeed + slideSpeedOffset), playerRigidBody.velocity.y);
                playerRigidBody.velocity = playerVeloctiy;
            }
            //allow player to reduce velocity when sliding left
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlideLeft") && controlThrow > 0 && playerRigidBody.velocity.x < 0)
            {
                Vector2 playerVeloctiy = new Vector2(playerRigidBody.velocity.x + controlThrow, playerRigidBody.velocity.y);
                playerRigidBody.velocity = playerVeloctiy;
            }
            //allow player to reduce velocity when sliding right
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlideRight") && controlThrow < 0 && playerRigidBody.velocity.x > 0)
            {
                Vector2 playerVeloctiy = new Vector2(playerRigidBody.velocity.x + controlThrow, playerRigidBody.velocity.y);
                playerRigidBody.velocity = playerVeloctiy;
            }

        }
        else
        {
            Vector2 playerVeloctiy = new Vector2(controlThrow * runSpeed, playerRigidBody.velocity.y);
            playerRigidBody.velocity = playerVeloctiy;
        }                
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


    private void Jump()
    {        
        if (PlayerFlippingAnimationIsPlaying() || PlayerSlidingAnimationIsPlaying())
        {
            return;
        }
        CheckIfTouchingGround();
        if (CrossPlatformInputManager.GetButtonDown("Jump") && touchingFloor == true)
        {
            AudioSource.PlayClipAtPoint(footstepSFX, Camera.main.transform.position, footstepVolume);
            if(playerRigidBody.velocity.y > jumpSpeed/2)
            {
                Vector2 newVelocity = new Vector2(playerRigidBody.velocity.x, playerRigidBody.velocity.y + jumpSpeed/2);
                playerRigidBody.velocity = newVelocity;
            }
            else
            {
                Vector2 newVelocity = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
                playerRigidBody.velocity = newVelocity;
            }
        }
    }


    private void Flip()
    {
        CheckIfTouchingGround();
        if (CrossPlatformInputManager.GetButtonDown("Jump") && touchingFloor == false)
        {
            if (PlayerFlippingAnimationIsPlaying() || PlayerSlidingAnimationIsPlaying())
            {
                return;
            }
            FlipAnimationWithSlomo();
        }
    }

    private void FlipAnimationWithSlomo()
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

    private void FlipAnimation()
    {
        if (playerRigidBody.transform.localScale.x == 1)
        {
            playerAnimator.SetTrigger("FlipLeft");
        }
        if (playerRigidBody.transform.localScale.x == -1)
        {
            playerAnimator.SetTrigger("FlipRight");
        }
    }

    private void Slide()
    {

        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
        if (verticalInput < 0)
            {
            if (PlayerSlidingAnimationIsPlaying() || sliding)
            {
                return;
            }
            sliding = true;
            headCollider.enabled = false;
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                if (playerRigidBody.transform.localScale.x == 1)
                {
                    playerAnimator.SetTrigger("SlideLeft");
                    AudioSource.PlayClipAtPoint(slideSFX, Camera.main.transform.position, slideVolume);
                }
                if (playerRigidBody.transform.localScale.x == -1)
                {
                    playerAnimator.SetTrigger("SlideRight");
                    AudioSource.PlayClipAtPoint(slideSFX, Camera.main.transform.position, slideVolume);
                }
            }
            StartCoroutine(SlidingDelay(slideDelay));
        }
    }

    IEnumerator SlidingDelay(float slideDelay)
    {
        yield return new WaitForSeconds(slideDelay);
        headCollider.enabled = true;
        sliding = false;
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
        if(bullet.isRocket)
        {
            //to fix bug where background is lost after camera shake, reset the z pos of background each time player is hit
            parallax.ResetZPos();
            cinemachineSwitcher.ExplosionCamera();
        }
        hit = true;
        GivePlayerVelocityOnHit(bullet);
        PlayerDamage(bullet.GetDamage());
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
        if(alive)
        {
            Vector2 bulletVelocity = bullet.GetComponent<Rigidbody2D>().velocity;
            if (bullet.isRocket)
            {
                Vector2 newVelocity = new Vector2(bulletVelocity.x * bullet.GetBulletBlastMultiplierX(), bullet.GetBulletBlastMultiplierY());
                playerRigidBody.velocity = newVelocity;
            }
            if (!bullet.isRocket)
            {
                Vector2 newVelocity = new Vector2(playerRigidBody.velocity.x * bullet.GetBulletBlastMultiplierX(), bullet.GetBulletBlastMultiplierY());
                playerRigidBody.velocity = newVelocity;
            }
            if (playerRigidBody.velocity.x > hitVelocityToCauseFlip || playerRigidBody.velocity.y > hitVelocityToCauseFlip)
            {
                FlipAnimation();
            }
        }
    }



    public void PlayerDamage(float damage)
    {
        if (alive && !win.levelClear)
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
        machete.DropMachete();
        gun.DropGun();
        BleedWhenDead();
        MakeRagDoll();
        slomoController.SlomoOn();
        alive = false;
        TriggerGameOver();
    }

    public void Die()
    {
        DeathKick();
        health = 0;
        machete.DropMachete();
        gun.DropGun();
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
        BleedWhenDead();
        MakeRagDoll();
        slomoController.SlomoOn();
        alive = false;
        TriggerGameOver();       
    }
    public void TriggerGameOver()
    {
        GameOver gameover = FindObjectOfType<GameOver>();
        gameover.EnableGameOverUI();
        string activeScene = SceneManager.GetActiveScene().name;
    }

    public bool PlayerAlive()
    {
        return alive;
    }

    private void FlipSprite()
    {
        if(PlayerFlippingAnimationIsPlaying() || PlayerSlidingAnimationIsPlaying())
        {
            return;
        }
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;   // Mathf.Epsilon is better than using zero
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(-1 * Mathf.Sign(playerRigidBody.velocity.x), 1);
        }

    }

    private bool PlayerSlidingAnimationIsPlaying()
    {
        if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlideLeft") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlideRight"))
        {
            return true;
        }
        return false;
    }

    private bool PlayerFlippingAnimationIsPlaying()
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

    public void DisablePlayer()
    {
        disabled = true;
    }

    public void MakeRagDoll()
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
            if(BodyParts[i].GetComponent<PlayerBodyPart>().isTorso)
            {            
                float randomSpin = UnityEngine.Random.Range(ragDollSpinMin, ragDollSpinMax);
                bodyPartRigidBody.angularVelocity = randomSpin;
            }
            BodyParts[i].GetComponent<Collider2D>().enabled = true;
        }
        ragDolled = true;
        playerRigidBody.bodyType = RigidbodyType2D.Static;
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

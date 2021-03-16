using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] float biteDamage = 10f;

    [SerializeField] public float bufferDistance = 0.8f;
    [SerializeField] public float biteRange = 0.5f;
    [SerializeField] float biteRangeWhenPlayerDead = 1.2f;
    [SerializeField] float crawlHeight = 0.8f;

    [SerializeField] GameObject mouth;
    [SerializeField] GameObject bitingBlood;
    [SerializeField] float biteBloodDuration = 5f;

    [SerializeField] AudioClip biteSFX;
    [SerializeField] [Range(0, 1)] float biteVolume = 1f;

    ZombieHealth zombieHealth;
    Zombie zombie;
    ZombieMovement zombieMovement;
    Player player;
    PlayerPosition playerPosition;


    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponent<Zombie>();
        zombieHealth = GetComponent<ZombieHealth>();
        zombieMovement = GetComponent<ZombieMovement>();
        player = FindObjectOfType<Player>();
        playerPosition = GetComponent<PlayerPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (zombieHealth.alive)
        {
            AttackAI();
        }
    }

    void AttackAI()
    {
        if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage") || zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Biting"))
        {
            return;
        }
        if (zombieHealth.alive)
        {
            if (!player.PlayerAlive())
            {
                biteRange = biteRangeWhenPlayerDead;
            }
            var distance = Vector3.Distance(playerPosition.GetPlayerPosition(), transform.position);
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

    private void Biting()
    {
        if (zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakingDamage") || zombie.zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Biting"))
        {
            return;
        }
        var zombieYDistanceFromPlayer = zombieMovement.zombieRigidBody.position.y - playerPosition.GetPlayerPosition().y;
        if (!player.PlayerAlive() && zombieYDistanceFromPlayer > crawlHeight)
        {
            zombie.zombieAnimator.SetBool("Crawling", true);
            zombieMovement.bodyCollider.enabled = false;
            zombieMovement.crawlCollider.enabled = true;
        }
        else
        {
            zombieMovement.crawlCollider.enabled = false;
            zombieMovement.bodyCollider.enabled = true;
            zombie.zombieAnimator.SetBool("Biting", true);
        }
    }

    private void StopBiting()
    {
        zombie.zombieAnimator.SetBool("Biting", false);
        zombie.zombieAnimator.SetBool("Crawling", false);
    }

    public void BitePlayer()
    {
        if (!player) { return; }
        player.PlayerDamage(biteDamage);
    }

    public void BitingBlood()
    {
        GameObject splurt = Instantiate(bitingBlood, mouth.transform.position, mouth.transform.rotation);
        AudioSource.PlayClipAtPoint(biteSFX, Camera.main.transform.position, biteVolume);
        Destroy(splurt, biteBloodDuration);
    }
}

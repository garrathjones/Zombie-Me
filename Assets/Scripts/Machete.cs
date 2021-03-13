using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machete : MonoBehaviour
{
    [SerializeField] int damage = 50;
    [SerializeField] float macheteHitXVelocity = 8f;
    [SerializeField] float macheteHitYVelocity = 2f;
    [SerializeField] float macheteDeathHitXVelocity = 5f;
    [SerializeField] float macheteDeathHitYVelocity = 10f;
    [SerializeField] GameObject bloodSplat;
    [SerializeField] float splatDuration = 5f;
    [SerializeField] TrailFX trail;

    [SerializeField] AudioClip machetteHitSFX;
    [SerializeField] [Range(0, 1)] float machetteHitVolume = 1f;


    BoxCollider2D macheteDamageCollider;
    CapsuleCollider2D macheteDropCollider;
    Rigidbody2D macheteRigidBody;
    Player player;
 


    private void Start()
    {
        macheteDamageCollider = GetComponent<BoxCollider2D>();
        macheteDropCollider = GetComponent<CapsuleCollider2D>();
        macheteRigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();

    }

    private void Update()
    {
        if (!player.PlayerAlive())
        {
            DropMachete();
        }
    }
    public int GetDamage()
    {
        return damage;
    }

    public void CreateMacheteHitFX()
    {
        AudioSource.PlayClipAtPoint(machetteHitSFX, Camera.main.transform.position, machetteHitVolume);
        GameObject splat = Instantiate(bloodSplat, transform.position, transform.rotation);
        Destroy(splat, splatDuration);
    }     
      
    public Vector2 GetMacheteHitVelocity()
    {
        return new Vector2(macheteHitXVelocity, macheteHitYVelocity);
    }



    public void EnableDamageCollider(bool enableOrDisable)
    {
        macheteDamageCollider.enabled = enableOrDisable;
    }
    public void EnableTrail(bool enableOrDisable)
    {
        trail.EnableTrailFX(enableOrDisable);
    }




    private void DropMachete()
    {
        macheteRigidBody.isKinematic = false;
        macheteDropCollider.enabled = true;
    }

    public Vector2 GetMacheteDeathHitVelocty()
    {
        return new Vector2(macheteDeathHitXVelocity, macheteDeathHitYVelocity);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideKick : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] float slideHitXVelocity = 1f;
    [SerializeField] float slideHitYVelocity = 5f;
    [SerializeField] GameObject bloodSplat;
    [SerializeField] float splatDuration = 3f;
    [SerializeField] AudioClip hitBySlideSFX;
    [SerializeField] [Range(0, 1)] float hitBySlideSFXVolume = 1f;

    BoxCollider2D slideDamageCollider;
    Player player;

    void Start()
    {
        slideDamageCollider = GetComponent<BoxCollider2D>();
    }

    public void EnableDamageCollider(bool enableOrDisable)
    {
        slideDamageCollider.enabled = enableOrDisable;
    }

    public void PlaySlideKickHitFX()
    {
        GameObject blood = Instantiate(bloodSplat, transform.position, transform.rotation);
        Destroy(blood, splatDuration);
        AudioSource.PlayClipAtPoint(hitBySlideSFX, Camera.main.transform.position, hitBySlideSFXVolume);
    }
        


    public int GetDamage()
    {
        return damage;
    }

    public Vector2 GetSlideKickVelocity()
    {
        return new Vector2(slideHitXVelocity, slideHitYVelocity);
    }
}

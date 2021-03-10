using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideKick : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] float slideHitXVelocity = 1f;
    [SerializeField] float slideHitYVelocity = 5f;

    BoxCollider2D slideDamageCollider;
    Player player;

    void Start()
    {
        slideDamageCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableDamageCollider(bool enableOrDisable)
    {
        slideDamageCollider.enabled = enableOrDisable;
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

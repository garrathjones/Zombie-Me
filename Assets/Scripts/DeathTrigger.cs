using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] GameObject bloodSplat;
    [SerializeField] float splatDuration = 5f;

    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathVolume = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        Zombie zombie = other.gameObject.GetComponent<Zombie>();

        if (!player && !zombie) { return; }
        if (player)
        {
            player.FallToDeath();
            DeathSFX();
            GameObject splat = Instantiate(bloodSplat, player.transform.position, player.transform.rotation);
            Destroy(splat, splatDuration);

        }
        if (zombie)
        {
            zombie.FallToDeath();
            DeathSFX();
            GameObject splat = Instantiate(bloodSplat, zombie.transform.position, zombie.transform.rotation);
        }

    }

    private void DeathSFX()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
    }
        



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
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
        }
        if (zombie)
        {
            zombie.FallToDeath();
        }

    }


}

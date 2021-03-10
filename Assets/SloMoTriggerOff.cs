using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloMoTriggerOff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (!player) { return; }
        if (player)
        {
            player.SloMoOff();
        }
    }
}

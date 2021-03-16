using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlomoTriggerOn : MonoBehaviour
{
    SlomoController slomoController;

    private void Start()
    {
        slomoController = FindObjectOfType<SlomoController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (!player) { return; }
        if (player)
        {
            slomoController.SlomoOn();
        }
    }
}






using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    Player player;
    Transform playerTransform;
    Transform playerTorsoPosition;

    void Start()
    {
        player = FindObjectOfType<Player>();
        playerTransform = player.transform;
        playerTorsoPosition = player.LocationOfTorso();
    }

    public Vector2 GetPlayerPosition()
    {
        if (player.PlayerAlive())
        {
            return playerTransform.position;
        }
        else
            return playerTorsoPosition.position;
    }

    public int DirectionOfPlayer()
    {
        if (GetPlayerPosition().x > transform.position.x)
        {
            return 1;
        }
        else
            return -1;
    }

}

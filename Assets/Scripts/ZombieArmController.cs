using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieArmController : MonoBehaviour
{
    PlayerPosition playerPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = FindObjectOfType<PlayerPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = new Vector2(playerPosition.GetPlayerPosition().x, playerPosition.GetPlayerPosition().y);
        transform.position = pos;
    }
}

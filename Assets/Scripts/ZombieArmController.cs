using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieArmController : MonoBehaviour
{
    PlayerPosition playerPosition;
    [SerializeField] float yAimOffset = 0.5f;
    [SerializeField] float aimingPeriod = 2f;
    [SerializeField] [Range(0, 5)] float aimInaccuracy = 1f;
    bool regeneratingAimInaccuracy = false;
    float randomInaccuracy;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = FindObjectOfType<PlayerPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        Aiming();
        if(!regeneratingAimInaccuracy)
        {
            RefreshAimInaccuracy();
        }
       
    }

    void Aiming()
    {
        Vector2 pos = new Vector2(playerPosition.GetPlayerPosition().x, playerPosition.GetPlayerPosition().y + yAimOffset + randomInaccuracy);
        transform.position = pos;
    }

    void RefreshAimInaccuracy()
    {
        StartCoroutine(AimInaccurayCoroutine());
    }

    IEnumerator AimInaccurayCoroutine()
    {
        regeneratingAimInaccuracy = true;
        randomInaccuracy = Random.Range(-aimInaccuracy, aimInaccuracy);
        yield return new WaitForSeconds(aimingPeriod);
        regeneratingAimInaccuracy = false;
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    private int kills = 0;

    void Start()
    {
        kills = 0;
    }
    public void AddKill()
    {
        kills++;
        Debug.Log("you did a Kill!, total kills = " + kills);
    }
    public int ReadKills()
    {
        return kills;
    }
        


}

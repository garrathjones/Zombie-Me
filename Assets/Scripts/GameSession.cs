using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    public bool gameWon = false;
    private float finalTime = 0f;
    private int finalKills = 0;

    
    Timer timer;
    KillCounter killCounter;
    
    private void Awake()
    {
        SetUpSingleton();
    }



    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

   public void GameWon()
   {
        timer = FindObjectOfType<Timer>();
        finalTime = timer.ReadTimer();
        killCounter = FindObjectOfType<KillCounter>();
        finalKills = killCounter.ReadKills();
        gameWon = true;
   }


    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public float GetEndTime()
    {
        return finalTime;
    }
    public int GetFinalKills()
    {
        return finalKills;
    }
}

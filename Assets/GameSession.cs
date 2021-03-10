using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    public bool gameWon = false;
    private float finalTime = 0f;
    
    Timer timer;
    
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
}

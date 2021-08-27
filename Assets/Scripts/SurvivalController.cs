using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalController : MonoBehaviour
{
    [SerializeField] Text enemiesRemainingText;
    [SerializeField] int enemiesInWave = 50;
    [SerializeField] bool disablePlayeronWin = true;
    int enemiesRemaining;
    Win win;
    Player player;
    
    void Start()   
    {
        enemiesRemaining = enemiesInWave;
        enemiesRemainingText.text = enemiesRemaining.ToString();
        win = FindObjectOfType<Win>();
        player = FindObjectOfType<Player>();
    }

    public void UpdateEnemiesRemaining()
    {
        enemiesRemaining--;
        enemiesRemainingText.text = enemiesRemaining.ToString();
        if(enemiesRemaining<=0)
        {
            if(disablePlayeronWin)
            {
                player.DisablePlayer();
            }
            win.levelClear = true;
            win.EnablewinUI();
        }
    }


}

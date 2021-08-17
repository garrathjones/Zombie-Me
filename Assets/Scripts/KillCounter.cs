using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    [SerializeField] Text killsDisplayText;
    [SerializeField] bool survivalMode = false;
    private int kills = 0;
    GameOver gameOver;

    SurvivalController survivalController;

    void Start()
    {
        kills = 0;
        killsDisplayText.text = "KILLS " + kills.ToString();
        gameOver = FindObjectOfType<GameOver>();
        if(survivalMode)
        {
            survivalController = FindObjectOfType<SurvivalController>();
        }
    }
    public void AddKill()
    {
        if(!gameOver.gameOver)
        {
            kills++;
            killsDisplayText.text = "KILLS " + kills.ToString();
            if(survivalMode)
            {
                survivalController.UpdateEnemiesRemaining();
            }
        }
    }
    public int ReadKills()
    {
        return kills;
    }
        


}

using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    [SerializeField] Text killsDisplayText;
    private int kills = 0;
    GameOver gameOver;

    void Start()
    {
        kills = 0;
        killsDisplayText.text = kills.ToString();
        gameOver = FindObjectOfType<GameOver>();
    }
    public void AddKill()
    {
        if(!gameOver.gameOver)
        {
            kills++;
            killsDisplayText.text = kills.ToString();
        }
    }
    public int ReadKills()
    {
        return kills;
    }
        


}

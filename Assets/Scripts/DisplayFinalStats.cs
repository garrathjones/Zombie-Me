using UnityEngine;
using UnityEngine.UI;

public class DisplayFinalStats : MonoBehaviour
{
    GameSession gameSession;

    public Text finalTime;
    public Text finalKills;
    private decimal finalTimeDecimal;    

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();

        finalTimeDecimal = (decimal)gameSession.GetEndTime();
        finalTime.text = finalTimeDecimal.ToString("F");
        finalKills.text = gameSession.GetFinalKills().ToString();
    }
}

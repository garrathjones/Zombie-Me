using UnityEngine;
using UnityEngine.UI;

public class DisplayFinalTime : MonoBehaviour
{
    GameSession gameSession;
    public Text finalTime;
    private decimal finalTimeDecimal;
    
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        finalTimeDecimal = (decimal)gameSession.GetEndTime();
        finalTime.text = finalTimeDecimal.ToString("F");
    }
}

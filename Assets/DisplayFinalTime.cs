using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFinalTime : MonoBehaviour
{
    GameSession gameSession;
    public Text finalTime;
    
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        finalTime.text = gameSession.GetEndTime().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{

    [SerializeField] Button menuButton;
    [SerializeField] Text gameOverText;
    [SerializeField] Button retryButton;
    [SerializeField] TextMeshProUGUI retryText;
    [SerializeField] TextMeshProUGUI menuButtonText;
    Timer timer;

    public bool gameOver = false;

    public void EnableGameOverUI()
    {
        gameOverText.enabled = true;
        menuButton.enabled = true;
        menuButtonText.enabled = true;
        retryButton.enabled = true;
        retryText.enabled = true;
        gameOver = true;
        timer = FindObjectOfType<Timer>();
        timer.StopTimer();
        return;
    }

}

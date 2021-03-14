using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] Text gameOverText;
    [SerializeField] Button menuButton;
    [SerializeField] TextMeshProUGUI menuButtonText;

    public bool gameOver = false;

    public void EnableGameOverUI()
    {
        gameOverText.enabled = true;
        menuButton.enabled = true;
        menuButtonText.enabled = true;
        gameOver = true;
        return;
    }

}

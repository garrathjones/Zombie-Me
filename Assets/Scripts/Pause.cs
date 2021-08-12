using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] Text pauseText;
    [SerializeField] Button menuButton;
    [SerializeField] TextMeshProUGUI menuButtonText;

    SlomoController slomoController;
    GameOver gameOver;

    public bool paused = false;

    private void Start()
    {
        slomoController = FindObjectOfType<SlomoController>();
    }
    private void Update()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown("escape"))
        {
            gameOver = FindObjectOfType<GameOver>();
            if (!paused && !gameOver.gameOver)
            {
                EnablePauseUI();
                Time.timeScale = 0;
            }
            else
            {
                DisablePauseUI();
                if (slomoController.isSlomoEnabled)
                {
                    slomoController.SlomoOn();
                }
                else
                {
                    Time.timeScale = 1;
                }
            }
        }
    }



    public void EnablePauseUI()
    {
        pauseText.enabled = true;
        menuButton.enabled = true;
        menuButtonText.enabled = true;
        paused = true;
        return;
    }
    public void DisablePauseUI()
    {
        pauseText.enabled = false;
        menuButton.enabled = false;
        menuButtonText.enabled = false;
        paused = false;
        return;
    }
}

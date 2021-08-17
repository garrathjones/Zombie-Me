using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] TextMeshProUGUI continueText;
    [SerializeField] Button menuButton;
    [SerializeField] TextMeshProUGUI menuText;
    [SerializeField] Button retryButton;
    [SerializeField] TextMeshProUGUI retryText;


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
                Continue();
            }
        }
    }

    public void Continue()
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

    public void EnablePauseUI()
    {
        continueButton.enabled = true;
        continueText.enabled = true;
        menuButton.enabled = true;
        menuText.enabled = true;
        retryButton.enabled = true;
        retryText.enabled = true;
        paused = true;
        return;
    }
    public void DisablePauseUI()
    {
        continueButton.enabled = false;
        continueText.enabled = false;
        menuButton.enabled = false;
        menuText.enabled = false;
        retryButton.enabled = false;
        retryText.enabled = false;
        paused = false;
        return;
    }
}

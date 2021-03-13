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

    public bool paused = false;

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if(!paused)
            {
                EnablePauseUI();
                Time.timeScale = 0;
            }
            else
            {
                DisablePauseUI();
                Time.timeScale = 1;
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

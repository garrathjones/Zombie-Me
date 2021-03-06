using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    private float timer;
    public Text timerText;
    bool timerRunning = true;
    private decimal decimalTimer;

    void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        if(timerRunning)
        {
            timer += Time.deltaTime;
            decimalTimer = (decimal)timer;
            timerText.text = decimalTimer.ToString("F");
        }

    }

    public float ReadTimer()
    {
        return timer;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
    public void StartTimer()
    {
        timerRunning = true;
    }




}

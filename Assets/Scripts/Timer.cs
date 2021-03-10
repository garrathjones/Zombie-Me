using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timer;
    public Text timerText;
    bool timerRunning = true;

    void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        if(timerRunning)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString();
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




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] public float thrustY = 10f;
    [SerializeField] public float thrustX = 0f;

    [SerializeField] ParticleSystem steamFX;
    [SerializeField] CapsuleCollider2D steamCollider;

    [SerializeField] public float timeOn = 3f;
    [SerializeField] public float timeOff = 1f;

    bool steamActive = false;

    private void Start()
    {
        var main = steamFX.main;
        main.duration = timeOn;
    }


    private void Update()
    {
        if(!steamActive)
        {
            StartCoroutine(SteamOn(timeOn));
        }        
    }

    IEnumerator SteamOn(float timeOn)
    {
        steamActive = true;  
        steamCollider.enabled = true; 
        steamFX.Play();
        yield return new WaitForSeconds(timeOn);
        StartCoroutine(SteamOff(timeOff));
    }

    IEnumerator SteamOff(float timeOff)
    {
        steamCollider.enabled = false;
        steamFX.Stop();
        yield return new WaitForSeconds(timeOff);
        steamActive = false;
    }
}

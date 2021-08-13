using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    GameSession gameSession;
    Timer timer;
    Player player;
    [SerializeField] float levelEndDelay = 5f;
    [SerializeField] [Range(0, 1)] float levelEndVolume = 1f;
    [SerializeField] AudioClip levelExitSFX;
    [SerializeField] ParticleSystem levelEndSplurt;



    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player)
        {
            //AudioSource.PlayClipAtPoint(levelExitSFX, Camera.main.transform.position);
            gameSession = FindObjectOfType<GameSession>();
            gameSession.GameWon();
            timer = FindObjectOfType<Timer>();
            timer.StopTimer();
            AudioSource.PlayClipAtPoint(levelExitSFX, Camera.main.transform.position, levelEndVolume);
            player = FindObjectOfType<Player>();
            player.DisablePlayer();
            levelEndSplurt.Play();
            StartCoroutine(WinCoroutine(levelEndDelay));  
        }
        return;
    }

    IEnumerator WinCoroutine(float levelEndDelay)
    {
        yield return new WaitForSeconds(levelEndDelay);
        LoadWinScene();
    }


    public void LoadWinScene()
    {
        SceneManager.LoadScene("AdventureWin");
    }
}

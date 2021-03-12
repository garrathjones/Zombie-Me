using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    GameSession gameSession;
    Timer timer;

    
    //[SerializeField] AudioClip levelExitSFX;

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
            LoadWinScene();
        }
        return;
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("SpeedRunWin");
    }
}

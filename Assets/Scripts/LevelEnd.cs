using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    //[SerializeField] AudioClip levelExitSFX;

    void OnTriggerEnter2D(Collider2D other)
    {
        //AudioSource.PlayClipAtPoint(levelExitSFX, Camera.main.transform.position);
        LoadWinScene();
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("SpeedRunWin");
    }
}

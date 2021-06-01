using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadSurvival()
    {
        SceneManager.LoadScene("Survival");
    }

    public void LoadSurvivalWin()
    {
        SceneManager.LoadScene("SurvivalWin");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SurvivalGameOver : MonoBehaviour
{
    GameSession gameSession;

    public void SurvivalWin()
    {
        gameSession = FindObjectOfType<GameSession>();
        gameSession.GameWon();
        SceneManager.LoadScene("SurvivalWin");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    [SerializeField] Text killsDisplayText;
    private int kills = 0;

    void Start()
    {
        kills = 0;
        killsDisplayText.text = kills.ToString();
    }
    public void AddKill()
    {
        kills++;
        killsDisplayText.text = kills.ToString();
    }
    public int ReadKills()
    {
        return kills;
    }
        


}

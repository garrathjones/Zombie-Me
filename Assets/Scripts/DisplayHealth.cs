using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{
    [SerializeField] Text healthDisplayText;

    Player player;


    void Start()
    {
        player = FindObjectOfType<Player>();
    }


    void Update()
    {
        healthDisplayText.text = player.PlayerHealth().ToString();
    }
}

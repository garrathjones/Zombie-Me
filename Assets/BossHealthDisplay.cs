using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthDisplay : MonoBehaviour
{
    [SerializeField] Text zombieHealthDisplayText;

    ZombieHealth zombieHealth;


    void Start()
    {
        zombieHealth = FindObjectOfType<ZombieHealth>();
    }


    void Update()
    {
        zombieHealthDisplayText.text = zombieHealth.GetZombieHealth().ToString();
    }
}

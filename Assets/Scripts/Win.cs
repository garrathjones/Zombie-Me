using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    [SerializeField] Text winText;
    [SerializeField] Button menuButton;
    [SerializeField] TextMeshProUGUI menuButtonText;


    public void EnablewinUI()
    {
        winText.enabled = true;
        menuButton.enabled = true;
        menuButtonText.enabled = true;
    }
}

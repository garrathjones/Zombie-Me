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
    [SerializeField] Button retryButton;
    [SerializeField] TextMeshProUGUI retryText;

    public bool levelClear = false;

    public void EnablewinUI()
    {
        winText.enabled = true;
        menuButton.enabled = true;
        menuButtonText.enabled = true;
        retryButton.enabled = true;
        retryText.enabled = true;
        levelClear = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    [SerializeField] float yOffset = 0f;
    [SerializeField] float xOffset = 0f;
    Pause pause;
    

    private void Start()
    {
        pause = FindObjectOfType<Pause>();
    }
    void Update()
    {
        if(!pause.paused)
        {
            var pos = new Vector3(Input.mousePosition.x + xOffset, Input.mousePosition.y + yOffset, 10);
            pos = Camera.main.ScreenToWorldPoint(pos);
            transform.position = pos;
        }

    }
}

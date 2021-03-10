using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    void Update()
    {
        var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        pos = Camera.main.ScreenToWorldPoint(pos);
        transform.position = pos;
    }
}

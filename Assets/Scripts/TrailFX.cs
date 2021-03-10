using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFX : MonoBehaviour
{
    TrailRenderer trail;


    void Start()
    {
        trail = GetComponent<TrailRenderer>();
    }
    public void EnableTrailFX(bool enableOrDisable)
    {
        trail.emitting = enableOrDisable;
    }


}

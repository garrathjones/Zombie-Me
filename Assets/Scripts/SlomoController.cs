using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlomoController : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] float slomoChance = 30f;
    [SerializeField] float minSlomoTime = 0.5f;
    [SerializeField] float maxSlomoTime = 2f;
    
    public bool isSlomoEnabled = false;

    void Start()
    {
        SlomoOff();
    }

    public void RandomSlomo()
    {
        float randomChance = Random.Range(0, 100);
        float randomTime = Random.Range(minSlomoTime, maxSlomoTime);
        if (randomChance < slomoChance)
        {
            SlomoEvent(randomTime);
        }
    }
    public void SlomoOn()
    {
        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isSlomoEnabled = true;
    }

    public void SlomoOff()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        isSlomoEnabled = false;
    }

    public void ToggleSlomo()
    {
        if (!isSlomoEnabled)
        {
            SlomoOn();
        }
        else if (isSlomoEnabled)
        {
            SlomoOff();
        }
    }

    public void SlomoEvent(float slomoTime)
    {
        StartCoroutine(SloMoEventCoroutine(slomoTime));
    }

    IEnumerator SloMoEventCoroutine(float slomoTime)
    {
        SlomoOn();
        yield return new WaitForSeconds(slomoTime);
        SlomoOff();
    }
}

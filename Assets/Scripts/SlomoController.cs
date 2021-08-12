using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SlomoController : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] float slomoChance = 30f;
    [SerializeField] float minSlomoTime = 0.5f;
    [SerializeField] float maxSlomoTime = 2f;

    public bool isSlomoEnabled = false;

    GameObject cam;
    AudioLowPassFilter lowPassFilter;
    AudioChorusFilter chorusFilter;

    [SerializeField] AudioClip slomoOnSFX;
    [SerializeField] [Range(0, 1)] float slomoOnVolume = 1f;

    [SerializeField] AudioClip slomoOffSFX;
    [SerializeField] [Range(0, 1)] float slomoOffVolume = 1f;


    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
        lowPassFilter = cam.GetComponent<AudioLowPassFilter>();
        chorusFilter = cam.GetComponent<AudioChorusFilter>();
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
        lowPassFilter.enabled = true;
        chorusFilter.enabled = true;
        AudioSource.PlayClipAtPoint(slomoOnSFX, Camera.main.transform.position, slomoOnVolume);
        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isSlomoEnabled = true;

    }

    public void SlomoOff()
    {
        AudioSource.PlayClipAtPoint(slomoOffSFX, Camera.main.transform.position, slomoOffVolume);
        lowPassFilter.enabled = false;
        chorusFilter.enabled = false;
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

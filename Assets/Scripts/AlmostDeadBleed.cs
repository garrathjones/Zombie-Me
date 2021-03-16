using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmostDeadBleed : MonoBehaviour
{
    [SerializeField] float bleedingDuration = 5f;
    [SerializeField] float pulseRate = 0.5f;
    [SerializeField] GameObject bloodSplurt;

    bool bleeding = false;

    public void AlmostDeadBleeding()
    {
        bleeding = true;
        StartCoroutine(BodyPartBleedingCoroutine());
    }

    IEnumerator BodyPartBleedingCoroutine()
    {
        while (bleeding)
        {
            GameObject splurt = Instantiate(bloodSplurt, transform.position, transform.rotation);
            Destroy(splurt, bleedingDuration);
            yield return new WaitForSeconds(pulseRate);
        }
    }




}

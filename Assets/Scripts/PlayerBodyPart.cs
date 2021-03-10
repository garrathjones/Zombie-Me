using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyPart : MonoBehaviour
{    

    [SerializeField] float bleedingDuration = 5f;
    [SerializeField] float pulseRate = 0.5f;
    [SerializeField] GameObject bloodSplurt;

    bool bodyPartIsBleeding = false;


    private void OnJointBreak2D(Joint2D joint)
    {
        bodyPartIsBleeding = true;
        StartCoroutine(BodyPartBleedingCoroutine());
    }

    IEnumerator BodyPartBleedingCoroutine()
    {
        while (bodyPartIsBleeding)
        {
            GameObject splurt = Instantiate(bloodSplurt, transform.position, transform.rotation);
            Destroy(splurt, bleedingDuration);
            yield return new WaitForSeconds(pulseRate);
        }
    }

}

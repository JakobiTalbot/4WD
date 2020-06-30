using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    private float m_secondsToRescueAnimal = 2f;

    private IEnumerator RescueAnimal()
    {
        yield return new WaitForSeconds(m_secondsToRescueAnimal);

        // animal is rescued (play sound? particles?)
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FourWheelDrive>())
        {
            StartCoroutine(RescueAnimal());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FourWheelDrive>())
        {
            StopAllCoroutines();
        }
    }
}
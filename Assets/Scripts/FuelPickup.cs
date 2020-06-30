using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField]
    private float m_fuelAddOnPickup = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FourWheelDrive>())
        {
            other.GetComponent<FourWheelDrive>().AddFuel(m_fuelAddOnPickup);
            gameObject.SetActive(false);
        }
    }
}
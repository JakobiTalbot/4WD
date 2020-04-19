using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField]
    private float m_fuelAddOnPickup = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<FourWheelDrive>())
        {
            other.GetComponentInParent<FourWheelDrive>().AddFuel(m_fuelAddOnPickup);
        }
    }
}
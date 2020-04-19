using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayerEnterTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onPlayerEnterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<FourWheelDrive>())
            m_onPlayerEnterTrigger.Invoke();
    }
}
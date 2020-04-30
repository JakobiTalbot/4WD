using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireWall : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_velocity;

    void FixedUpdate()
    {
        transform.position += m_velocity * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<FourWheelDrive>())
        {
            GameManager.instance.GameOver("Consumed by Flames!");
        }
    }
}
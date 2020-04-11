using UnityEngine;
using System.Collections;

public class FourWheelDrive : MonoBehaviour
{
    [SerializeField]
	private float m_accelerateSpeed = 3000f;
    [SerializeField]
    private float m_rotateSpeed = 100f;
    [SerializeField]
	private GameObject m_wheelPrefab;

    private WheelCollider[] wheels;
    private Rigidbody m_rb;

    // here we find all the WheelColliders down in the hierarchy
    public void Start()
	{
        m_rb = GetComponent<Rigidbody>();
		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels[i];

			// create wheel shapes only when needed
			if (m_wheelPrefab != null)
			{
				var ws = Instantiate(m_wheelPrefab);
				ws.transform.parent = wheel.transform;
			}
		}
	}

	public void Update()
	{
		float torque = m_accelerateSpeed * Input.GetAxis("Vertical");
        float rotate = m_rotateSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;

        m_rb.AddTorque(new Vector3(0, 0, -rotate), ForceMode.Acceleration);

        foreach (WheelCollider wheel in wheels)
		{
			wheel.motorTorque = torque;

			// update visual wheels if any
			if (m_wheelPrefab) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose(out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild(0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}

		}
	}
}

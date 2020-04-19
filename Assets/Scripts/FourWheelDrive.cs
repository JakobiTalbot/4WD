using UnityEngine;
using System.Collections;

public class FourWheelDrive : MonoBehaviour
{
    [Header("Driving")]
    [SerializeField]
	private float m_accelerateSpeed = 3000f;
    [SerializeField]
    private float m_rotateSpeed = 100f;
    [SerializeField]
    private ForceMode m_turnForceMode;
    [SerializeField]
	private GameObject m_wheelPrefab;
    [SerializeField]
    private float m_startingFuelValue = 100f;
    [SerializeField]
    private float m_fuelConsumptionCoefficient = 1f;

    private UIManager m_uiManager;
    private GameManager m_gameManager;

    private WheelCollider[] wheels;
    private Rigidbody m_rb;
    private float m_currentFuel;
    private bool m_bCanDrive = true;

    // here we find all the WheelColliders down in the hierarchy
    public void Start()
	{
        m_rb = GetComponent<Rigidbody>();
		wheels = GetComponentsInChildren<WheelCollider>();
        m_currentFuel = m_startingFuelValue;
        m_uiManager = UIManager.instance;
        m_gameManager = GameManager.instance;

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
        if (m_bCanDrive)
        {
            // get player input
            float torque = m_accelerateSpeed * Input.GetAxis("Vertical");
            float rotate = m_rotateSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;

            // turn car chassis
            m_rb.AddRelativeTorque(new Vector3(rotate, 0, 0), m_turnForceMode);

            // spin wheels
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

            // reduce fuel
            m_currentFuel -= m_fuelConsumptionCoefficient * Time.deltaTime;
            m_uiManager.SetFuelValue(m_currentFuel, m_startingFuelValue);
            if (m_currentFuel <= 0f)
            {
                m_gameManager.GameOver("Out of Fuel!");
            }
        }
	}

    public void SetCanDrive(bool bCanDrive)
    {
        m_bCanDrive = bCanDrive;
    }

    public void AddFuel(float fuelAmount)
    {
        m_currentFuel += fuelAmount;
    }
}
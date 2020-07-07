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
    [SerializeField]
    private float m_curvedTrackTurnSpeed = 0.1f;
    [SerializeField]
    private float m_downforceCoefficient = 1f;

    private UIManager m_uiManager;
    private GameManager m_gameManager;

    private WheelCollider[] wheels;
    private Rigidbody m_rb;
    private float m_currentFuel;
    private bool m_bCanDrive = true;
    private bool m_bFollowCurvedTrack = false;

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

    private void FixedUpdate()
    {
        // downforce
        m_rb.AddForce(Vector3.down * m_rb.velocity.magnitude * m_downforceCoefficient);
    }

    public void SetCanDrive(bool bCanDrive)
    {
        m_bCanDrive = bCanDrive;
    }

    public void AddFuel(float fuelAmount)
    {
        m_currentFuel += fuelAmount;
    }

    public void SetFollowCurvedTrack(BezierSpline trackSpline, bool bFollowCurvedTrack)
    {
        m_bFollowCurvedTrack = bFollowCurvedTrack;

        if (m_bFollowCurvedTrack)
        {
            m_rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            StartCoroutine(FollowCurvedTrack(trackSpline));
        }
        else
        {
            m_rb.constraints |= RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
    }

    private IEnumerator FollowCurvedTrack(BezierSpline trackSpline)
    {
        while (m_bFollowCurvedTrack && m_bCanDrive)
        {
            Vector3 dir = Vector3.Lerp(transform.forward, trackSpline.GetDirection(trackSpline.ProjectPoint(transform.position)), m_curvedTrackTurnSpeed);
            m_rb.rotation = Quaternion.LookRotation(dir);
            m_rb.velocity = dir * m_rb.velocity.magnitude;

            yield return null;
        }
    }
}
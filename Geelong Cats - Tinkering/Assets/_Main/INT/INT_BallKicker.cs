using UnityEngine;

public class INT_BallKicker : MonoBehaviour
{
	public float m_height = 25;
	public bool debugPath;

	private Rigidbody m_ballRigidBody;
	private Vector3 m_target;

	private float m_gravity = 0;

	void Start()
	{
		m_ballRigidBody = GetComponent<Rigidbody>();
		m_gravity = Physics.gravity.y;
	}

	public void LaunchBall(Vector3 target)
    {
		m_target = target;
		Launch();
    }

	void Launch()
	{
		Physics.gravity = Vector3.up * m_gravity;
		m_ballRigidBody.useGravity = true;
		m_ballRigidBody.velocity = CalculateLaunchData().initialVelocity;
	}

	LaunchData CalculateLaunchData()
	{
		float displacementY = m_target.y - m_ballRigidBody.position.y;
		Vector3 displacementXZ = new Vector3(m_target.x - m_ballRigidBody.position.x, 0, m_target.z - m_ballRigidBody.position.z);
		float time = Mathf.Sqrt(-2 * m_height / m_gravity) + Mathf.Sqrt(2 * (displacementY - m_height) / m_gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * m_gravity * m_height);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(m_gravity), time);
	}

	void DrawPath()
	{
		LaunchData launchData = CalculateLaunchData();
		Vector3 previousDrawPoint = m_ballRigidBody.position;

		int resolution = 30;
		for (int i = 1; i <= resolution; i++)
		{
			float simulationTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * m_gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = m_ballRigidBody.position + displacement;
			Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
			previousDrawPoint = drawPoint;
		}
	}

	struct LaunchData
	{
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData(Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}

	}
}
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
	public Rigidbody m_prefab;

	public List<Rigidbody> m_flock;

	//Connor-------------------------------------
	[Range(1, 500)]
	public ushort m_amount = 10;
	public float m_spawnRadius = 25.0f;
	public float m_avoidanceRadius = 10.0f;
	public float m_maxVelocity = 10.0f;
	public float m_neighbourRadius = 10.0f;
	//Josh--------------------------------------
	public float m_seperationForceMag = 1.0f;
	public float m_cohesionForceMag = 100.0f;
	public float m_alighmentForceMult = 8.0f;


	

	// Start is called before the first frame update
	void Start()
	{
		for (int i = 0; i < m_amount; i++)
		{
			// Make random position
			Vector3 newPosition = Random.insideUnitSphere * m_spawnRadius;// * m_boidDensity
			// Overwrite y axis
			newPosition.y = 0;

			Rigidbody temp = Instantiate
				(
				 m_prefab,
				 newPosition,
				 Random.rotation, //Random.rotation,
				 transform
				);
			temp.name = "Boid " + i;

			//temp.velocity = new Vector3(temp.velocity.x, 0, temp.velocity.z)

			m_flock.Add(temp);
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		MoveBoidsToNewPos();
	}

	void MoveBoidsToNewPos()//GameObject a_boid, List<GameObject> a_flock)
	{
		Vector3 allForces = Vector3.zero;
		foreach (Rigidbody newBoid in m_flock)
		{
			Vector3 velocity = newBoid.velocity;
			allForces = CalForces(newBoid);
			velocity += allForces * Time.fixedDeltaTime;
			if (velocity.magnitude > m_maxVelocity)
			{
				velocity = velocity.normalized * m_maxVelocity;
			}

			newBoid.velocity = velocity;
		}
		Debug.Log("Allforce" + allForces);
	}

	Vector3 CalForces(Rigidbody a_boid)
	{
		Vector3 cohesionVector = Vector3.zero;
		Vector3 separationVector = Vector3.zero;
		Vector3 alignmentVector = Vector3.zero;

		// Gather all in radius
		Collider[] neighbours = Physics.OverlapSphere(a_boid.transform.position, m_neighbourRadius);

		for (int i = 0; i < neighbours.Length; i++)
		{
			// Get rigidbody of current boid
			Rigidbody rb = neighbours[i].GetComponent<Rigidbody>();
			if (rb != a_boid)
			{
				// Cohesion force
				cohesionVector += neighbours[i].transform.position;

				// Separation force
				if ((neighbours[i].transform.position - a_boid.transform.position).magnitude < m_avoidanceRadius)
				{
					separationVector -= (rb.transform.position - a_boid.transform.position) / m_seperationForceMag;
				}

				// Alignment force
				alignmentVector += rb.velocity;
			}
		}


		cohesionVector /= m_flock.Count - 1;

		cohesionVector = (cohesionVector - a_boid.transform.position) / m_cohesionForceMag;

		alignmentVector = (alignmentVector - a_boid.velocity) / m_alighmentForceMult;

		return cohesionVector + separationVector + alignmentVector;
	}
}

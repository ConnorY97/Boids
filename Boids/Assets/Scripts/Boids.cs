using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public GameObject m_prefab;

    public List<GameObject> m_flock;

    [Range(1, 100)]
    public ushort m_amount = 10;
    //public float m_boidDensity = 0.08f;

    public float radius = 25.0f;

    public float m_avoidanceRadius = 10.0f;

    public float m_maxVelocity = 10.0f;  

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_amount; i++)
        {
            GameObject temp = Instantiate
                (
                 m_prefab,
                 Random.insideUnitSphere * radius,// * m_boidDensity,
                 Random.rotation,
                 transform
                );
            temp.name = "Boid " + i;
            m_flock.Add(temp); 
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //while (!m_gamveOver)
            MoveBoidsToNewPos(); 
    }

    void MoveBoidsToNewPos()//GameObject a_boid, List<GameObject> a_flock)
    {
        Vector3 cohesionForce, seperationForce, alignmentForce = Vector3.zero;
        
        foreach (GameObject newboid in m_flock)
        {
            Vector3 velocity = newboid.GetComponent<Rigidbody>().velocity;
            cohesionForce = CalCohesionForce(newboid);
            seperationForce = CalSeperationForce(newboid);
            alignmentForce = CalAlignmentForce(newboid);
            velocity += cohesionForce + seperationForce + alignmentForce * Time.deltaTime;// + alignmentForce * Time.deltaTime;
            if (velocity.magnitude > m_maxVelocity)
            {
                velocity = velocity.normalized * m_maxVelocity; 
            }

            newboid.GetComponent<Rigidbody>().velocity = velocity;
            newboid.transform.rotation = Quaternion.LookRotation(velocity); 
            //newboid.transform.position += newboid.GetComponent<Rigidbody>().velocity;

            Debug.Log("Seperation" + seperationForce);
            Debug.Log("Cohesion" + cohesionForce);
            Debug.Log("Alignment" + alignmentForce);
        }
    }

    Vector3 CalCohesionForce(GameObject a_boid)
    {
        Vector3 pc = Vector3.zero;
        
        foreach (GameObject boid in m_flock)
        {
            if (boid != a_boid)
            {
                pc += boid.transform.position; 
            }
        }
        pc /= m_flock.Count - 1;
        
        return (pc - a_boid.transform.position) / 100; 
    }

    Vector3 CalSeperationForce(GameObject a_boid)
    {
        Vector3 c = Vector3.zero;
        foreach(GameObject boid in m_flock)
        {
            if (boid != a_boid)
            {
                if ((boid.transform.position - a_boid.transform.position).magnitude < m_avoidanceRadius)
                {
                    c -= boid.transform.position - a_boid.transform.position; 
                }
            }
        }
        return c; 
    }

    Vector3 CalAlignmentForce(GameObject a_boid)
    {
        Vector3 pv = Vector3.zero; 
        foreach(GameObject boid in m_flock)
        {
            if (boid != a_boid)
            {
                pv += boid.GetComponent<Rigidbody>().velocity;
            }
        }
        return (pv - a_boid.GetComponent<Rigidbody>().velocity) / 8; 
    }
}

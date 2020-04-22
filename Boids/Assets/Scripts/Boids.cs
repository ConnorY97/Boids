using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public GameObject m_prefab;

    public List<GameObject> m_flock; 

    [Range(10, 100)]
    public ushort m_amount = 10;
    //public float m_boidDensity = 0.08f;

    public float radius = 25.0f; 

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
    void Update()
    {
        while (!gameObject)
            MoveBoidsToNewPos(); 
    }

    void MoveBoidsToNewPos()//GameObject a_boid, List<GameObject> a_flock)
    {
        Vector3 rule1, rule2, rule3 = Vector3.zero;
        GameObject temp;
        Rigidbody rb = temp.GetComponent<Rigidbody>(); 
        foreach (GameObject boid in m_flock)
        {
            rule1 = Rule1(boid);
            rule2 = Rule2(boid);
            rule3 = Rule3(boid);

            rb.velocity += rule1 + rule2 + rule3; 
        }
        //Vector3 rule1 = Rule1(a_boid, a_flock);
        //Vector3 rule2 = Rule2(a_boid, a_flock);
        //Vector3 rule3 = Rule3(a_boid, a_flock);

        //Rigidbody rb = a_boid.GetComponent<Rigidbody>();

        //rb.velocity += rule1 + rule2 + rule3;
        //a_boid.transform.position += rb.velocity; 
    }

    Vector3 Rule1(GameObject a_boid)
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

    Vector3 Rule2(GameObject a_boid)
    {
        Vector3 c = Vector3.zero;
        foreach(GameObject boid in m_flock)
        {
            if (boid != a_boid)
            {
                Vector3 distanceApart = boid.transform.position - a_boid.transform.position;
                if (distanceApart.magnitude < 100)
                {
                    c -= distanceApart; 
                }
            }
        }
        return c; 
    }

    Vector3 Rule3(GameObject a_boid)
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

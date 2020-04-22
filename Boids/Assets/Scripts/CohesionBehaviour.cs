using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Boid))]
public class CohesionBehaviour : MonoBehaviour
{
    private Boid m_boid;

    public float m_radius; 

    // Start is called before the first frame update
    void Start()
    {
        m_boid = GetComponent<Boid>(); 
    }

    // Update is called once per frame
    void Update()
    {
        var allBoids = FindObjectsOfType<Boid>();

        var average = Vector3.zero;
        var found = 0;

        foreach(var newBoid in allBoids.Where(b => b != m_boid))
        {
            var diff = newBoid.transform.position - this.transform.position;
            if (diff.magnitude < m_radius)
            {
                average += diff;
                found += 1;
            }
        }

        if (found > 0)
        {
            average = average / found;
            m_boid.m_velocity = Vector3.Lerp(Vector3.zero, average, average.magnitude / m_radius);
        }
    }
}

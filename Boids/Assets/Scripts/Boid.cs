using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 m_velocity;

    public float m_maxVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_velocity.magnitude > m_maxVelocity)
            m_velocity = m_velocity.normalized * m_maxVelocity; 

        this.transform.position += m_velocity * Time.deltaTime;
        this.transform.rotation = Quaternion.LookRotation(m_velocity); 
    }
}

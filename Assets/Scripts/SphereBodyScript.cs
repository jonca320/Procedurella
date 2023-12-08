using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereBodyScript : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Sphere body properties")]
    [SerializeField]
    protected float mass;

    public float forceMultiplier = 1.0f;
    [SerializeField]
    protected Rigidbody rb;

    private float gravitationalForce;
    private float orbitalVelocity;
    public float centripetalForce;
    public SphereCollider sphereCollider;
    private float gravityForce = 6.67430e-11f; // Gravitational constant (m^3/kg/s^2)

    private float force;
    private Vector3 forceDirection;
    private Vector3 applicationForce;


    void Start()
    {
        // Calculate gravitational force
        gravitationalForce = (gravityForce * mass * mass) / (sphereCollider.radius* sphereCollider.radius);

        // Calculate orbital velocity
        orbitalVelocity = Mathf.Sqrt(gravitationalForce / mass);

        // Calculate centripetal force (for a circular orbit)
        centripetalForce = (mass * orbitalVelocity * orbitalVelocity) / sphereCollider.radius;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] currentMassBodies = GameObject.FindGameObjectsWithTag("MassBody");
        foreach (var body in currentMassBodies)
        {
            if (body.transform != this.transform)
            {
                SphereBodyScript otherBody = body.GetComponent<SphereBodyScript>();

                //Calculate force to apply
                Vector3 r = body.transform.position - transform.position;
                float distance = Mathf.Sqrt(r.x * r.x + r.y * r.y + r.z * r.z);
                float forceMagnitude = gravityForce*((mass * otherBody.mass) / (distance * distance));
                Vector3 force = forceMagnitude * (r / distance);

                //Normalized direction to apply force
                forceDirection = (otherBody.transform.position - transform.position).normalized;

                Vector3 massAcceleration = force / mass;
                // Update accelerations
               

                // Update velocities and positions
          

                applicationForce += massAcceleration*forceMultiplier;
            }
            
            
        }
        rb.AddForce(applicationForce);
        rb.AddForce(Vector3.forward* centripetalForce);
    }
}

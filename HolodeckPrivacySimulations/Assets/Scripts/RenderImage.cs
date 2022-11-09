using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderImage : MonoBehaviour
{
    public Rigidbody Drone;
    public Vector3 startPosition = new Vector3(0, 0, 1);
    public Vector3 targetPosition = new Vector3(0, 1.5f, 1);
    
    private bool startUp = false;
    private float speed = 0.5f;
    private float distanceToStop = 0.01f;
    private Vector3 desiredVelocity;
    private float lastEuclideanDist;
    private bool surfaceEmission = false;

    // Start is called before the first frame update
    void Start()
    {
        Drone.position = startPosition;

        desiredVelocity = (targetPosition - startPosition).normalized * speed;
        lastEuclideanDist = Mathf.Infinity;
    }

    // Update is called once per frame
    void Update()
    {
        // empty update function
    }

    void FixedUpdate() 
    {
        Drone.AddForce(0,9.80665f,0);
        if (startUp == true)
        {
            float EuclideanDist = (targetPosition - Drone.position).sqrMagnitude;

            if (EuclideanDist > distanceToStop)
            {
                var direction = targetPosition - Drone.position;
                Drone.AddRelativeForce(direction.normalized * speed, ForceMode.Force);
            }

            // Drone.position = targetPosition;

            if ((EuclideanDist < distanceToStop) && !surfaceEmission)
            {
                this.transform.Find("Surface").GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            }
        }
    }

    public void InitializeDrone()
    {
        startUp = true;
    }
}

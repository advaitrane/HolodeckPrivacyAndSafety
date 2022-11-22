using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderImage : MonoBehaviour
{
    public Rigidbody Drone;
    public Vector3 startPosition = new Vector3(0, 0, 1);
    public Vector3 targetPosition = new Vector3(0, 1.5f, 1);
    public bool trackHand = false;
    public GameObject handMarker;
    public Vector3 targetPosition2 = new Vector3(0, 1.5f, 1);
    public Vector3 targetPosition3 = new Vector3(0, 1.5f, 1);
    
    private bool startUp = false;
    private bool shutDown = false;
    private float speed = 0.5f;
    private float distanceToStop = 0.01f;
    private Vector3 desiredVelocity;
    private float lastEuclideanDist;
    private bool surfaceEmission = false;
    private bool atTarget = false;
    private float trackSpeed = 0.1f;
    private float trackDistanceToStop = 0.0000001f;

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
        if (startUp == true && atTarget == false)
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
                surfaceEmission = true;
                atTarget = true;
            }
        }

        if (shutDown == true)
        {
            float EuclideanDist = (startPosition - Drone.position).sqrMagnitude;

            if (EuclideanDist > distanceToStop)
            {
                var direction = startPosition - Drone.position;
                Drone.AddRelativeForce(direction.normalized * speed, ForceMode.Force);
            }
        }

        if (atTarget == true && trackHand)
        {   
            Vector3 markerPosition = handMarker.transform.position;

            float targetDist = (markerPosition - targetPosition).sqrMagnitude;
            float targetDist2 = (markerPosition - targetPosition2).sqrMagnitude;
            float targetDist3 = (markerPosition - targetPosition3).sqrMagnitude;

            Vector3 trackPosition;
            float minDist = -1f;
            if (targetDist < targetDist2)
            {
                trackPosition = targetPosition;
                minDist = targetDist;
            }
            else
            {
                trackPosition = targetPosition2;
                minDist = targetDist2;
            }
            if (minDist > targetDist3)
            {
                trackPosition = targetPosition3;
                minDist = targetDist3;
            }

            float EuclideanDist = (trackPosition - Drone.position).sqrMagnitude;

            if (EuclideanDist > trackDistanceToStop)
            {
                var direction = trackPosition - Drone.position;
                Drone.AddRelativeForce(direction.normalized * trackSpeed, ForceMode.Force);

            }
        }
    }

    public void InitializeDrone()
    {
        startUp = true;
    }

    public void ShutDownDrone()
    {
        startUp = false;
        shutDown = true;
        this.transform.Find("Surface").GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        surfaceEmission = false;
    }
}

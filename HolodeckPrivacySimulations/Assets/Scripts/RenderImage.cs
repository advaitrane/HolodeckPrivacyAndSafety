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
    public Vector3[] targetPositions;
    // private Vector3 targetPosition = targetPositions[0];

    // public Vector3 targetPosition2 = new Vector3(0, 1.5f, 1);
    // public Vector3 targetPosition3 = new Vector3(0, 1.5f, 1);
    
    private bool startUp = false;
    private bool shutDown = false;
    private float speed = 1.5f;
    private float distanceToStop = 0.005f;
    private Vector3 desiredVelocity;
    private float lastEuclideanDist;
    private bool surfaceEmission = false;
    private bool atTarget = false;
    private float trackSpeed = 1f;
    private float trackDistanceToStop = 0.0001f;
    private int currPosIdx = 0;
    private int bestPosIdx = -1;
    private bool trackTimeStart = false;

    float trackStartTime, trackEndTime, trackTime;

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
        Drone.AddForce(0,9.81f,0);
        if (startUp == true && atTarget == false)
        {
            float EuclideanDist = (targetPosition - Drone.position).sqrMagnitude;

            if (EuclideanDist > distanceToStop)
            {
                var direction = targetPosition - Drone.position;
                // Drone.AddRelativeForce(direction.normalized * speed, ForceMode.Force);
                Drone.AddRelativeForce(direction * speed, ForceMode.Force);

            }

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

            Vector3 trackPosition = new Vector3(0, 0, 0);
            float minDist = 1000f;
            float targetDist = 0f;
            for (int i = 0; i < targetPositions.Length; i++)
            {
                targetDist = (markerPosition - targetPositions[i]).sqrMagnitude;
                if (targetDist < minDist)
                {
                    trackPosition = targetPositions[i];
                    minDist = targetDist;
                    bestPosIdx = i;
                }
            }

            // float targetDist = (markerPosition - targetPosition).sqrMagnitude;
            // float targetDist2 = (markerPosition - targetPosition2).sqrMagnitude;
            // float targetDist3 = (markerPosition - targetPosition3).sqrMagnitude;

            // Vector3 trackPosition;
            // float minDist = -1f;
            // if (targetDist < targetDist2)
            // {
            //     trackPosition = targetPosition;
            //     minDist = targetDist;
            //     bestPosIdx = 1;
            // }
            // else
            // {
            //     trackPosition = targetPosition2;
            //     minDist = targetDist2;
            //     bestPosIdx = 2;
            // }
            // if (minDist > targetDist3)
            // {
            //     trackPosition = targetPosition3;
            //     minDist = targetDist3;
            //     bestPosIdx = 3;
            // }

            if (currPosIdx != bestPosIdx)
            {
                if(!trackTimeStart)
                {
                    trackStartTime = Time.time;
                    // Debug.Log("Track start time - ");
                    // Debug.Log(trackStartTime);
                    trackTimeStart = true;
                }
                float EuclideanDist = (trackPosition - Drone.position).sqrMagnitude;

                if (EuclideanDist > trackDistanceToStop)
                {
                    var direction = trackPosition - Drone.position;
                    // Drone.AddRelativeForce(direction.normalized * trackSpeed, ForceMode.Force);
                    Drone.AddRelativeForce(direction * trackSpeed, ForceMode.Force);
                }
                else
                {
                    currPosIdx = bestPosIdx;
                    if(trackTimeStart)
                    {
                        trackEndTime = Time.time;
                        trackTime = trackEndTime - trackStartTime;
                        // Debug.Log("Track time - ");
                        // Debug.Log(trackTime);
                        trackTimeStart = false;
                    }
                }
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

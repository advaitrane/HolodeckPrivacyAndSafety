using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHolodeck : MonoBehaviour
{
    public GameObject holodeck;
    public Vector3 startUpPosition = new Vector3(0, 0, 0);

    private float distanceError = 0.3f;
    private bool startUp = false;

    // Start is called before the first frame update
    void Start()
    {
        // empty start function
    }

    // Update is called once per frame
    void Update()
    {
        float startUpDist = (transform.position - startUpPosition).sqrMagnitude;
        if ((startUpDist < distanceError) && !startUp)
        {
            holodeck.GetComponent<StartUp>().Initialize();
            startUp = true;
        }
    }
}

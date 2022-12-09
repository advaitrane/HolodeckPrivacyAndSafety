using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision)
    {
    //    Debug.Log("entered");
    //    this.gameObject.SetActive(false);
    //    if (collision.gameObject.tag == "Man")
    //     {
    //        this.gameObject.SetActive(false);
    //        Debug.Log("entered");
           
    //     }
        Debug.Log(collision.gameObject.tag);
        Debug.Log("entered");
        // Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;
        // Debug.Log(Collider.impactForceSum);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // void OnCollisionEnter(Collision collision)
    // {
    //    Debug.Log("Player Oncollision entered");
    //    if (collision.gameObject.tag == "Player")
    //     {
    //        collision.gameObject.SetActive(false);
           
           
    //     }
      
    // }



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
        Debug.Log("Player OnTrigger entered");
      
      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMotor : MonoBehaviour
{

    [SerializeField] private GameObject Man;
    private CharacterController controller;
    [SerializeField] private float speed = 0F;
    public bool decel = false;

    // void increaseSpeed()
    // {
    //     if(decel == true)
    //     speed = speed - 0.5F;
    //     if(decel == false)
    //     speed = speed +0.5F;
        



    //     float max_speed = 16;
        
    
    //     float dist = Vector3.Distance(Man.transform.position, transform.position);
    //     // Debug.Log("Dist"+ dist+"  Speed:"+speed);
    //     if(dist<5F){
    //         decel =true;
    //     }
    //     Debug.Log("ppppp");
    //     Debug.Log(speed);

    //     // if(speed <0){
    //     //     CancelInvoke("increaseSpeed");
    //     // }
    //     if(speed > max_speed){
            
    //         speed = max_speed;
    //     }
        

        


    // }

    // private Vector3 moveVector;
    // private float verticalVelocity = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        // InvokeRepeating("increaseSpeed", 0f, 0.1f); 
    }

    // Update is called once per frame
    void Update()
    
    {   
        float acceleration=0.5F;
        float dist = Vector3.Distance(Man.transform.position, transform.position);
        Debug.Log(dist);
        Debug.Log(speed);

        
        if(speed < 16F){
        speed += acceleration * Time.deltaTime;


    }
transform.position = Vector3.MoveTowards(transform.position,Man.transform.position,speed);
            
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //    Debug.Log("entered");
    //    this.gameObject.SetActive(false);
    //    if (collision.gameObject.tag == "Man")
    //     {
    //        this.gameObject.SetActive(false);
    //        Debug.Log("entered");
           
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
        Debug.Log("speed");
        // Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;
        // Debug.Log(Collider.impactForceSum);
        Debug.Log(speed);
        Time.timeScale = 0;
        Application.Quit();

    }

}

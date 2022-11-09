using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutDown : MonoBehaviour
{
    public bool shutDown;
    // Start is called before the first frame update
    void Start()
    {
        shutDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        // empty update function
    }

    public void ShutDownDisplay()
    {
        if (shutDown == false)
        {
            shutDown = true;
            int numDrones = this.gameObject.transform.childCount;
            for (int i = 0; i < numDrones; i++)
            {
                RenderImage ri = this.gameObject.transform.GetChild(i).GetComponent<RenderImage>();
                if (ri)
                {
                    ri.ShutDownDrone();
                }
                MonitorYOLOv4 myv4 = this.gameObject.transform.GetChild(i).GetComponent<MonitorYOLOv4>();
                if (myv4)
                {
                    myv4.ShutDownDrone();
                }
            }
        }
    }
}

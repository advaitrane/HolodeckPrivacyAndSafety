using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    public bool start;

    // Start is called before the first frame update
    void Start()
    {
        start = false;
    }

    // Update is called once per frame
    void Update()
    {
        // empty update function
    }

    public void Initialize()
    {
        if (start == false)
        {
            start = true;
            int numDrones = this.gameObject.transform.childCount;
            for (int i = 0; i < numDrones; i++)
            {
                this.gameObject.transform.GetChild(i).GetComponent<RenderImage>().InitializeDrone();
            }
        }
    }
}

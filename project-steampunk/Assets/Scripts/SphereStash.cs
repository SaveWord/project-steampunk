using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereStash : MonoBehaviour

{
    public int speed;
    public GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        // Temporary vector
        Vector3 temp = player.transform.position;
        temp.x = temp.x;
        temp.y = temp.y;
        temp.z = temp.z-1;

        // Assign value to Camera position
        transform.position = temp;
        //dtransform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);

    }

}
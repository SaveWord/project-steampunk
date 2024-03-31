using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    
    public void DoorOpen()
    {
        transform.position += new Vector3(transform.position.x,
            transform.position.y,transform.position.z + 10);
    }
    public void DoorClose()
    {
        Debug.Log("DoorClose");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWheel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(transform, true);
            
        }
            
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerrotate = other.gameObject.transform.rotation.eulerAngles;
            playerrotate.z = 0;
            playerrotate.x = 0;
            other.gameObject.transform.rotation = Quaternion.Euler(playerrotate);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.gameObject.transform.SetParent(null);
    }
}

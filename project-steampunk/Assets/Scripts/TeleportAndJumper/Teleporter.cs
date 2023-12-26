using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform nextTeleport;
    private float nextTimeTP;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nextTimeTP = Time.time + 2f;
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (nextTimeTP < Time.time)
            other.transform.position = nextTeleport.transform.position;
    }
}

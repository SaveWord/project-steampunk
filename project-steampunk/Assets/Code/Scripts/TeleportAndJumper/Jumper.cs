using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private float forceJumper;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Rigidbody rb;
           rb = other.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * forceJumper, ForceMode.Impulse);
        }
    }
}

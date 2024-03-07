using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

public class LiftToBossArena : MonoBehaviour
{
    private bool inLift;
    private Rigidbody rb;
    private Vector3 curPosition;
    private Vector3 playerPosition;
    public float heightStep;
    public float heightDestination;
    public AudioSource source;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&transform.position.y<heightDestination)
        {
            inLift = true;
            curPosition = transform.position;
            rb = other.GetComponent<Rigidbody>();
            source.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inLift = false;
            source.Stop();
        }     
    }

    void FixedUpdate()
    {
        if (inLift&&transform.position.y<heightDestination)
        {
            curPosition.y += heightStep;
            transform.position = curPosition;
            playerPosition = rb.position;
            playerPosition.y += heightStep;
            rb.position = playerPosition;
        }


    }
}

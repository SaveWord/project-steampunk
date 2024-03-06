using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
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
    // Start is called before the first frame update
    void Start()
    {
        curPosition= playerPosition=transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&curPosition.y<heightDestination)
        {
            source.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            source.Stop();
        }     
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inLift=true;
            rb=other.GetComponent<Rigidbody>();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (inLift&&transform.position.y<heightDestination)
        {
            curPosition.y += heightStep;
            playerPosition = curPosition;
            playerPosition.y += 2;
            transform.position = curPosition;
            rb.position = playerPosition;
        }


    }
}

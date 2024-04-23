using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

public class LiftToBossArena : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] float speed;
    int lastPoint;
    int curPoint;
    bool inLift;
    Rigidbody rb;
    AudioSource source;

    private void Start()
    {
        curPoint = 0;
        transform.position = points[curPoint].position;
        source = GetComponent<AudioSource>();
        lastPoint = points.Length - 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && curPoint!=lastPoint)
        {
            inLift = true;
            rb = other.GetComponent<Rigidbody>();
            rb.transform.SetParent(transform);
            source.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.transform.SetParent(null);
            inLift = false;
            source.Stop();
        }     
    }

    void Update()
    {
        if (inLift && curPoint!=lastPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[curPoint+1].position,speed*Time.deltaTime);
            if(transform.position == points[curPoint+1].position)
            {
                curPoint++;
            }
        }
    }
}

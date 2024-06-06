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
    AudioSource source;
    Vector3 playerposition;
    Transform player;
    bool inLift;

    private void Start()
    {
        curPoint = 0;
        transform.position = points[curPoint].position;
        source = GetComponent<AudioSource>();
        lastPoint = points.Length - 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform.position != points[lastPoint].position)
        {
            inLift = true;
            player = other.transform;
            //other.gameObject.transform.SetParent(transform);
            source.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inLift = false;
            //other.gameObject.transform.SetParent(null);
            source.Stop();
        }
    }
    private void LateUpdate()
    {
        if (inLift && transform.position != points[lastPoint].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[lastPoint].position, speed * Time.deltaTime);
            player.GetComponent<CharacterControllerMove>().gravityDownForce = 0f;
            playerposition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            player.position = playerposition;
        }
        else
        {
            player.GetComponent<CharacterControllerMove>().gravityDownForce = 20f;
        }
        if (!inLift && transform.position != points[lastPoint].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[curPoint].position, speed * Time.deltaTime);
        }
    }
    //private void FixedUpdate()
    //{
    //    if (inLift && transform.position != points[lastPoint].position)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, points[lastPoint].position, speed * Time.deltaTime);
    //        player.GetComponent<CharacterControllerMove>().gravityDownForce = 0f;
    //        playerposition = new Vector3(player.position.x, transform.position.y + 1, player.position.z);
    //        player.position = playerposition;
    //    }
    //    else
    //    {
    //        player.GetComponent<CharacterControllerMove>().gravityDownForce = 20f;
    //    }
    //    if (!inLift && transform.position != points[lastPoint].position)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, points[curPoint].position, speed * Time.deltaTime);
    //    }
    //}
    //void Update()
    //{
    //    if (inLift && transform.position != points[lastPoint].position)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, points[lastPoint].position, speed * Time.deltaTime);
    //        player.GetComponent<CharacterControllerMove>().gravityDownForce = 0f;
    //        playerposition = new Vector3(player.position.x, transform.position.y + 1, player.position.z);
    //        player.position = playerposition;
    //    }
    //    else
    //    {
    //        player.GetComponent<CharacterControllerMove>().gravityDownForce = 20f;
    //    }
    //    if (!inLift && transform.position != points[lastPoint].position)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, points[curPoint].position, speed * Time.deltaTime);
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWheel : MonoBehaviour
{
    bool inLift;
    Transform player;
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        collision.gameObject.transform.SetParent(transform, true);

    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inLift = true;
            other.gameObject.transform.SetParent(transform, true);
            player = other.gameObject.transform;
        }

    }
    private void FixedUpdate()
    {
        if (inLift)
        {
            Vector3 playerrotate = player.rotation.eulerAngles;
            playerrotate.z = 0;
            playerrotate.x = 0;
            player.rotation = Quaternion.Euler(playerrotate);
        }
    }
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Vector3 playerrotate = collision.gameObject.transform.rotation.eulerAngles;
    //        playerrotate.z = 0;
    //        playerrotate.x = 0;
    //        collision.gameObject.transform.rotation = Quaternion.Euler(playerrotate);
    //    }
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Vector3 playerrotate = other.gameObject.transform.rotation.eulerAngles;
    //        playerrotate.z = 0;
    //        playerrotate.x = 0;
    //        other.gameObject.transform.rotation = Quaternion.Euler(playerrotate);
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //        collision.gameObject.transform.SetParent(null);
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inLift = false;
            other.gameObject.transform.SetParent(null);
        }
    }
}

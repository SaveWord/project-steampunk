using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class DeathByPlatform : MonoBehaviour
{
    //[SerializeField] Transform deathposition;
    [SerializeField] float offset;
    [SerializeField] float speed;
    [SerializeField] float Damage = 100f;
    // Vector3 deathVector;
    // AudioSource source;
    Animator animator;
    //bool once=true;
    private void Start()
    {
        //deathVector=deathposition.position;
       // source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.Play("CrashingPlatform", 0, offset);
        animator.speed = speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HpHandler>().TakeDamage(Damage);
        }
        //if (other.gameObject.layer == 6 && other.GetType() == typeof(CapsuleCollider))
        //{
        //    Destroy(other.gameObject);
        //}
    }

        //}
        // Start is called before the first frame update
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Player")&&once)
        //    {
        //        deathVector.x=other.transform.position.x;
        //        deathVector.z=other.transform.position.z;
        //        once = false;
        //        source.Play();
        //        StartCoroutine(waitForSound(other));
        //    }
        //    if (other.gameObject.layer==6 && other.GetType()==typeof(CapsuleCollider))
        //    {         
        //        Destroy(other.gameObject);
        //    }

        //}
        //IEnumerator waitForSound(Collider other)
        //{       
        //    while (source.isPlaying)
        //    {
        //        other.transform.position = deathVector;
        //        other.GetComponent<Rigidbody>().Sleep();
        //        yield return null;
        //    }
        //    other.GetComponent<HpHandler>().TakeDamage(1000f);
        //}
    }

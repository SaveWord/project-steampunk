using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class Doors0Close : MonoBehaviour
{
    [SerializeField] GameObject Doors;
    [SerializeField] float speed;
    Animator animator;
    bool open;
    private void Awake()
    {
        animator = Doors.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {          
            animator.enabled = true;
            animator.Play("Doors0Close",0);
            animator.speed = speed;
        }
    }
}

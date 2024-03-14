using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform0Controller : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator=GetComponent<Animator>();
        animator.enabled = false;
    }
    void disableAnimation()
    {
        animator.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurbineController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    HpHandler hp;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        animator.speed = speed;
    }
    public void collisionEvent(Collider other)
    {
        hp =other.GetComponent<HpHandler>();
        hp.TakeDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMove : MonoBehaviour
{
    [SerializeField] private float speedTrail;
    private Vector3 startPosition;
    private Rigidbody rbTrail;
    private void Awake()
    {
        rbTrail = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        rbTrail.velocity = transform.forward * speedTrail;

    }
    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        rbTrail.velocity = Vector3.zero;
    }
}

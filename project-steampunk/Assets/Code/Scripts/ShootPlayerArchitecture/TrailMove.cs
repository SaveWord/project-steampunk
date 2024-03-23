using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMove : MonoBehaviour
{
    [SerializeField] private float speedTrail;
    [SerializeField] private float timerLife;
    private float lifeTime;
    private Vector3 startPosition;
    private Rigidbody rbTrail;


    //reset time, because trail get position and draw zig-zag line
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        rbTrail = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (lifeTime < Time.time)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        trailRenderer.Clear();
        rbTrail.velocity = transform.forward * speedTrail;
        lifeTime = Time.time + timerLife;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
            this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        rbTrail.velocity = Vector3.zero;
    }
}

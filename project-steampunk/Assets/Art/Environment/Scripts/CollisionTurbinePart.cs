using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTurbinePart : MonoBehaviour
{
    [SerializeField] GameObject turbine;
    TurbineController controller;
    private void Start()
    {
        controller = turbine.GetComponent<TurbineController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.collisionEvent(other);
        }
    }
}

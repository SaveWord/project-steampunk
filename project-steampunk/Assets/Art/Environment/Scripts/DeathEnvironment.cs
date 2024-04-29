using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnvironment : MonoBehaviour
{
    [SerializeField] float Damage=100f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HpHandler>().TakeDamage(Damage);
        }
        if (other.gameObject.layer == 6 && other.GetType() == typeof(CapsuleCollider))
        {
            Destroy(other.gameObject);
        }
    }
}

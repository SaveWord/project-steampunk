using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnvironment : MonoBehaviour
{
    [SerializeField] float Damage=100f;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HpHandler>().TakeDamage(Damage);
        }
    }
}

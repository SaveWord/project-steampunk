using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodDamageProps : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float damageRate;
    private float nextDamage;

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player") && Time.time > nextDamage)
        {
            nextDamage = Time.time + damageRate;
            collision.gameObject.TryGetComponent(out IHealth damageableZone);
            damageableZone?.TakeDamage(damage);
        }
    }
}

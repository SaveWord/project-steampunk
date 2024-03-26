using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComponent : MonoBehaviour
{
    public bool isDamaging=false;
    public float _damage;
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("shosh " + collision.gameObject.GetInstanceID());

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && isDamaging)
        {
            DealDamage(collision.gameObject);
            isDamaging = false;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("shosh " + collision.gameObject.GetInstanceID());

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && isDamaging)
        {
            DealDamage(collision.gameObject);
            isDamaging = false;
        }

    }
    private void DealDamage(GameObject target)
    {
        //var damageable = target.GetComponent<IHealth>();
        // damageable.TakeDamage(_damage);
        target.TryGetComponent(out IHealth damageable);
        damageable?.TakeDamage(_damage);
        Debug.Log("attack from pizza");
    }
}

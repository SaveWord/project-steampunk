using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComponent : MonoBehaviour
{
    public bool isDamaging=false;
    public float _damage;
    private bool _damageCooldown = false;

    private void OnTriggerEnter(Collider collision)
    {
        Damage(collision);
    }
    private void OnTriggerStay(Collider collision)
    {
        Damage(collision);
    }

    private void Damage(Collider collision)
    {
        Debug.Log("shosh " + collision.gameObject.GetInstanceID());

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && isDamaging && !_damageCooldown)
        {
            if (isDamaging)
            {
                DealDamage(collision.gameObject, _damage);
                isDamaging = false;
            }
            else
            {
                //DealDamage(collision.gameObject, _damage / 2);
            }
            DamageReload();
        }
    }
    private IEnumerator DamageReload()
    {
        _damageCooldown = true;
        yield return new WaitForSeconds(0.2f);
        _damageCooldown = false;
    }

    private void DealDamage(GameObject target, float damage)
    {
        //var damageable = target.GetComponent<IHealth>();
        // damageable.TakeDamage(_damage);
        target.TryGetComponent(out IHealth damageable);
        damageable?.TakeDamage(damage);
        Debug.Log("attack from pizza");
    }
}

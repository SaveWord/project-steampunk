using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnv : MonoBehaviour
{
    [Header("Attack Parametres")]

    public float _damage = 2;


    [Header("Visual Parametres")]
    [SerializeField]
    private Material _targetMat;

    void Update()
    {
       // _particle = GetComponent<ParticleSystem>();
       // var rend = _particle.GetComponent<ParticleSystemRenderer>();
       // rend.material = _targetMat;
    }
    protected void DealDamage(GameObject target)
    {
        target.TryGetComponent(out IHealth damageable);
        damageable?.TakeDamage(_damage);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DealDamage(collision.gameObject);
        }
    }
}

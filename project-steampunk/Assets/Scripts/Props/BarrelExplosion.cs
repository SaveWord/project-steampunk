using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BarrelExplosion : EnemyGetDamage
{
    [SerializeField] private float damage;
    [SerializeField] private float rangeExplosion;
    private ParticleSystem explosionParticle;
    private void Awake()
    {
        
    }
    protected override void Start()
    {
        base.Start();
        explosionParticle = GetComponentInChildren<ParticleSystem>();
    }


    //get damage and if die give damage
    private void GiveDamageIfDie()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, rangeExplosion);
        for (int i =0; i < hitColliders.Length-1; i++)
        {
            if (hitColliders[i].CompareTag("enemy") || hitColliders[i].CompareTag("Player"))
            {
                hitColliders[i].TryGetComponent(out IDamageable damageable);
                damageable.GetDamage(damage);
            }
        }
    }
    public override void GetDamage(float damage)
    {
        enemyHP -= damage;
        sliderHP.value = enemyHP;
        StartCoroutine(HPView());
        if (enemyHP <= 0)
        {
            GiveDamageIfDie();
            StartCoroutine(DestroyBarrel());
        }
    }
    IEnumerator DestroyBarrel()
    {
        explosionParticle.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Color tmp = Color.red;
        tmp.a = 0.5f;
        Gizmos.color = tmp;
        Gizmos.DrawSphere(transform.position, rangeExplosion);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float maxDistanceRay;
    [SerializeField] private LayerMask playerMask;
    private RaycastHit hitPlayer;
    private void FixedUpdate()
    {
        AttackPlayer();
        //OnDrawGizmos();
    }
    private  void AttackPlayer()
    {
        if (Physics.BoxCast(transform.position, transform.localScale/2, transform.forward,
            out hitPlayer, transform.rotation, maxDistanceRay, playerMask))
        {
            Debug.Log(hitPlayer);
            hitPlayer.collider.TryGetComponent(out IDamageable damageable);
            damageable.GetDamage(damage);
        }
    }
   /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * maxDistanceRay);

        Gizmos.DrawWireCube(transform.position + transform.forward *
            maxDistanceRay, transform.localScale);
    }
   */
}

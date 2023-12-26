using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMove : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int idPlayerLayer;
    [SerializeField] private float damage;
    public float speed;
    public void BulletMove()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == idPlayerLayer)
        {
            if (collision.gameObject.GetComponent<HealthPoint>().parry == false)
            {
                collision.gameObject.TryGetComponent(out IDamageable damageable);
                damageable.GetDamage(damage);
                Destroy(gameObject);
            }
            else if (collision.gameObject.GetComponent<HealthPoint>().parry == true)
            {
                transform.rotation = Camera.main.transform.rotation;
                rb.velocity = transform.forward * speed;
            }
        }
        else
            Destroy(gameObject);
    }
}

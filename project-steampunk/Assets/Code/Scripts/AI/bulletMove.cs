using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMove : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int idPlayerLayer;
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    public float speed;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == idPlayerLayer)
        {
            collision.gameObject.TryGetComponent(out IDamageable damageable);
            damageable.GetDamage(damage);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        transform.position = Vector3.zero;
    }
}

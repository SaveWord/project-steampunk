using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_damage : MonoBehaviour, damage_interface
{
    [SerializeField] private float _damage = 0.2f;
    [SerializeField] private float timerR = 2f;
    [SerializeField] private float damageInterval = 2f;
    private float damageTimer = 0f;

    public float getDamage()
    {
        return _damage;
    }
    public string getstate()
    {
        return "";
    }
    void Update()
    {
        timerR -= Time.deltaTime;
        if (timerR < 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter!");
        if (other.CompareTag("killzone"))
        {
            earth_damage earth = other.gameObject.GetComponent<earth_damage>();

            if (earth != null)
            {
                Destroy(gameObject);
            }
            water_damage water = other.gameObject.GetComponent<water_damage>();

            if (water != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        collider.TryGetComponent(out IHealth damageable);
        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            damageable?.TakeDamage(_damage);
            damageTimer = 0f;
        }
    }
}

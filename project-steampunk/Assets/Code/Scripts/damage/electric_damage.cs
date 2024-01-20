using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electric_damage : MonoBehaviour, damage_interface
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float timerR = 6f;
    [SerializeField] private float damageInterval = 1f;
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
            water_damage water = other.gameObject.GetComponent<water_damage>();

            if (water != null)
            {
                Destroy(gameObject);
            }

            earth_damage earth = other.gameObject.GetComponent<earth_damage>();

            if (earth != null)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        other.gameObject.TryGetComponent(out health_abstract health);
        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            health?.TakeDamage(_damage);
            damageTimer = 0f;
        }
    }
    public float gettime()
    {
        return timerR;
    }
}
